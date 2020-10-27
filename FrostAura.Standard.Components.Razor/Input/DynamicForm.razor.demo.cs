using FrostAura.Standard.Components.Razor.Abstractions;
using FrostAura.Standard.Components.Razor.Models.Demo;
using FrostAura.Standard.Components.Razor.Enums.DynamicForm;

namespace FrostAura.Standard.Components.Razor.Input
{
    public partial class DynamicForm<TDataContextType> : BaseComponent<DynamicForm<TDataContextType>> where TDataContextType : new()
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (!EnableDemoMode) return;

            ValidationSummaryPosition = ValidationSummaryPosition.FormBottom;
            DataContext = (TDataContextType)(object)new DynamicFormDemoInputModel();
        }
    }
}
