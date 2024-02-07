using System.Xml.Serialization;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Specifies the trigger channel on a Pulse Pal device.
    /// </summary>
    public enum TriggerChannel : byte
    {
        /// <summary>
        /// The trigger channel 1.
        /// </summary>
        [XmlEnum("1")] Channel1 = 1,

        /// <summary>
        /// The trigger channel 2.
        /// </summary>
        [XmlEnum("2")] Channel2 = 2,
    }
}
