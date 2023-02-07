namespace FrostAura.Libraries.Components.Shared.Attributes
{
	/// <summary>
	/// An attribute to indicate what type to create a component with when in demo.
	/// </summary>
	public class DemoTypeAttribute : Attribute
	{
		/// <summary>
		/// The type to use.
		/// </summary>
		public Type Type { get; private set; }

        /// <summary>
        /// Allow for injecting parameters.
        /// </summary>
        /// <param name="type">The type to use.</param>
        public DemoTypeAttribute(Type type)
		{
			Type = type;

        }
	}
}

