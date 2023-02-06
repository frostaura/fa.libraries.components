using FrostAura.Libraries.Data.Models.EntityFramework;
using Microsoft.AspNetCore.Components.Forms;

namespace FrostAura.Libraries.Components.Shared.Models.Input
{
    /// <summary>
    /// A form property effect which allows the rendering of a custom select dropdown UI component for a property of type int (which implies the entity's unique id).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being captured.</typeparam>
    /// <typeparam name="TControlToRenderType">The type of the UI component to render when encountering the respective property.</typeparam>
    public class EntitySelectFormPropertyEffect<TValue, TControlToRenderType> : FormPropertyEffect
        where TControlToRenderType : InputBase<TValue>
    {
        /// <summary>
        /// A collection of items to be made available for selection.
        /// </summary>
        public List<BaseNamedEntity> DataSource { get; private set; }
        /// <summary>
        /// The type of the UI component to render when encountering the respective property.
        /// </summary>
        public Type ControlToRenderType { get; private set; }

        /// <summary>
        /// Overloaded constructr to allow for dependency injection.
        /// </summary>
        /// <param name="propertyName">The property name to apply an effect to given a parent data transfer object.</param>
        /// <param name="dataSource">A collection of items to be made available for selection.</param>
        public EntitySelectFormPropertyEffect(string propertyName, List<BaseNamedEntity> dataSource) :
            base(propertyName)
        {
            DataSource = dataSource;
            ControlToRenderType = typeof(TControlToRenderType);
        }
    }
}
