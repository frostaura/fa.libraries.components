namespace FrostAura.Libraries.Components.Shared.Interfaces.Versioning
{
	/// <summary>
	/// For types that requires a Version property on them.
	/// </summary>
	public interface IRequiresVersioning
	{
        /// <summary>
        /// The version.
        /// </summary>
        public Version Version { get; }
    }
}

