using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents an operator that creates a sequence of custom pulse trains.
    /// </summary>
    [DefaultProperty(nameof(PulseTrain))]
    [Description("Creates a sequence of custom pulse trains.")]
    public class CustomPulseTrain : Combinator<PulseOnset[]>
    {
        /// <summary>
        /// Gets the collection of pulse train onset times and voltages
        /// specifying the custom pulse train.
        /// </summary>
        [Description("The collection of pulse train onset times and voltages specifying the custom pulse train.")]
        public Collection<PulseOnset> PulseTrain { get; } = new();

        /// <summary>
        /// Creates an observable sequence containing a single custom pulse train.
        /// </summary>
        /// <returns>
        /// A sequence containing a single array of pulse onset times and voltages
        /// representing the custom pulse train.
        /// </returns>
        public IObservable<PulseOnset[]> Process()
        {
            return Process(Observable.Return(Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of custom pulse trains where each train
        /// is created whenever another sequence emits a notification.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting new custom
        /// pulse trains.
        /// </param>
        /// <returns>
        /// A sequence containing arrays of pulse onset times and voltages representing
        /// the custom pulse trains.
        /// </returns>
        public override IObservable<PulseOnset[]> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => PulseTrain.ToArray());
        }
    }
}
