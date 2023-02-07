using FrostAura.Libraries.Components.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;
using FrostAura.Libraries.Components.Data.Interfaces;
using FrostAura.Libraries.Components.Shared.Attributes;

namespace FrostAura.Libraries.Components.Presentational.Content
{
    /// <summary>
    /// Component to inject content from an embedded file, into a Blazore component as HTML.
    /// </summary>
    [NoDemo]
    public partial class InjectEmbeddedContent : BaseComponent<InjectEmbeddedContent>
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(1, 0, 0);
        /// <summary>
        /// Application content service.
        /// </summary>
        [Inject]
        public IContentDataAccess ContentService { get; set; }
        /// <summary>
        /// JavaScript runtime engine.
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        /// <summary>
        /// Markup content.
        /// </summary>
        public MarkupString Markup { get; set; }
        /// <summary>
        /// Versioning to use in the fetching of the asset URL in order to allow for cache control.
        /// </summary>
        [Parameter]
        public Version AssetVersion { get; set; }
        /// <summary>
        /// Embedded content file name.
        /// </summary>
        [Parameter]
        public string ContentName { get; set; }
        /// <summary>
        /// What HTML to wrap the content with, if any.
        /// </summary>
        [Parameter]
        public string HtmlWrapper { get; set; }
        /// <summary>
        /// Assembly to load content from.
        /// </summary>
        [Parameter]
        public Assembly ContentAssembly { get; set; }
        /// <summary>
        /// Whether to invoke script content immidiately upon encounter.
        /// </summary>
        [Parameter]
        public bool RequestImmidiateInvocation { get; set; }
        /// <summary>
        /// Event to fire when the immidiate invocation completed, if set.
        /// </summary>
        [Parameter]
        public EventCallback<InjectEmbeddedContent> OnImmidiateInvocation { get; set; }

        /// <summary>
        /// Soon as all parameters are set, bootstrap the component.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (RequestImmidiateInvocation) return;

            var contentString = await ContentService.GetContentByKeyAsync<string>(ContentName, ContentAssembly ?? GetType().Assembly, CancellationToken.None);

            if (string.IsNullOrWhiteSpace(HtmlWrapper)) Markup = (MarkupString)$"{contentString}";
            else Markup = (MarkupString)$"<{HtmlWrapper}>{contentString}</{HtmlWrapper}>";

            StateHasChanged();
        }

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        /// <param name="firstRender">Whether this was the component's first render.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender) return;
            if (!RequestImmidiateInvocation) return;

            var contentString = await ContentService.GetContentByKeyAsync<string>(ContentName, ContentAssembly ?? GetType().Assembly, CancellationToken.None);

            await JsRuntime.InvokeVoidAsync("eval", contentString);
            await OnImmidiateInvocation.InvokeAsync(this);
        }
    }
}
