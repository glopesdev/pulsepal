using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the duration of a pulse
    /// burst when using burst mode.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.BurstDuration))]
    public class BurstDurationConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the duration of a pulse burst, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [XmlAttribute]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The duration of a pulse burst, in seconds.")]
        public double Value { get; set; } = MinTimePeriod;

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetBurstDuration(Channel, Value);
        }
    }
}
