using FrostAura.Standard.Components.Razor.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace FrostAura.Standard.Components.Razor.Input
{
    /// <summary>
    /// Dynamic field to render a form element for given property infomation.
    /// </summary>
    public partial class DynamicField : BaseComponent<DynamicField>
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
        /// <summary>
        /// Getter for the field's description.
        /// </summary>
        private string _fieldLabel
        {
            get
            {
                var descriptionAttribute = PropertyInformation
                    .GetCustomAttribute<DescriptionAttribute>();

                return descriptionAttribute?.Description ?? PropertyInformation.Name;
            }
        }

        /// <summary>
        /// Generate a renderable input component section by determining the property type and initiating the generic method to perform the field's generation.
        /// </summary>
        /// <returns>Renderable component.</returns>
        private RenderFragment GenerateInputComponent()
        {
            var method = typeof(DynamicField)
                .GetMethod(nameof(DynamicField.GenerateRenderTreeForInputField), BindingFlags.NonPublic | BindingFlags.Instance);
            var appendInputComponentToRenderer = method
                .MakeGenericMethod(PropertyInformation.PropertyType);

            return builder =>
            {
                appendInputComponentToRenderer.Invoke(this, new object[] { builder });
            };
        }

        /// <summary>
        /// Generate a validation component to back the field.
        /// </summary>
        /// <returns>Renderable component.</returns>
        private RenderFragment GenerateValidationComponent()
        {
            var dynamicValidationMessageType = typeof(DynamicValidationMessage<object>)
                .GetGenericTypeDefinition()
                .MakeGenericType(PropertyInformation?.PropertyType);

            return builder =>
            {
                builder.OpenComponent(0, dynamicValidationMessageType);
                builder.AddAttribute(1, nameof(DynamicValidationMessage<object>.PropertyInformation), PropertyInformation);
                builder.CloseComponent();
            };
        }

        /// <summary>
        /// Generate a randerable section for the given property information.
        /// </summary>
        /// <typeparam name="TValue">Property value type.</typeparam>
        /// <param name="builder">Render tree.</param>
        private void GenerateRenderTreeForInputField<TValue>(RenderTreeBuilder builder)
        {
            var constant = Expression.Constant(CascadedEditContext.Model, CascadedEditContext.Model.GetType());
            var exp = Expression.Property(constant, PropertyInformation.Name);
            var lambda = Expression.Lambda(exp);
            var castedLambda = (Expression<Func<TValue>>)lambda;
            var componentType = GetComponentTypeToRenderForValueType<TValue>();
            var currentValue = (TValue)PropertyInformation.GetValue(CascadedEditContext.Model);

            builder.OpenComponent(0, componentType);
            builder.AddAttribute(1, "id", PropertyInformation.Name);
            builder.AddAttribute(2, nameof(InputBase<TValue>.Value), currentValue);
            builder.AddAttribute(3, nameof(InputBase<TValue>.ValueExpression), castedLambda);
            builder.AddAttribute(4, nameof(InputText.ValueChanged), RuntimeHelpers.TypeCheck(
                EventCallback.Factory.Create(
                    this, 
                    EventCallback.Factory.CreateInferred(this, val => PropertyInformation.SetValue(CascadedEditContext.Model, val), 
                    (TValue)PropertyInformation.GetValue(CascadedEditContext.Model)))));
            builder.CloseComponent();
        }

        /// <summary>
        /// Determine which object type to render for a given data type.
        /// </summary>
        /// <typeparam name="TValue">Data type.</typeparam>
        /// <returns>Object type to render.</returns>
        private Type GetComponentTypeToRenderForValueType<TValue>()
        {
            if (typeof(TValue) == typeof(string)) return typeof(InputText);
            if (typeof(TValue) == typeof(int)) return typeof(InputNumber<TValue>);
            if (typeof(TValue) == typeof(DateTime)) return typeof(InputDate<TValue>);
            if (typeof(TValue) == typeof(bool)) return typeof(InputCheckbox);

            throw new NotImplementedException($"The type '{typeof(TValue).FullName}' is not supported by DynamicField.");
        }
    }
}
