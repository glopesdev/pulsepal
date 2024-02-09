using System.ComponentModel;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents configuration parameters specifying the identity of the custom
    /// train used to specify pulse times and voltages on an output channel.
    /// </summary>
    [DisplayName(nameof(ParameterCode.CustomTrainIdentity))]
    public class CustomTrainIdentityConfiguration : OutputChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying the identity of the custom pulse train
        /// to use on this output channel.
        /// </summary>
        [Description("Specifies the identity of the custom pulse train to use on this output channel.")]
        public CustomTrainId CustomTrainIdentity { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetCustomTrainIdentity(Channel, CustomTrainIdentity);
        }
    }
}
