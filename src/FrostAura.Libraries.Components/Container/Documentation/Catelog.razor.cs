using FrostAura.Libraries.Components.Abstractions;
using FrostAura.Libraries.Components.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace FrostAura.Libraries.Components.Container.Documentation
{
    /// <summary>
    /// Toggle between two templates as async work gets done.
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
        /// Supported component types.
        /// </summary>
        private List<IGrouping<string, Type>> _componentTypeGroups = new List<IGrouping<string, Type>>();

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            _componentTypeGroups = GetComponentTypes();
        }

        /// <summary>
        /// Get a collection of components inheriting from the BaseComponent type.
        /// </summary>
        /// <returns>A collection of components inheriting from the BaseComponent type.</returns>
        private List<IGrouping<string, Type>> GetComponentTypes()
        {
            var components = ComponentsAssembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.BaseType.IsGenericType)
                .Where(t => t.BaseType.GetGenericTypeDefinition() == typeof(BaseComponent<object>).GetGenericTypeDefinition())
                .OrderBy(t => t.Name)
                .GroupBy(t => t.GetCategoryCategory())
                .ToList();

            return components;
        }
    }
}
