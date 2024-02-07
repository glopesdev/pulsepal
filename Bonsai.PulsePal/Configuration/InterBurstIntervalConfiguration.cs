using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the duration of the
    /// off-time between bursts.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.InterBurstInterval))]
    public class InterBurstIntervalConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the duration of the off-time between bursts, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [XmlAttribute]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The duration of the off-time between bursts, in seconds.")]
        public double Value { get; set; } = MinTimePeriod;

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetInterBurstInterval(Channel, Value);
        }
    }
}
