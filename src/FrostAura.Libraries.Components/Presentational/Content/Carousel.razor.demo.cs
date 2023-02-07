using System;
using FrostAura.Libraries.Components.Abstractions;
using FrostAura.Libraries.Components.Shared.Models.Content;

namespace FrostAura.Libraries.Components.Presentational.Content
{
    /// <summary>
    /// Component for generating an input form automatically based on a given type and instance of the type.
    /// </summary>
    public partial class Carousel : BaseComponent<Carousel>
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!EnableDemoMode) return;

            Scenes = new List<CarouselScene>
            {
                new CarouselScene
                {
                    Name = "Slide 1",
                    BackgroundUrl = "https://cdn.wallpapersafari.com/81/44/tDeGVk.jpg"
                },
                new CarouselScene
                {
                    Name = "Slide 2",
                    BackgroundUrl = "https://media.istockphoto.com/photos/neon-background-abstract-blue-and-pink-with-light-shapes-line-picture-id1191658515?b=1&k=20&m=1191658515&s=612x612&w=0&h=BtKT_wMgQzpsM_m_AkKciHxT7cY0kW7FijIzryc1cMk="
                },
                new CarouselScene
                {
                    Name = "Slide 3",
                    BackgroundUrl = "https://wallpaperaccess.com/full/508201.jpg"
                }
            };
        }
    }
}
