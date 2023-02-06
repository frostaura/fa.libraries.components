using FrostAura.Libraries.Components.Shared.Models.Configuration;

namespace FrostAura.Libraries.Components.Data.Interfaces
{
    /// <summary>
    /// Service to manipulate and fetch content from the the client-space.
    /// </summary>
    public interface IClientDataAccess
    {
        /// <summary>
        /// Get content by key and parse it to a desired type.
        /// </summary>
        /// <typeparam name="TParsedContentResult">Type which to cast the content to if found.</typeparam>
        /// <param name="key">Key which to look up the content for.</param>
        /// <param name="token">Cancellation token to cancel downstream operations if required.</param>
        /// <returns>Parsed content or default.</returns>
        Task<TParsedContentResult> GetContentByKeyAsync<TParsedContentResult>(string key, CancellationToken token);
        /// <summary>
        /// Set content by key.
        /// </summary>
        /// <typeparam name="TParsedContentResult">Type of the object which to persist.</typeparam>
        /// <param name="key">Key which to set the content for.</param>
        /// <param name="token">Cancellation token to cancel downstream operations if required.</param>
        /// <returns>Void</returns>
        Task SetContentByKeyAsync<TParsedContentResult>(string key, TParsedContentResult obj, CancellationToken token);
        /// <summary>
        /// Get the client configuration for the application.
        /// </summary>
        /// <param name="token">Cancellation token to cancel downstream operations if required.</param>
        /// <returns>The client configuration for the application.</returns>
        Task<FrostAuraApplicationConfiguration> GetClientConfigurationAsync(CancellationToken token);
    }
}

