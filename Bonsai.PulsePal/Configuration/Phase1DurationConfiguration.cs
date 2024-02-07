using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the duration for the first
    /// phase of each pulse.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.Phase1Duration))]
    public class Phase1DurationConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the duration of the first phase of the pulse, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [XmlAttribute]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The duration of the first phase of the pulse, in seconds.")]
        public double Value { get; set; } = MinTimePeriod;

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetPhase1Duration(Channel, Value);
        }
    }
}
