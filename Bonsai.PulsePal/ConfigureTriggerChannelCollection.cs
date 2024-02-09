using System.Collections.ObjectModel;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents a collection of trigger channel configuration objects.
    /// </summary>
    public class ConfigureTriggerChannelCollection : KeyedCollection<TriggerChannel, ConfigureTriggerChannel>
    {
        /// <summary>
        /// Returns the key for the specified configuration object.
        /// </summary>
        /// <param name="item">The configuration object from which to extract the key.</param>
        /// <returns>The key for the specified configuration object.</returns>
        protected override TriggerChannel GetKeyForItem(ConfigureTriggerChannel item)
        {
            return item.Channel;
        }
    }
}
