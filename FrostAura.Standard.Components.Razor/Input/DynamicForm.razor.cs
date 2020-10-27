using FrostAura.Standard.Components.Razor.Abstractions;
using FrostAura.Standard.Components.Razor.Enums.DynamicForm;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Reflection;

namespace FrostAura.Standard.Components.Razor.Input
{
    /// <summary>
    /// Component for generating an input form automatically based on a given type and instance of the type.
    /// </summary>
    public partial class DynamicForm<TDataContextType> : BaseComponent<DynamicForm<TDataContextType>> where TDataContextType : new()
    {
        /// <summary>
        /// Form data context to use when updating / editing.
        /// </summary>
        [Parameter]
        public TDataContextType DataContext { get; set; } = new TDataContextType();
        /// <summary>
        /// Callback for when the form is submitted with valid values.
        /// </summary>
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }
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
        /// Get data context property information.
        /// </summary>
        private IEnumerable<PropertyInfo> _dataContextProperties => DataContext?
            .GetType()
            .GetProperties();

        /// <summary>
        /// Handler for when the form has been successfully submitted.
        /// </summary>
        /// <param name="context">Edit context.</param>
        public void HandleOnValidSubmit(EditContext context)
        {
            if (!OnValidSubmit.HasDelegate) return;

            OnValidSubmit.InvokeAsync(context);
        }
    }
}