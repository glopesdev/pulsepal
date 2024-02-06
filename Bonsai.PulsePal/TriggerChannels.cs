using System;

namespace Bonsai.PulsePal
{
    [Flags]
    public enum TriggerChannels
    {
        Channel1 = 0x1,
        Channel2 = 0x2,
        Channel3 = 0x4,
        Channel4 = 0x8
    }
}
