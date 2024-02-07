using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the interval between pulses.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.InterPulseInterval))]
    public class InterPulseIntervalConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the interval between pulses, in the range [0.0001, 3600] seconds.
        /// </summary>
        [XmlAttribute]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The interval between pulses, in seconds.")]
        public double Value { get; set; } = MinTimePeriod;

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetInterPulseInterval(Channel, Value);
        }
    }
}
