using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Provides an abstract base class for Pulse Pal channel parameters.
    /// </summary>
    public abstract class OutputChannelParameterConfiguration
    {
        internal const double MinVoltage = -10;
        internal const double MaxVoltage = 10;
        internal const int VoltageDecimalPlaces = 3;
        internal const double VoltageIncrement = 0.001;
        internal const double MinTimePeriod = 0.0001;
        internal const double MaxTimePeriod = 3600;
        internal const int TimeDecimalPlaces = 4;

        /// <summary>
        /// Gets or sets a value specifying the output channel to configure.
        /// </summary>
        [Description("Specifies the output channel to configure.")]
        public OutputChannel Channel { get; set; } = OutputChannel.Channel1;

        /// <summary>
        /// Applies the channel parameter configuration to the specified
        /// Pulse Pal device.
        /// </summary>
        /// <param name="device">The Pulse Pal device to configure.</param>
        public abstract void Configure(PulsePal device);
    }
}
