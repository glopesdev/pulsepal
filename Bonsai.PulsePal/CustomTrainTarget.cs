namespace Bonsai.PulsePal
{
    /// <summary>
    /// Specifies the interpretation of pulse times in a custom pulse train.
    /// </summary>
    public enum CustomTrainTarget
    {
        /// <summary>
        /// Pulse times specify the pulse onset.
        /// </summary>
        PulseOnset,

        /// <summary>
        /// Pulse times specify pulse burst onsets.
        /// </summary>
        BurstOnset
    }
}
