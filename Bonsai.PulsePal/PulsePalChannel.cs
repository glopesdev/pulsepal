using System.Xml.Serialization;

namespace Bonsai.PulsePal
{
    public enum PulsePalChannel : byte
    {
        [XmlEnum("1")] Channel1 = 1,
        [XmlEnum("2")] Channel2 = 2,
        [XmlEnum("3")] Channel3 = 3,
        [XmlEnum("4")] Channel4 = 4,
    }
}
