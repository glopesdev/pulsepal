using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying whether the output channel
    /// will loop its custom pulse train.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.CustomTrainLoop))]
    public class CustomTrainLoopConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying whether the output channel
        /// will loop its custom pulse train.
        /// </summary>
        [XmlAttribute]
        [Description("Specifies whether the output channel will loop its custom pulse train.")]
        public bool Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetCustomTrainLoop(Channel, Value);
        }
    }
}
