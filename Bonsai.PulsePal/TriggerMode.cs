namespace Bonsai.PulsePal
{
    /// <summary>
    /// Specifies the behavior of a trigger channel.
    /// </summary>
    public enum TriggerMode
    {
        /// <summary>
        /// Rising edge triggers linked channels, and subsequent trigger
        /// pulses are ignored.
        /// </summary>
        Normal,

        /// <summary>
        /// Rising edge triggers linked channels, and subsequent trigger
        /// pulses stop linked channels.
        /// </summary>
        Toggle,

        /// <summary>
        /// Rising edge triggers linked channels, and falling edge
        /// stops linked channels.
        /// </summary>
        PulseGated
    }
}
