using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the voltage for the first
    /// phase of each pulse.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.Phase1Voltage))]
    public class Phase1VoltageConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the voltage for the first phase of each pulse.
        /// </summary>
        [XmlAttribute]
        [Range(MinVoltage, MaxVoltage)]
        [Precision(VoltageDecimalPlaces, VoltageIncrement)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The voltage for the first phase of each pulse.")]
        public double Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetPhase1Voltage(Channel, Value);
        }
    }
}
