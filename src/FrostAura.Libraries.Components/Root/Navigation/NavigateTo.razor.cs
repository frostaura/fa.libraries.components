using System;
using FrostAura.Libraries.Components.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FrostAura.Libraries.Components.Root.Navigation
{
    /// <summary>
    /// Component to automatically redirect to a given route.
    /// </summary>
    public partial class NavigateTo : BaseComponent<NavigateTo>
    {
        /// <summary>
        /// Path to navigate to.
        /// </summary>
        [Parameter]
        public string Path { get; set; }
        /// <summary>
        /// Whether to forcefully reload upon navigating.
        /// </summary>
        [Parameter]
        public bool ReloadOnNavigate { get; set; }

        /// <summary>
        /// Upon component initialization.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (string.IsNullOrWhiteSpace(Path)) return;

            NavigationManager.NavigateTo(Path);
        }
    }
}

