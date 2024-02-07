using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the interpretation of pulse times
    /// in the custom train configured on this output channel.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.CustomTrainTarget))]
    public class CustomTrainTargetConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying the interpretation of pulse times in the
        /// custom pulse train.
        /// </summary>
        [XmlAttribute]
        [Description("Specifies the interpretation of pulse times in the custom pulse train.")]
        public CustomTrainTarget Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetCustomTrainTarget(Channel, Value);
        }
    }
}
