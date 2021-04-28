using FrostAura.Standard.Components.Razor.Abstractions;
using FrostAura.Standard.Components.Razor.Attributes.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public EditContext CascadedEditContext { get; set; }
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
        /// Collection of types that support being rendered by the dynamic field system together with which component to render for the type.
        /// </summary>
        private static readonly Dictionary<Type, Type> _typeToControlRendererMappings = new Dictionary<Type, Type>();

        /// <summary>
        /// Register a new control to be rendered when a given field type is encountered. The control type has to be derived from InputBase<typeparamref name="TFieldType"/>.
        /// </summary>
        /// <typeparam name="TFieldType">Fielt type to map the control for.</typeparam>
        /// <typeparam name="TControlToRenderType">Control type to render for the field.</typeparam>
        public static void RegisterRendererTypeControl<TFieldType, TControlToRenderType>() where TControlToRenderType : InputBase<TFieldType>
        {
            _typeToControlRendererMappings[typeof(TFieldType)] = typeof(TControlToRenderType);
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
            var componentType = GetComponentTypeToRenderForValueType<TValue>();

            if (componentType == default)
            {
                GenerateRenderTreeForChildInputFields<TValue>(builder);

                return;
            }

            var constant = Expression.Constant(CascadedEditContext.Model, CascadedEditContext.Model.GetType());
            var exp = Expression.Property(constant, PropertyInformation.Name);
            var lambda = Expression.Lambda(exp);
            var castedLambda = (Expression<Func<TValue>>)lambda;
            var currentValue = (TValue)PropertyInformation.GetValue(CascadedEditContext.Model);

            builder.OpenComponent(0, componentType);
            builder.AddAttribute(1, "id", PropertyInformation.Name);
            builder.AddAttribute(2, nameof(InputBase<TValue>.Value), currentValue);
            builder.AddAttribute(3, nameof(InputBase<TValue>.ValueExpression), castedLambda);
            builder.AddAttribute(4, nameof(InputBase<TValue>.ValueChanged), RuntimeHelpers.TypeCheck(
                EventCallback.Factory.Create(
                    this, 
                    EventCallback.Factory.CreateInferred(this, val => PropertyInformation.SetValue(CascadedEditContext.Model, val), 
                    (TValue)PropertyInformation.GetValue(CascadedEditContext.Model)))));
            builder.CloseComponent();
        }

        /// <summary>
        /// Generate a randerable section for the given property information's children properties.
        /// </summary>
        /// <typeparam name="TValue">Property value type.</typeparam>
        /// <param name="builder">Render tree.</param>
        private void GenerateRenderTreeForChildInputFields<TValue>(RenderTreeBuilder builder)
        {
            var type = typeof(TValue);
            var properties = type
                .GetProperties()
                .Where(p => p.GetCustomAttribute<FieldIgnoreAttribute>() == default)
                .ToArray();

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                builder.OpenComponent(0, typeof(DynamicField));
                builder.AddAttribute(1, nameof(EnableDemoMode), EnableDemoMode);
                builder.AddAttribute(2, nameof(PropertyInformation), property);
                builder.CloseComponent();
            }
        }

        /// <summary>
        /// Determine which object type to render for a given data type.
        /// </summary>
        /// <typeparam name="TValue">Data type.</typeparam>
        /// <returns>Object type to render.</returns>
        private Type GetComponentTypeToRenderForValueType<TValue>()
        {
            if (!_typeToControlRendererMappings.ContainsKey(typeof(TValue)))
            {
                Logger.LogWarning($"The type '{typeof(TValue).FullName}' is not supported by DynamicField. Call FrostAura.Standard.Components.Razor.Input.DynamicField.RegisterRendererTypeControl in order to map a renderer.");

                return default;
            }

            return _typeToControlRendererMappings[typeof(TValue)];
        }
    }
}
