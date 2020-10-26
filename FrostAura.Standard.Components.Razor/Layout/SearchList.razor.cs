using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using FrostAura.Standard.Components.Razor.Abstractions;

namespace FrostAura.Standard.Components.Razor.Layout
{
    /// <summary>
    /// List component to allow for searching child content and filtering it out.
    /// </summary>
    /// <typeparam name="TItemType">Item object type.</typeparam>
    public partial class SearchList<TItemType> : BaseComponent<SearchList<TItemType>>
    {
        /// <summary>
        /// Search box placeholder.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; } = string.Empty;
        /// <summary>
        /// Template to use in search list.
        /// </summary>
        [Parameter]
        public RenderFragment<TItemType> ItemTemplate { get; set; }
        /// <summary>
        /// Template to use for the footer of the search list.
        /// </summary>
        [Parameter]
        public RenderFragment SearchFooterTemplate { get; set; }
        /// <summary>
        /// Collection of items and their identifiers as the key. Identifiers get used in the search matching.
        /// </summary>
        [Parameter]
        public Dictionary<string, TItemType> Items { get; set; } = new Dictionary<string, TItemType>();
        /// <summary>
        /// Currently active search phrase.
        /// </summary>
        private string _searchPhrase { get; set; } = string.Empty;
        /// <summary>
        /// Internal filtered items collection to render.
        /// </summary>
        private List<TItemType> _items { get; set; } = new List<TItemType>();

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            PerformSearch();
        }

        /// <summary>
        /// Perform a search and render the filtered result.
        /// </summary>
        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(_searchPhrase)) _items = Items
                    .Select(i => i.Value)
                    .ToList();
            else _items = Items
                    .Where(i => i.Key.Contains(_searchPhrase, StringComparison.InvariantCultureIgnoreCase))
                    .Select(i => i.Value)
                    .ToList();

            StateHasChanged();
        }

        /// <summary>
        /// Event handler for when the input changes on the search phrase input.
        /// </summary>
        /// <param name="e">Event args.</param>
        private void OnSearchPhraseChanged(ChangeEventArgs e)
        {
            _searchPhrase = e.Value.ToString();
            PerformSearch();
        }

        /// <summary>
        /// Event handler for when the search phrase clear button has been tapped.
        /// </summary>
        /// <param name="e">Event args.</param>
        private void OnSearchPhraseClearTapped(MouseEventArgs e)
        {
            _searchPhrase = string.Empty;
            PerformSearch();
        }
    }
}
