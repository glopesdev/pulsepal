using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai.IO.Ports;

namespace Bonsai.PulsePal
{
    [Description("Creates and configures a serial connection to a Pulse Pal device.")]
    public class CreatePulsePal : Source<PulsePal>, INamedElement
    {
        readonly PulsePalConfiguration configuration = new PulsePalConfiguration();

        [Description("The optional alias for the Pulse Pal device.")]
        public string Name { get; set; }

        [TypeConverter(typeof(SerialPortNameConverter))]
        [Description("The name of the serial port used to communicate with the Pulse Pal.")]
        public string PortName
        {
            get { return configuration.PortName; }
            set { configuration.PortName = value; }
        }

        [Description("The collection of channel parameters used to configure the Pulse Pal.")]
        public ChannelParameterCollection ChannelParameters
        {
            get { return configuration.ChannelParameters; }
        }

        /// <summary>
        /// Generates an observable sequence that contains the serial connection object.
        /// </summary>
        /// <returns>
        /// A sequence containing a single instance of the <see cref="PulsePal"/> class
        /// representing the serial connection.
        /// </returns>
        public override IObservable<PulsePal> Generate()
        {
            return Observable.Using(
                () => PulsePalManager.ReserveConnection(Name, configuration),
                resource =>
                {
                    return Observable.Return(resource.PulsePal)
                                     .Concat(Observable.Never(resource.PulsePal));
                }
            );
        }
    }
}
