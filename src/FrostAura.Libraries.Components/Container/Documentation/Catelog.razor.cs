using FrostAura.Libraries.Components.Shared.Abstractions;
using FrostAura.Libraries.Components.Shared.Attributes;
using FrostAura.Libraries.Components.Shared.Extensions;
using FrostAura.Libraries.Data.Attributes;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace FrostAura.Libraries.Components.Container.Documentation
{
    /// <summary>
    /// A component to render automated documentation for an assembly of components inheriting from BaseComponent<T>.
    /// </summary>
    public partial class Catelog : BaseComponent<Catelog>
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
        /// The current component to render documentation for, if any.
        /// </summary>
        [Parameter]
        public string? FocusedComponentName { get; set; }
        /// <summary>
        /// A delegate to execute on the selection of a component.
        /// </summary>
        [Parameter]
        public Action<string>? OnComponentSelected { get; set; }
        /// <summary>
        /// Supported component types.
        /// </summary>
        private List<IGrouping<string, Type>> _componentTypeGroups = new List<IGrouping<string, Type>>();

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _componentTypeGroups = GetComponentTypes();
        }

        /// <summary>
        /// Get a collection of components inheriting from the BaseComponent type.
        /// </summary>
        /// <returns>A collection of components inheriting from the BaseComponent type.</returns>
        private List<IGrouping<string, Type>> GetComponentTypes()
        {
            if (ComponentsAssembly == default) return new List<IGrouping<string, Type>>();

            var components = ComponentsAssembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.BaseType.IsGenericType)
                .Where(t => t.BaseType.GetGenericTypeDefinition() == typeof(BaseComponent<object>).GetGenericTypeDefinition())
                .Where(p => p.GetCustomAttribute<NoDemoAttribute>() == default)
                .OrderBy(t => t.Name)
                .GroupBy(t => t.GetCategoryCategory())
                .OrderBy(g => g.Key)
                .ToList();

            return components;
        }

        /// <summary>
        /// Handle a selection of a component. If there is a callback registered, call it otherwise perform a nagvigation.
        /// </summary>
        /// <param name="componentName">Selected component name.</param>
        private void ComponentSelectedHandler(string componentName)
        {
            if (OnComponentSelected == default) SafelyNavigateTo(componentName);
            else OnComponentSelected.Invoke(componentName);
        }
    }
}
