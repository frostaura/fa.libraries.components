using System;
using System.ComponentModel;
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

        /// <summary>
        /// Get the component description from the DescriptionAttribute upon the component class.
        /// </summary>
        /// <returns>The component description from the DescriptionAttribute upon the component class.</returns>
        private string GetDescription()
        {
            var componentType = ComponentsAssembly
                .GetTypes()
                .SingleOrDefault(t => t.FullName == ComponentName);
            var descriptionAttr = componentType
                .GetCustomAttribute<DescriptionAttribute>();

            return descriptionAttr?.Description ?? "No description. Add a DescriptionAttribute to your component class to set one.";
        }

        /// <summary>
        /// Get all properties on the componenty type that have description attributes.
        /// </summary>
        /// <returns></returns>
        private List<PropertyInfo> GetProperties()
        {
            var componentType = ComponentsAssembly
                .GetTypes()
                .SingleOrDefault(t => t.FullName == ComponentName);
            var propertiesWithDescriptions = componentType
                .GetProperties()
                .Where(t => t.GetCustomAttribute<DescriptionAttribute>() != default)
                .OrderBy(p => p.Name)
                .ToList();

            return propertiesWithDescriptions;
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
