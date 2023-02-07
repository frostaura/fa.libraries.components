using System;
using FrostAura.Libraries.Components.Shared.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FrostAura.Libraries.Components.Demo.Presentational.Demo
{
    /// <summary>
    /// A demo component that simply renders an echo of what's passed to it, to the UI.
    /// </summary>
    public partial class Echo : BaseComponent<Echo>
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(1, 3, 2);
        /// <summary>
        /// The text to echo onto the UI.
        /// </summary>
        [Parameter]
        public string Text { get; set; }
    }
}
