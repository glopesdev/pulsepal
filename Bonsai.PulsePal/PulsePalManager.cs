using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace Bonsai.PulsePal
{
    internal static class PulsePalManager
    {
        static readonly Dictionary<string, Tuple<PulsePal, RefCountDisposable>> openConnections = new();
        static readonly object openConnectionsLock = new();

        internal static PulsePalDisposable ReserveConnection(string portName)
        {
            return ReserveConnection(portName, PulsePalConfiguration.Default);
        }

        internal static PulsePalDisposable ReserveConnection(string portName, PulsePalConfiguration pulsePalConfiguration)
        {
            var connection = default(Tuple<PulsePal, RefCountDisposable>);
            lock (openConnectionsLock)
            {
                if (string.IsNullOrEmpty(portName))
                {
                    if (!string.IsNullOrEmpty(pulsePalConfiguration.PortName)) portName = pulsePalConfiguration.PortName;
                    else if (openConnections.Count == 1) connection = openConnections.Values.Single();
                    else throw new ArgumentException("An alias or serial port name must be specified.", nameof(portName));
                }

                if (connection == null && !openConnections.TryGetValue(portName, out connection))
                {
                    var serialPortName = pulsePalConfiguration.PortName;
                    if (string.IsNullOrEmpty(serialPortName)) serialPortName = portName;

                    var pulsePal = new PulsePal(serialPortName);
                    try
                    {
                        pulsePal.Open();
                        pulsePal.SetClientId(nameof(Bonsai));
                        pulsePalConfiguration.Configure(pulsePal);
                        var dispose = Disposable.Create(() =>
                        {
                            pulsePal.Close();
                            openConnections.Remove(portName);
                        });

                        var refCount = new RefCountDisposable(dispose);
                        connection = Tuple.Create(pulsePal, refCount);
                        openConnections.Add(portName, connection);
                        return new PulsePalDisposable(pulsePal, refCount);
                    }
                    catch
                    {
                        pulsePal.Close();
                        throw;
                    }
                }
            }

            return new PulsePalDisposable(connection.Item1, connection.Item2.GetDisposable());
        }
    }
}
