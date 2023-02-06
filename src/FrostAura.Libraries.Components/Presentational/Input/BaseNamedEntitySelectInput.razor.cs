using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using FrostAura.Libraries.Data.Models.EntityFramework;

namespace FrostAura.Libraries.Components.Presentational.Input
{
    /// <summary>
    /// A custom select component that allows for a datasource of base named entities to render.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to capture. Typically int (the ID field type for base named entities).</typeparam>
    public class BaseNamedEntitySelectInput<TValue> : InputSelect<TValue>
    {
        /// <summary>
        /// A collection of items to be made available for selection.
        /// </summary>
        [Parameter]
        public List<BaseNamedEntity> DataSource { get; set; }

        /// <summary>
        /// Initialize the component and create the render fragment for the clid content based on all the items in the datasource.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            ChildContent = (builder) =>
            {
                for (var i = 0; i < DataSource.Count; i++)
                {
                    var dataRow = DataSource[i];
                    builder.AddMarkupContent(i + 1, $"<option value=\"{dataRow.Id}\">{dataRow.Name}</option>");
                }
            };
        }
    }
}
