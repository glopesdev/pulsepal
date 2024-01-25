using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.PulsePal
{
    public class SetClientId : Sink<string>
    {
        [TypeConverter(typeof(PortNameConverter))]
        [Description("The name of the serial port used to communicate with the Pulse Pal.")]
        public string PortName { get; set; }

        public override IObservable<string> Process(IObservable<string> source)
        {
            return Observable.Using(
                () => PulsePalManager.ReserveConnection(PortName),
                pulsePal => source.Do(val => {
                    lock (pulsePal.PulsePal)
                    {
                        pulsePal.PulsePal.SetClientId(val);
                    }
                })
            );
        }
    }
}
