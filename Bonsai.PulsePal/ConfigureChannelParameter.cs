using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents an operator that configures a single channel parameter on a
    /// Pulse Pal device.
    /// </summary>
    [DefaultProperty(nameof(Parameter))]
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
    [Description("Configures a single channel parameter on a Pulse Pal device.")]
    public class ConfigureChannelParameter : PolymorphicCombinatorBuilder
    {
        static readonly Range<int> argumentRange = Range.Create(lowerBound: 1, upperBound: 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureChannelParameter"/> class.
        /// </summary>
        public ConfigureChannelParameter()
        {
            Parameter = new BiphasicConfiguration();
        }

        /// <inheritdoc/>
        public override Range<int> ArgumentRange => argumentRange;

        /// <summary>
        /// Gets or sets the operator used to configure specific Pulse Pal channel parameters.
        /// </summary>
        [DesignOnly(true)]
        [Externalizable(false)]
        [RefreshProperties(RefreshProperties.All)]
        [Category(nameof(CategoryAttribute.Design))]
        [Description("The operator used to configure specific Pulse Pal channel parameters.")]
        [TypeConverter(typeof(CombinatorTypeConverter))]
        public object Parameter
        {
            get { return Operator; }
            set { Operator = value; }
        }

        /// <inheritdoc/>
        public override Expression Build(IEnumerable<Expression> arguments)
        {
            return arguments.Single();
        }
    }
}
