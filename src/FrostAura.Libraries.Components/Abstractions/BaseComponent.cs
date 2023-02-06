using FrostAura.Libraries.Components.Data.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

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
        /// The current version of the component.
        /// </summary>
        public abstract Version Version { get; }
        /// <summary>
        /// Whether to enable demo defaults for this component.
        /// </summary>
        [Parameter]
        public bool EnableDemoMode { get; set; }
        /// <summary>
        /// Service to manipulate and fetch content from the the client-space. Including fetching configuration.
        /// </summary>
        [Inject]
        protected IClientDataAccess ClientDataAccess { get; set; }
        /// <summary>
        /// Navigation manager.
        /// </summary>
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        /// <summary>
        /// Instance logger.
        /// </summary>
        [Inject]
        protected ILogger<TComponentType> Logger { get; set; }
        /// <summary>
        /// JavaScript runtime engine.
        /// </summary>
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }
    }
}

