using FrostAura.Libraries.Components.Interfaces.Navigation;
using FrostAura.Libraries.Components.Models.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FrostAura.Libraries.Components.Abstractions
{
    /// <summary>
    /// FrostAura base component for core and shared functionality.
    /// </summary>
    public abstract class BaseComponent<TComponentType> : ComponentBase
    {
        /// <summary>
        /// Unique component instance identifier.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// The current version of the component. This could be used in cache busting.
        /// </summary>
        public virtual Version Version { get; } = new Version(0, 0, 1);
        /// <summary>
        /// Whether to enable demo defaults for this component.
        /// </summary>
        [Parameter]
        public bool EnableDemoMode { get; set; }
        /// <summary>
        /// Application configuration.
        /// </summary>
        [Inject]
        protected FrostAuraApplicationConfiguration Configuration { get; set; }
        /// <summary>
        /// Navigation manager.
        /// </summary>
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        /// <summary>
        /// FrostAura navigation manager.
        /// </summary>
        [Inject]
        protected INavigationService NavigationService { get; set; }
        /// <summary>
        /// Instance logger.
        /// </summary>
        [Inject]
        protected ILogger<TComponentType> Logger { get; set; }
    }
}

