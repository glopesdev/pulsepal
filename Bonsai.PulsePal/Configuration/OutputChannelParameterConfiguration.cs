using System.ComponentModel;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Provides an abstract base class for configuring Pulse Pal
    /// output channel parameters.
    /// </summary>
    public abstract class OutputChannelParameterConfiguration : ChannelParameterConfiguration
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
    }
}
