using FrostAura.Standard.Components.Razor.Interfaces.Navigation;
using FrostAura.Standard.Components.Razor.Models.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;

namespace FrostAura.Standard.Components.Razor.Abstractions
{
    /// <summary>
    /// FrostAura base component for core and shared functionality.
    /// </summary>
    public abstract class BaseComponent<TComponentType> : ComponentBase
    {
        /// <summary>
        /// Unique component instance identifier.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
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
