using System;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Specifies which output channels to start on a soft-trigger command.
    /// </summary>
    [Flags]
    public enum TriggerChannels
    {
        /// <summary>
        /// Start stimulation on output channel 1.
        /// </summary>
        Channel1 = 0x1,

        /// <summary>
        /// Start stimulation on output channel 2.
        /// </summary>
        Channel2 = 0x2,

        /// <summary>
        /// Start stimulation on output channel 3.
        /// </summary>
        Channel3 = 0x4,

        /// <summary>
        /// Start stimulation on output channel 4.
        /// </summary>
        Channel4 = 0x8
    }
}
