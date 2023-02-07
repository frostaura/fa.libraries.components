using System;
using System.Reflection;
using FrostAura.Libraries.Components.Shared.Abstractions;
using FrostAura.Libraries.Components.Shared.Attributes;
using FrostAura.Libraries.Components.Shared.Interfaces.Versioning;
using Microsoft.AspNetCore.Components;

namespace FrostAura.Libraries.Components.Container.Documentation
{
    /// <summary>
    /// A single document item for the catelog component.
    ///
    /// Ref: https://www.webcomponents.org/element/PolymerElements/paper-dropdown-menu/elements/paper-dropdown-menu
    /// </summary>
    [NoDemo]
    public partial class CatelogItem : BaseComponent<CatelogItem>
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(0, 0, 1);
        /// <summary>
        /// The assembly which to browse for components.
        /// </summary>
        [Parameter]
        public Assembly ComponentsAssembly { get; set; }
        /// <summary>
        /// The component to render documentation for.
        /// </summary>
        [Parameter]
        public string ComponentName { get; set; }
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

            if (string.IsNullOrWhiteSpace(ComponentName)) return;

            var componentType = ComponentsAssembly
                .GetTypes()
                .SingleOrDefault(t => t.FullName == ComponentName);

            if (componentType == default) return;
            if (componentType.IsGenericType)
            {
                // Check if the component type has an attribute DemoType and if so check the type instead of using object.
                var demoTypeAttr = componentType.GetCustomAttribute<DemoTypeAttribute>();

                if (demoTypeAttr == default) componentType = componentType.MakeGenericType(typeof(object));
                else componentType = componentType.MakeGenericType(demoTypeAttr.Type);
            }

            ComponentFragment = builder =>
            {
                var i = 0;

                builder.OpenComponent(i++, componentType);
                builder.AddAttribute(i++, nameof(BaseComponent<object>.EnableDemoMode), true);
                builder.CloseComponent();
            };
        }

        private string GetDescription()
        {
            return "Dummy Description";
        }

        /// <summary>
        /// Extract the version of the catelog item from it's type.
        /// </summary>
        /// <returns>The version of the catelog item.</returns>
        private Version GetComponentVersion()
        {
            var componentType = ComponentsAssembly
                .GetTypes()
                .SingleOrDefault(t => t.FullName == ComponentName);
            var componentInstance = Activator.CreateInstance(componentType);
            var castedInstance = (IRequiresVersioning)componentInstance;

            return castedInstance.Version;
        }
    }
}
