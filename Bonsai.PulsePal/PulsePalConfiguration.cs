using System.ComponentModel;
using Bonsai.IO;

namespace Bonsai.PulsePal
{
    public class PulsePalConfiguration
    {
        internal static readonly PulsePalConfiguration Default = new PulsePalConfiguration();

        readonly ChannelParameterCollection channelParameters = new ChannelParameterCollection();

        [Description("The name of the serial port.")]
        [TypeConverter(typeof(SerialPortNameConverter))]
        public string PortName { get; set; }

        [Description("The collection of parameters used to specify operation of individual channels.")]
        public ChannelParameterCollection ChannelParameters
        {
            get { return channelParameters; }
        }
    }
}
