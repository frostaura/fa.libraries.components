using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FrostAura.Standard.Components.Razor.Input
{
    /// <summary>
    /// Wrapped validation message to allow for manual boostrapping.
    /// </summary>
    /// <typeparam name="TValue">Type of the value to validate.</typeparam>
    public class DynamicValidationMessage<TValue> : ValidationMessage<TValue>
    {
        /// <summary>
        /// Get the instance. This instance will be used to fill out the values inputted by the user.
        /// </summary>
        [CascadingParameter] 
        EditContext CascadedEditContext { get; set; }
        /// <summary>
        /// Property information to render an element for.
        /// </summary>
        [Parameter]
        public PropertyInfo PropertyInformation { get; set; }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (CascadedEditContext == null) return;

            var constant = Expression.Constant(CascadedEditContext.Model, CascadedEditContext.Model.GetType());
            var exp = Expression.Property(constant, PropertyInformation.Name);
            var lambda = Expression.Lambda(exp);

            For = (Expression<Func<TValue>>)lambda;

            base.OnParametersSet();
        }
    }
}
