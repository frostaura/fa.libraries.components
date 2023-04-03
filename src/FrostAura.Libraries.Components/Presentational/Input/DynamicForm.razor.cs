using FrostAura.Libraries.Components.Shared.Abstractions;
using FrostAura.Libraries.Components.Shared.Enums.Input;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using FrostAura.Libraries.Components.Shared.Models.Input;
using FrostAura.Libraries.Data.Attributes;

namespace FrostAura.Libraries.Components.Presentational.Input
{
    /// <summary>
    /// Component for generating an input form automatically based on a given type and instance of the type.
    /// </summary>
    /// <typeparam name="TDataContextType">Data context model type.</typeparam>
    public partial class DynamicForm<TDataContextType> : BaseComponent<DynamicForm<TDataContextType>> where TDataContextType : new()
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(0, 0, 1);
        /// <summary>
        /// Form data context to use when updating / editing.
        /// </summary>
        [Parameter]
        public TDataContextType DataContext { get; set; } = new TDataContextType();
        /// <summary>
        /// Callback for when the form is submitted with valid values.
        /// </summary>
        [Parameter]
        public EventCallback<TDataContextType> OnValidSubmit { get; set; }
        /// <summary>
        /// Submit button text to show.
        /// </summary>
        [Parameter]
        public string SubmitButtonText { get; set; } = "Submit";
        /// <summary>
        /// Where to show a full validation summary,
        /// </summary>
        [Parameter]
        public ValidationSummaryPosition ValidationSummaryPosition { get; set; }
        /// <summary>
        /// Collection of form property effects to apply.
        /// </summary>
        [Parameter]
        public List<FormPropertyEffect> PropertyEffects { get; set; } = new List<FormPropertyEffect>();
        /// <summary>
        /// Collection of types that support being rendered by the dynamic field system together with which component to render for the type.
        /// </summary>
        [Parameter]
        public Dictionary<Type, Type> TypeToControlRendererMappings { get; set; } = new Dictionary<Type, Type>
        {
            { typeof(string), typeof(InputText) },
            { typeof(int), typeof(InputNumber<int>) },
            { typeof(double), typeof(InputNumber<double>) },
            { typeof(DateTime), typeof(InputDate<DateTime>) },
            { typeof(bool), typeof(InputCheckbox) }
        };
        /// <summary>
        /// Get data context property information.
        /// </summary>
        private IEnumerable<PropertyInfo> _dataContextProperties => DataContext?
            .GetType()
            .GetProperties()
            .Where(p => p.GetCustomAttribute<FieldIgnoreAttribute>() == default)
            .Where(p => p.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() == default)
            .Where(p => p.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>() == default)
            .Where(p => p.GetCustomAttribute<DatabaseGeneratedAttribute>() == default)
            .Where(p => !p.GetAccessors().First().IsVirtual)
            .ToArray();

        /// <summary>
        /// Handler for when the form has been successfully submitted.
        /// </summary>
        /// <param name="context">Edit context.</param>
        public void HandleOnValidSubmit(EditContext context)
        {
            if (!OnValidSubmit.HasDelegate) return;

            OnValidSubmit.InvokeAsync((TDataContextType)context.Model);
            StateHasChanged();
        }
    }
}
