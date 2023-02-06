using System.Diagnostics;

namespace FrostAura.Libraries.Components.Shared.Models.Content
{
    /// <summary>
	/// A scene which should be displayed inside the Carousel component.
	/// </summary>
    [DebuggerDisplay("Name: {Name} - {Id} ({BackgroundUrl})")]
	public class CarouselScene
    {
        /// <summary>
        /// A unique identifier for each scene.
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();
        /// <summary>
        /// The name of the scene.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The background image to set for the scene.
        /// </summary>
        public string BackgroundUrl { get; set; }
    }
}
