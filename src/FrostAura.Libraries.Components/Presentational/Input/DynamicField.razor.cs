using FrostAura.Libraries.Components.Abstractions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using FrostAura.Libraries.Components.Shared.Models.Input;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.Extensions.Logging;
using FrostAura.Libraries.Data.Attributes;

namespace FrostAura.Libraries.Components.Presentational.Input
{
    /// <summary>
    /// Dynamic field to render a form element for given property infomation.
    /// </summary>
    public partial class DynamicField : BaseComponent<DynamicField>
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(0, 0, 1);
        /// <summary>
        /// Collection of form property effects to apply.
        /// </summary>
        [Parameter]
        public List<FormPropertyEffect> PropertyEffects { get; set; } = new List<FormPropertyEffect>();
        /// <summary>
        /// Get the instance. This instance will be used to fill out the values inputted by the user.
        /// </summary>
        [CascadingParameter]
        public EditContext CascadedEditContext { get; set; }
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
        /// <summary>
        /// Whether to render a per-element validator.
        /// </summary>
        [Parameter]
        public bool ShouldRenderValidator { get; set; }
        /// <summary>
        /// Getter for the field's description.
        /// </summary>
        private string _fieldLabel
        {
            get
            {
                var descriptionAttribute = PropertyInformation
                    .GetCustomAttribute<DescriptionAttribute>();
                var label = descriptionAttribute?.Description ?? PropertyInformation.Name;

                if (label.EndsWith("Id")) label = label.Replace("Id", "");

                return label;
            }
        }
        /// <summary>
        /// A unique field id for this particular fully qualified property name.
        /// </summary>
        private string _fieldId => $"{Model.GetType()}-{PropertyInformation.Name}";
        /// <summary>
        /// Collection of types that support being rendered by the dynamic field system together with which component to render for the type.
        /// </summary>
        private static readonly Dictionary<Type, Type> _typeToControlRendererMappings = new Dictionary<Type, Type>();

        /// <summary>
        /// Register default control type renderers for property types encountered.
        /// </summary>
        protected override void OnInitialized()
        {
            Model = Model ?? CascadedEditContext.Model;

            // Register default form control mappings.
            RegisterRendererTypeControl<string, InputText>();
            RegisterRendererTypeControl<int, InputNumber<int>>();
            RegisterRendererTypeControl<double, InputNumber<double>>();
            RegisterRendererTypeControl<DateTime, InputDate<DateTime>>();
            RegisterRendererTypeControl<bool, InputCheckbox>();

            base.OnInitialized();
        }

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

            if (!ShouldRenderValidator) return builder => { };

            return builder =>
            {
                var sequence = 0;

                builder.OpenComponent(sequence++, dynamicValidationMessageType);
                builder.AddAttribute(sequence++, nameof(DynamicValidationMessage<object>.PropertyInformation), PropertyInformation);
                builder.AddAttribute(sequence++, nameof(DynamicValidationMessage<object>.Model), Model);
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

            // Process effects.
            var effectsToProcess = PropertyEffects
              .Where(pe => pe.PropertyName == PropertyInformation?.Name);

            foreach (var effect in effectsToProcess)
            {
                TryProcessEntitySelectFormPropertyEffect<TValue>(effect, builder);
                TryProcessImagePickerFormPropertyEffect<TValue>(effect, builder);

                return;
            }

            var constant = Expression.Constant(Model, Model.GetType());
            var exp = Expression.Property(constant, PropertyInformation.Name);
            var lambda = Expression.Lambda(exp);
            var castedLambda = (Expression<Func<TValue>>)lambda;
            var currentValue = (TValue)PropertyInformation.GetValue(Model);
            var sequence = 0;

            builder.OpenComponent(sequence++, componentType);
            builder.AddAttribute(sequence++, "id", _fieldId);
            builder.AddAttribute(sequence++, "class", componentType == typeof(InputCheckbox) ? "form-check" : "form-control");
            // The following is a replacement for the bind-value property.
            builder.AddAttribute(sequence++, nameof(InputBase<TValue>.Value), currentValue);
            builder.AddAttribute(sequence++, nameof(InputBase<TValue>.ValueExpression), castedLambda);
            builder.AddAttribute(sequence++, nameof(InputBase<TValue>.ValueChanged), RuntimeHelpers.TypeCheck(
                EventCallback.Factory.Create(
                    this,
                    EventCallback.Factory.CreateInferred(this, val => PropertyInformation.SetValue(Model, val),
                    (TValue)PropertyInformation.GetValue(Model)))));
            builder.CloseComponent();
        }

