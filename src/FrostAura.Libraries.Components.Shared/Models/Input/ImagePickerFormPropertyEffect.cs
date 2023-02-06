using Microsoft.AspNetCore.Components.Forms;

namespace FrostAura.Libraries.Components.Shared.Models.Input
{
    /// <summary>
    /// A form property effect which allows the rendering of a custom wrapped InputFile component when string types with URLs are presented in a given a parent data transfer object.
    /// 
    /// This allows for:
    /// - Wrapping a file input while hiding its default styling.
    /// - Rendering an image with the URL of the default, if null or empty or the URL string provided in the string property.
    /// - Handling of events to allow for clicking on the image to trigger the upload on the InputFile.
    /// </summary>
    public class ImagePickerFormPropertyEffect : FormPropertyEffect
    {
        /// <summary>
        /// The control type to render when the respective property name has been encountered on a given parent data transfer object.
        /// </summary>
        public Type ControlToRenderType { get; private set; }
        /// <summary>
        /// The default image URL to render should the string property have no value.
        /// </summary>
        public string DefaultUrl { get; private set; }

        /// <summary>
        /// Overloaded constructr to allow for dependency injection.
        /// </summary>
        /// <param name="propertyName">The property name to apply an effect to given a parent data transfer object.</param>
        /// <param name="defaultUrl">The default image URL to render should the string property have no value.</param>
        public ImagePickerFormPropertyEffect(string propertyName, string defaultUrl = "https://via.placeholder.com/256x256") :
            base(propertyName)
        {
            ControlToRenderType = typeof(InputFile);
            DefaultUrl = defaultUrl;
        }
    }
}
