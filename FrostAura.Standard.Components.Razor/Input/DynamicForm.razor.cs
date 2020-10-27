using FrostAura.Standard.Components.Razor.Abstractions;
using Microsoft.AspNetCore.Components;

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
        /// Whether to show a full validation summary,
        /// </summary>
        [Parameter]
        public bool ShowValidationSummary { get; set; }
    }
}