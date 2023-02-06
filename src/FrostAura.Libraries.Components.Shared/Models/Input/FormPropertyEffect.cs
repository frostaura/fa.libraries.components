namespace FrostAura.Libraries.Components.Shared.Models.Input
{
    /// <summary>
    /// A base form property effect which can describe how to manipulate various property types inside of the DynamicForm component pipeline.
    /// </summary>
    public abstract class FormPropertyEffect
    {
        /// <summary>
        /// The property name to apply an effect to given a parent data transfer object.
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Overloaded constructr to allow for dependency injection.
        /// </summary>
        /// <param name="propertyName">The property name to apply an effect to given a parent data transfer object.</param>
        public FormPropertyEffect(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
