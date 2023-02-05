using System;
using System.Reflection;

namespace FrostAura.Libraries.Components.Interfaces.Resources
{
    /// <summary>
    /// Service to manipulate and fetch content.
    /// </summary>
    public interface IContentService
    {
        /// <summary>
        /// Get content by key and parse it to a desired type.
        /// </summary>
        /// <typeparam name="TParsedContentResult">Type which to cast the content to if found.</typeparam>
        /// <param name="key">Key which to look up the content for.</param>
        /// <param name="assembly">Assembly to load the content from.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Parsed content or default.</returns>
        Task<TParsedContentResult> GetContentByKeyAsync<TParsedContentResult>(string key, Assembly assembly, CancellationToken token);
    }
}

