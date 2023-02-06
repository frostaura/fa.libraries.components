namespace FrostAura.Libraries.Components.Shared.Extensions
{
    /// <summary>
    /// Type extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Get the category of a component by it's assembly name.
        /// </summary>
        /// <param name="type">The type to determine the category for.</param>
        /// <returns>The category of a component by it's assembly name.</returns>
        public static string GetCategoryCategory(this Type type)
        {
            var typeNamespaceSegments = type
                .AssemblyQualifiedName
                .Split(',')
                .First()
                .Split('.');

            if (typeNamespaceSegments.Length == 1) return typeNamespaceSegments.First();

            var categorySegment = typeNamespaceSegments[typeNamespaceSegments.Length - 2];

            return categorySegment;
        }
    }
}
