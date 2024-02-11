using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Bonsai.PulsePal
{
    [Description("Triggers the specified output channels to begin playing their pulse sequences.")]
    public class TriggerOutput : Sink
    {
        [TypeConverter(typeof(PortNameConverter))]
        [Description("The name of the serial port used to communicate with the Pulse Pal device.")]
        public string PortName { get; set; }

        [Description("A value representing the bitmask of channels to trigger.")]
        public byte Channels { get; set; }

        public override IObservable<TSource> Process<TSource>(IObservable<TSource> source)
        {
            return Observable.Using(
                () => PulsePalManager.ReserveConnection(PortName),
                pulsePal => source.Do(input =>
                {
                    lock (pulsePal.PulsePal)
                    {
                        pulsePal.PulsePal.TriggerChannels((TriggerChannels)Channels);
                    }
                }));
        }
    }
}
