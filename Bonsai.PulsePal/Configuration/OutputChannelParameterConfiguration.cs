﻿using System.ComponentModel;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Provides an abstract base class for configuring Pulse Pal
    /// output channel parameters.
    /// </summary>
    public abstract class OutputChannelParameterConfiguration : ChannelParameterConfiguration
    {
        internal const double MinVoltage = PulsePal.MinVoltage;
        internal const double MaxVoltage = PulsePal.MaxVoltage;
        internal const double MinTimePeriod = PulsePal.MinTimePeriod;
        internal const double MaxTimePeriod = PulsePal.MaxTimePeriod;
        internal const int VoltageDecimalPlaces = 3;
        internal const double VoltageIncrement = 0.001;
        internal const int TimeDecimalPlaces = 4;

        /// <summary>
        /// Gets or sets a value specifying the output channel to configure.
        /// </summary>
        [Description("Specifies the output channel to configure.")]
        public OutputChannel Channel { get; set; } = OutputChannel.Channel1;
    }
}
