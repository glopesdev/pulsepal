using System.ComponentModel;
using Bonsai.IO.Ports;
using Bonsai.PulsePal.Configuration;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents configuration settings used to program the operation of
    /// a Pulse Pal device.
    /// </summary>
    public class PulsePalConfiguration
    {
        internal static readonly PulsePalConfiguration Default = new();

        /// <summary>
        /// Gets or sets the name of the serial port.
        /// </summary>
        [Description("The name of the serial port.")]
        [TypeConverter(typeof(SerialPortNameConverter))]
        public string PortName { get; set; }

        /// <summary>
        /// Gets the collection of parameters used to program the operation of the Pulse Pal device.
        /// </summary>
        [Description("The collection of parameters used to program the operation of the Pulse Pal device.")]
        public ChannelParameterConfigurationCollection ChannelParameters { get; } = new();
    }
}
