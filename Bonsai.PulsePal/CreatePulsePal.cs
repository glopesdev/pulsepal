using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai.IO.Ports;
using Bonsai.PulsePal.Configuration;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents an operator that creates and configures a Pulse Pal device.
    /// </summary>
    [DefaultProperty(nameof(ChannelParameters))]
    [Description("Creates and configures a Pulse Pal device.")]
    public class CreatePulsePal : Source<PulsePal>, INamedElement
    {
        readonly PulsePalConfiguration configuration = new();

        /// <summary>
        /// Gets or sets the optional alias for the Pulse Pal device.
        /// </summary>
        [Description("The optional alias for the Pulse Pal device.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the serial port used to communicate with the
        /// Pulse Pal device.
        /// </summary>
        [TypeConverter(typeof(SerialPortNameConverter))]
        [Description("The name of the serial port used to communicate with the Pulse Pal device.")]
        public string PortName
        {
            get => configuration.PortName;
            set => configuration.PortName = value;
        }

        /// <summary>
        /// Gets the collection of channel parameters used to program the Pulse Pal.
        /// </summary>
        [Description("The collection of channel parameters used to program the Pulse Pal.")]
        [Editor("Bonsai.Resources.Design.CollectionEditor, Bonsai.System.Design", DesignTypes.UITypeEditor)]
        public ChannelParameterConfigurationCollection ChannelParameters => configuration.ChannelParameters;

        /// <summary>
        /// Generates an observable sequence that contains the created serial interface.
        /// </summary>
        /// <returns>
        /// A sequence containing a single instance of the <see cref="PulsePal"/> class
        /// representing the serial interface to the Pulse Pal device.
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
