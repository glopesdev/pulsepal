using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Bonsai.IO;

namespace Bonsai.PulsePal
{
    public class CreatePulsePal : Source<PulsePal>, INamedElement
    {
        readonly PulsePalConfiguration configuration = new PulsePalConfiguration();

        [Description("The optional alias for the Pulse Pal.")]
        public string Name { get; set; }

        [TypeConverter(typeof(SerialPortNameConverter))]
        [Description("The name of the serial port used to communicate with the Pulse Pal.")]
        public string PortName
        {
            get { return configuration.PortName; }
            set { configuration.PortName = value; }
        }

        public override IObservable<PulsePal> Generate()
        {
            return Observable.Using(
                () => PulsePalManager.ReserveConnection(configuration.PortName),
                resource =>
                {
                    return Observable.Return(resource.PulsePal)
                                     .Concat(Observable.Never(resource.PulsePal));
                }
            );
        }
    }
}
