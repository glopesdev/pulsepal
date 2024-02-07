using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the resting voltage on this
    /// output channel, i.e. the voltage between phases, pulses and pulse trains.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.RestingVoltage))]
    public class RestingVoltageConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the resting voltage, in the range [-10, 10] volts.
        /// </summary>
        [XmlAttribute]
        [Range(MinVoltage, MaxVoltage)]
        [Precision(VoltageDecimalPlaces, VoltageIncrement)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The resting voltage.")]
        public double Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetRestingVoltage(Channel, Value);
        }
    }
}
