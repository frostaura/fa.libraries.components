using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;

namespace FrostAura.Libraries.Components.Presentational.Input
{
    /// <summary>
    /// Wrapped validation message to allow for manual boostrapping.
    /// </summary>
    /// <typeparam name="TValue">Type of the value to validate.</typeparam>
    public class DynamicValidationMessage<TValue> : ValidationMessage<TValue>
    {
        /// <summary>
        /// The context of the model that this dynamic field represents.
        /// When this dynamic field component is at the root of an object, this value should be the EditContext model otherwise the nested model at hand.
        /// </summary>
        [Parameter]
        public object Model { get; set; }
        /// <summary>
        /// Property information to render an element for.
        /// </summary>
        [Parameter]
        public PropertyInfo PropertyInformation { get; set; }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            var constant = Expression.Constant(Model, Model.GetType());
            var exp = Expression.Property(constant, PropertyInformation.Name);
            var lambda = Expression.Lambda(exp);

            For = (Expression<Func<TValue>>)lambda;

            base.OnParametersSet();
        }
    }
}
