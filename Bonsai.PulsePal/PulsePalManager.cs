using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Bonsai.PulsePal
{
    internal static class PulsePalManager
    {
        public const string DefaultConfigurationFile = "PulsePal.config";
        static readonly Dictionary<string, Tuple<PulsePal, RefCountDisposable>> openConnections = new Dictionary<string, Tuple<PulsePal, RefCountDisposable>>();
        static readonly object openConnectionsLock = new object();

        public static PulsePalDisposable ReserveConnection(string portName)
        {
            return ReserveConnection(portName, PulsePalConfiguration.Default);
        }

        public static async Task<PulsePalDisposable> ReserveConnectionAsync(string portName)
        {
            return await Task.Run(() => ReserveConnection(portName, PulsePalConfiguration.Default));
        }

        internal static PulsePalDisposable ReserveConnection(string portName, PulsePalConfiguration pulsePalConfiguration)
        {
            var connection = default(Tuple<PulsePal, RefCountDisposable>);
            lock (openConnectionsLock)
            {
                if (string.IsNullOrEmpty(portName))
                {
                    if (!string.IsNullOrEmpty(pulsePalConfiguration.PortName)) portName = pulsePalConfiguration.PortName; // override the port name if the configuration has already provided one
                    else if (openConnections.Count == 1) connection = openConnections.Values.Single();
                    else throw new ArgumentException("An alias or serial port name must be specified.", nameof(portName));
                }

                if (connection == null && !openConnections.TryGetValue(portName, out connection)) {
                    var serialPortName = pulsePalConfiguration.PortName;
                    if (string.IsNullOrEmpty(serialPortName)) serialPortName = portName;

#pragma warning disable CS0612 // Type or member is obsolete
                    var configuration = LoadConfiguration();
                    if (configuration.Contains(serialPortName))
                    {
                        pulsePalConfiguration = configuration[serialPortName];
                    }
#pragma warning restore CS0612 // Type or member is obsolete

                    // TODO - check fw / gen of pulse pal here and create pulse pal object using a factory
                    var pulsePal = new PulsePal(serialPortName);
                    pulsePal.Open();
                    pulsePal.SetClientId("Bonsai");
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
            }

            return new PulsePalDisposable(connection.Item1, connection.Item2.GetDisposable());
        }

        [Obsolete]
        public static PulsePalConfigurationCollection LoadConfiguration()
        {
            if (!File.Exists(DefaultConfigurationFile))
            {
                return new PulsePalConfigurationCollection();
            }

            var serializer = new XmlSerializer(typeof(PulsePalConfigurationCollection));
            using (var reader = XmlReader.Create(DefaultConfigurationFile))
            {
                return (PulsePalConfigurationCollection)serializer.Deserialize(reader);
            }
        }

        public static void SaveConfiguration(PulsePalConfigurationCollection configuration)
        {
            var serializer = new XmlSerializer(typeof(PulsePalConfigurationCollection));
            using (var writer = XmlWriter.Create(DefaultConfigurationFile, new XmlWriterSettings { Indent = true }))
            {
                serializer.Serialize(writer, configuration);
            }
        }
    }
}
