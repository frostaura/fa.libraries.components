using FrostAura.Libraries.Core.Interfaces.Reactive;

namespace FrostAura.Standard.Components.Razor.Interfaces.Navigation
{
    /// <summary>
    /// Interface for a service to control navigational features.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Subscribable event steam for the page title.
        /// 
        /// Default value: string.Empty.
        /// </summary>
        IObservedValue<string> PageTitleStream { get; }
        /// <summary>
        /// Navigate on the client-side to a specified URL.
        /// </summary>
        /// <param name="url">URL to navigate to on the client-side.</param>
        void NavigateClientTo(string url);
    }
}
