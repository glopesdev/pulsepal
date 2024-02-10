using System.ComponentModel;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Provides an abstract base class for configuring Pulse Pal
    /// trigger channel parameters.
    /// </summary>
    public abstract class TriggerChannelParameterConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying the output channel to configure.
        /// </summary>
        [Description("Specifies the output channel to configure.")]
        public TriggerChannel Channel { get; set; } = TriggerChannel.Channel1;
    }
}
