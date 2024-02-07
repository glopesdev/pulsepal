using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the interval between the first
    /// and second phases of biphasic pulses.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.InterPhaseInterval))]
    public class InterPhaseIntervalConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the interval between the first and second phase of a biphasic pulse,
        /// in the range [0.0001, 3600] seconds.
        /// </summary>
        [XmlAttribute]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The interval between the first and second phase of a biphasic pulse, in seconds.")]
        public double Value { get; set; } = MinTimePeriod;

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetInterPhaseInterval(Channel, Value);
        }
    }
}
