using System.ComponentModel;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents configuration parameters specifying the resting voltage on this
    /// output channel, i.e. the voltage between phases, pulses and pulse trains.
    /// </summary>
    [DisplayName(nameof(ParameterCode.RestingVoltage))]
    public class RestingVoltageConfiguration : OutputChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets the resting voltage, in the range [-10, 10] volts.
        /// </summary>
        [Range(MinVoltage, MaxVoltage)]
        [Precision(VoltageDecimalPlaces, VoltageIncrement)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The resting voltage.")]
        public double RestingVoltage { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetRestingVoltage(Channel, RestingVoltage);
        }
    }
}
