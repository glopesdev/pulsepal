using System.Xml.Serialization;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Specifies the output channel on a Pulse Pal device.
    /// </summary>
    public enum OutputChannel : byte
    {
        /// <summary>
        /// The output channel 1.
        /// </summary>
        [XmlEnum("1")] Channel1 = 1,

        /// <summary>
        /// The output channel 2.
        /// </summary>
        [XmlEnum("2")] Channel2 = 2,

        /// <summary>
        /// The output channel 3.
        /// </summary>
        [XmlEnum("3")] Channel3 = 3,

        /// <summary>
        /// The output channel 4.
        /// </summary>
        [XmlEnum("4")] Channel4 = 4,
    }
}
