using FrostAura.Standard.Components.Razor.Abstractions;
using FrostAura.Standard.Components.Razor.Enums.DynamicForm;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrostAura.Standard.Components.Razor.Input
{
    /// <summary>
    /// Component for generating an input form automatically based on a given type and instance of the type.
    /// </summary>
    /// <typeparam name="TDataContextType">Data context model type.</typeparam>
    public partial class DynamicForm<TDataContextType> : BaseComponent<DynamicForm<TDataContextType>> where TDataContextType : new()
    {
        /// <summary>
        /// Form element reference.
        /// </summary>
        [Parameter]
        public EditForm FormElement { get; set; }
        /// <summary>
        /// Preview for the data context.
        /// </summary>
        private string _dataContextPreview => JsonConvert.SerializeObject(FormElement?.Model, Formatting.Indented);

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (!EnableDemoMode) return;

            ValidationSummaryPosition = ValidationSummaryPosition.FormBottom;
            DataContext = (TDataContextType)(object)new object(); // TODO: Figure out what has to go here. I think its the model we want to render for.
            //DataContext = (TDataContextType)(object)new DynamicFormDemoInputModel();
            SubmitButtonText = "Show Payload";
        }
    }
}
