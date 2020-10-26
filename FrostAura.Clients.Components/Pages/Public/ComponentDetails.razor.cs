using FrostAura.Standard.Components.Razor;
using FrostAura.Standard.Components.Razor.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Reflection;

namespace FrostAura.Clients.Components.Pages.Public
{
    /// <summary>
    /// Component to render a component's details.
    /// </summary>
    public partial class ComponentDetails : BaseComponent<ComponentDetails>
    {
        /// <summary>
        /// Component's full name.
        /// </summary>
        [Parameter]
        public string FullName { get; set; }
        /// <summary>
        /// Component fragment to render.
        /// </summary>
        private RenderFragment ComponentFragment { get; set; }

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            var assembly = Assembly.GetAssembly(typeof(FrostAuraApplication<object>));
            var componentType = assembly
                .GetTypes()
                .SingleOrDefault(t => t.FullName == FullName);

            if (componentType == default) return;

            ComponentFragment = builder =>
            {
                builder.OpenComponent(0, componentType);
                builder.CloseComponent();
            };
        }
    }
}
