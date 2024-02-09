using System.ComponentModel;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Provides an abstract base class for Pulse Pal channel parameters.
    /// </summary>
    public abstract class TriggerChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying the output channel to configure.
        /// </summary>
        [Description("Specifies the output channel to configure.")]
        public TriggerChannel Channel { get; set; } = TriggerChannel.Channel1;

        /// <summary>
        /// Applies the channel parameter configuration to the specified
        /// Pulse Pal device.
        /// </summary>
        /// <param name="device">The Pulse Pal device to configure.</param>
        public abstract void Configure(PulsePal device);
    }
}
