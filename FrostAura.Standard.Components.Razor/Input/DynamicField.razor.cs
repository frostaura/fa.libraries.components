using FrostAura.Standard.Components.Razor.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace FrostAura.Standard.Components.Razor.Input
{
    /// <summary>
    /// Dynamic field to render a form element for given property infomation.
    /// </summary>
    public partial class DynamicField : BaseComponent<DynamicField>
    {
        /// <summary>
        /// Property information to render an element for.
        /// </summary>
        [Parameter]
        public PropertyInfo PopertyInformation { get; set; }
    }
}