        /// <summary>
        /// Process properties with the image picker effect on them.
        /// </summary>
        /// <typeparam name="TValue">The type of the property to work with.</typeparam>
        /// <param name="effect">The effect context.</param>
        /// <param name="builder">The render fragment builder.</param>
        private void TryProcessImagePickerFormPropertyEffect<TValue>(FormPropertyEffect effect, RenderTreeBuilder builder)
        {
            if (!(effect is ImagePickerFormPropertyEffect)) return;

            var parsedEffect = ((ImagePickerFormPropertyEffect)effect);
            var componentType = parsedEffect.ControlToRenderType;
            var constants = Expression.Constant(Model, Model.GetType());
            var exps = Expression.Property(constants, PropertyInformation.Name);
            var lambdas = Expression.Lambda(exps);
            var currentValue = (TValue)PropertyInformation.GetValue(Model);
            var sequence = 0;

            // The img element to display the content.
            builder.OpenElement(sequence++, "img");
            builder.AddAttribute(sequence++, "class", "image-picker-image");
            builder.AddAttribute(sequence++, "id", $"{_fieldId}_img");
            builder.AddAttribute(sequence++, "src", string.IsNullOrWhiteSpace(currentValue?.ToString()) ? parsedEffect.DefaultUrl : currentValue?.ToString());
            builder.AddAttribute(sequence++, "onload", EventCallback.Factory.Create(
                this,
                EventCallback.Factory.CreateInferred<ProgressEventArgs>(
                    this,
                    args => HandleImageLoadedAsync(args),
                    default)
            ));
            builder.CloseElement();

            // The file input component itself.
            builder.OpenComponent(sequence++, componentType);
            builder.AddAttribute(sequence++, "id", _fieldId);
            // The following is a replacement for the bind-value property.
            builder.AddAttribute(sequence++, nameof(InputFile.OnChange), EventCallback.Factory.Create(
                    this,
                    EventCallback.Factory.CreateInferred<InputFileChangeEventArgs>(
                        this,
                        args => HandleSetImageValue(args),
                        default)
                ));
            builder.CloseComponent();
        }

        /// <summary>
        /// Handler for when an image has been loaded.
        /// 
        /// We bootstrap the onclick handler for the image to trigger the InputFile component.
        /// </summary>
        /// <param name="args">Supplied information about a progress event being raised.</param>
        /// <returns>Void</returns>
        private async Task HandleImageLoadedAsync(ProgressEventArgs args)
        {
            var imageElementId = $"{_fieldId}_img";
            var command = $"document.getElementById('{imageElementId}').onclick = () => document.getElementById('{_fieldId}').click()";

            await JsRuntime.InvokeVoidAsync("eval", command);
        }

        /// <summary>
        /// Handler for when a new image has been picked on the InputFile component.
        /// </summary>
        /// <param name="args">Supplied information about a progress event being raised.</param>
        /// <returns>Void</returns>
        private async Task HandleSetImageValue(InputFileChangeEventArgs args)
        {
            var configuration = await ClientDataAccess.GetClientConfigurationAsync(CancellationToken.None);
            var imagesDirectoryName = "images";
            var baseUrl = Path.Combine(configuration.AppBaseUrl, imagesDirectoryName);
            var directory = Path.Combine(Directory.GetCurrentDirectory(), imagesDirectoryName);
            var id = Guid.NewGuid().ToString();
            var filename = $"{directory}/{id}.png";

            await using FileStream fs = new(filename, FileMode.Create);
            await args.File.OpenReadStream(maxAllowedSize: int.MaxValue).CopyToAsync(fs);

            var url = $"{baseUrl}/{id}.png";
            PropertyInformation.SetValue(Model, url);
        }

        /// <summary>
        /// Process properties with the entity select form effect on them.
        /// </summary>
        /// <typeparam name="TValue">The type of the property to work with.</typeparam>
        /// <param name="effect">The effect context.</param>
        /// <param name="builder">The render fragment builder.</param>
        private void TryProcessEntitySelectFormPropertyEffect<TValue>(FormPropertyEffect effect, RenderTreeBuilder builder)
        {
            if (!(effect is EntitySelectFormPropertyEffect<int, BaseNamedEntitySelectInput<int>>)) return;

            var parsedEffect = ((EntitySelectFormPropertyEffect<int, BaseNamedEntitySelectInput<int>>)effect);
            var componentType = parsedEffect.ControlToRenderType;
            componentType = typeof(BaseNamedEntitySelectInput<int>);
            var constants = Expression.Constant(Model, Model.GetType());
            var exps = Expression.Property(constants, PropertyInformation.Name);
            var lambdas = Expression.Lambda(exps);
            var castedLambdas = (Expression<Func<TValue>>)lambdas;
            var currentValue = (TValue)PropertyInformation.GetValue(Model);
            var sequence = 0;

            builder.OpenComponent(sequence++, componentType);
            builder.AddAttribute(sequence++, "id", _fieldId);
            builder.AddAttribute(sequence++, "class", "form-control");
            // The following is a replacement for the bind-value property.
            builder.AddAttribute(sequence++, nameof(InputBase<TValue>.Value), currentValue);
            builder.AddAttribute(sequence++, nameof(InputBase<TValue>.ValueExpression), castedLambdas);
            builder.AddAttribute(sequence++, nameof(InputBase<TValue>.ValueChanged), RuntimeHelpers.TypeCheck(
                EventCallback.Factory.Create(
                    this,
                    EventCallback.Factory.CreateInferred(this, val => PropertyInformation.SetValue(Model, val),
                    (TValue)PropertyInformation.GetValue(Model)))));
            builder.AddAttribute(sequence++, "DataSource", parsedEffect.DataSource);
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
                .Where(p => p.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() == default)
                .Where(p => p.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>() == default)
                .Where(p => p.GetCustomAttribute<DatabaseGeneratedAttribute>() == default)
                .Where(p => !p.GetAccessors().First().IsVirtual)
                .ToArray();
            var nestedModel = Model
              .GetType()
              .GetProperties()
              .First(p => p.PropertyType == type)
              .GetValue(Model);

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var sequence = 0;

                builder.OpenComponent(sequence++, typeof(DynamicField));
                builder.AddAttribute(sequence++, nameof(EnableDemoMode), EnableDemoMode);
                builder.AddAttribute(sequence++, nameof(PropertyInformation), property);
                builder.AddAttribute(sequence++, nameof(Model), nestedModel);
                builder.AddAttribute(sequence++, nameof(PropertyEffects), PropertyEffects);
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
