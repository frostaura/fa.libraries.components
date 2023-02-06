using FrostAura.Libraries.Components.Data.Interfaces;
using FrostAura.Libraries.Components.Shared.Models.Configuration;
using FrostAura.Libraries.Core.Extensions.Validation;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace FrostAura.Libraries.Components.Data
{
    /// <summary>
    /// Service to manipulate and fetch content living in the client-space using localstorage.
    /// </summary>
	public class BlazorDefaultClientDataStore : IClientDataAccess
    {
        /// <summary>
        /// JS runtime to allow client-side access.
        /// </summary>
        private readonly IJSRuntime _jsRuntime;
        /// <summary>
        /// Http client factory as provided by dotnet.
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// Cached configuration for the instance of this client.
        /// </summary>
        private FrostAuraApplicationConfiguration _cachedConfiguration;

        /// <summary>
        /// Overloaded constructr to allow for dependency injection.
        /// </summary>
        /// <param name="jsRuntime">JS runtime to allow client-side access.</param>
        /// <param name="httpClientFactory">Http client factory as provided by dotnet.</param>
        public BlazorDefaultClientDataStore(IJSRuntime jsRuntime, IHttpClientFactory httpClientFactory)
        {
            _jsRuntime = jsRuntime
                .ThrowIfNull(nameof(jsRuntime));
            _httpClientFactory = httpClientFactory
                .ThrowIfNull(nameof(httpClientFactory));
        }

        /// <summary>
        /// Get content by key and parse it to a desired type.
        /// </summary>
        /// <typeparam name="TParsedContentResult">Type which to cast the content to if found.</typeparam>
        /// <param name="key">Key which to look up the content for.</param>
        /// <param name="token">Cancellation token to cancel downstream operations if required.</param>
        /// <returns>Parsed content or default.</returns>
        public async Task<TParsedContentResult> GetContentByKeyAsync<TParsedContentResult>(string key, CancellationToken token)
        {
            var dataString = await _jsRuntime.InvokeAsync<string>("eval", $"localStorage.getItem('{key}');");
            var parsedData = JsonConvert.DeserializeObject<TParsedContentResult>(dataString);

            return parsedData;
        }

        /// <summary>
        /// Set content by key.
        /// </summary>
        /// <typeparam name="TParsedContentResult">Type of the object which to persist.</typeparam>
        /// <param name="key">Key which to set the content for.</param>
        /// <param name="token">Cancellation token to cancel downstream operations if required.</param>
        /// <returns>Void.</returns>
        public async Task SetContentByKeyAsync<TParsedContentResult>(string key, TParsedContentResult obj, CancellationToken token)
        {
            var stringifiedData = JsonConvert.SerializeObject(obj);

            await _jsRuntime.InvokeAsync<string>("eval", $"localStorage.setItem('{key}', '{stringifiedData}')");
        }

        /// <summary>
        /// Get the client configuration for the application.
        /// </summary>
        /// <param name="token">Cancellation token to cancel downstream operations if required.</param>
        /// <returns>The client configuration for the application.</returns>
        public async Task<FrostAuraApplicationConfiguration> GetClientConfigurationAsync(CancellationToken token)
        {
            if (_cachedConfiguration != default) return _cachedConfiguration;

            var httpClient = _httpClientFactory.CreateClient("default");
            var settingsString = await httpClient.GetStringAsync("settings.json");
            var settings = JsonConvert.DeserializeObject<FrostAuraApplicationConfiguration>(settingsString);

            _cachedConfiguration = settings;

            return _cachedConfiguration;
        }
    }
}
