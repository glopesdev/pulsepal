using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bonsai.IO.Ports;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents an operator that configures trigger channel parameters on a Pulse Pal device.
    /// </summary>
    [Description("Configures trigger channel parameters on a Pulse Pal device.")]
    public class ConfigureTriggerChannel : Sink
    {
        const string ChannelCategory = "Channel";

        /// <summary>
        /// Gets or sets the name of the serial port used to communicate with the
        /// Pulse Pal device.
        /// </summary>
        [Category(ChannelCategory)]
        [TypeConverter(typeof(SerialPortNameConverter))]
        [Description("The name of the serial port used to communicate with the Pulse Pal device.")]
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the output channel to configure.
        /// </summary>
        [Category(ChannelCategory)]
        [Description("The trigger channel to configure.")]
        public TriggerChannel Channel { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the behavior of the trigger channel.
        /// </summary>
        [Category(ChannelCategory)]
        [Description("Specifies the behavior of the trigger channel.")]
        public TriggerMode TriggerMode { get; set; }

        /// <summary>
        /// Configures the trigger channel parameters on the Pulse Pal device whenever
        /// an observable sequence emits a notification.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used to trigger configuration
        /// of the Pulse Pal trigger channel.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of configuring the
        /// trigger channel parameters on the Pulse Pal device whenever the sequence
        /// emits a notification.
        /// </returns>
        public override IObservable<TSource> Process<TSource>(IObservable<TSource> source)
        {
            return Observable.Using(
                cancellationToken => PulsePalManager.ReserveConnectionAsync(PortName),
                (connection, cancellationToken) => Task.FromResult(source.Do(input =>
                {
                    lock (connection.PulsePal)
                    {
                        Configure(connection.PulsePal);
                    }
                })));
        }

        internal void Configure(PulsePal pulsePal)
        {
            var channel = Channel;
            pulsePal.SetTriggerMode(channel, TriggerMode);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Channel == 0 ? nameof(ConfigureTriggerChannel) : $"{Channel}";
        }
    }
}
