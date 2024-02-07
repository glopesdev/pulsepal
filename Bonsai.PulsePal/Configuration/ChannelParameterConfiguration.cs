using System.ComponentModel;
using System.Xml.Serialization;
using Bonsai.Expressions;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Provides an abstract base class for Pulse Pal channel parameters.
    /// </summary>
    [XmlInclude(typeof(BiphasicConfiguration))]
    [XmlInclude(typeof(Phase1VoltageConfiguration))]
    [XmlInclude(typeof(Phase2VoltageConfiguration))]
    [XmlInclude(typeof(Phase1DurationConfiguration))]
    [XmlInclude(typeof(InterPhaseIntervalConfiguration))]
    [XmlInclude(typeof(Phase2DurationConfiguration))]
    [XmlInclude(typeof(InterPulseIntervalConfiguration))]
    [XmlInclude(typeof(BurstDurationConfiguration))]
    [XmlInclude(typeof(InterBurstIntervalConfiguration))]
    [XmlInclude(typeof(PulseTrainDurationConfiguration))]
    [XmlInclude(typeof(PulseTrainDelayConfiguration))]
    [XmlInclude(typeof(TriggerOnChannel1Configuration))]
    [XmlInclude(typeof(TriggerOnChannel2Configuration))]
    [XmlInclude(typeof(CustomTrainIdentityConfiguration))]
    [XmlInclude(typeof(CustomTrainTargetConfiguration))]
    [XmlInclude(typeof(CustomTrainLoopConfiguration))]
    [XmlInclude(typeof(RestingVoltageConfiguration))]
    [XmlInclude(typeof(TriggerModeConfiguration))]
    [XmlType(Namespace = Constants.XmlNamespace)]
    public abstract class ChannelParameterConfiguration
    {
        internal const double MinVoltage = -10;
        internal const double MaxVoltage = 10;
        internal const int VoltageDecimalPlaces = 3;
        internal const double VoltageIncrement = 0.001;
        internal const double MinTimePeriod = 0.0001;
        internal const double MaxTimePeriod = 3600;
        internal const int TimeDecimalPlaces = 4;

        /// <summary>
        /// Gets or sets a value specifying the output channel to configure.
        /// </summary>
        [XmlAttribute]
        [Description("Specifies the output channel to configure.")]
        public PulsePalChannel Channel { get; set; } = PulsePalChannel.Channel1;

        /// <summary>
        /// Applies the channel parameter configuration to the specified
        /// Pulse Pal device.
        /// </summary>
        /// <param name="device">The Pulse Pal device to configure.</param>
        public abstract void Configure(PulsePal device);

        /// <inheritdoc/>
        public override string ToString()
        {
            var type = GetType();
            var displayName = ExpressionBuilder.GetElementDisplayName(type);
            var value = type.GetProperty("Value")?.GetValue(this);
            return $"{displayName}({Channel}, {value})";
        }
    }
}
