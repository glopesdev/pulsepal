using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bonsai.PulsePal
{
    public class SetDisplay : Sink<string>
    {
        [TypeConverter(typeof(PortNameConverter))]
        [Description("The name of the serial port used to communicate with the Pulse Pal.")]
        public string PortName { get; set; }

        public override IObservable<string> Process(IObservable<string> source)
        {
            return Observable.Using(
                cancellationToken => PulsePalManager.ReserveConnectionAsync(PortName),
                (connection, CancellationToken) =>
                {
                    return Task.FromResult(source.Do(value =>
                    {
                        lock (connection.PulsePal)
                        {
                            connection.PulsePal.SetDisplay(value);
                        }
                    }));
                }
            );
        }
    }
}
