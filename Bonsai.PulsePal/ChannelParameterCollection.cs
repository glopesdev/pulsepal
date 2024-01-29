using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.PulsePal
{
    [Editor("Bonsai.PulsePal.Design.ChannelParameterCollectionEditor, Bonsai.PulsePal.Design", typeof(UITypeEditor))]
    public class ChannelParameterCollection : Collection<ChannelParameter>
    {
    }
}
