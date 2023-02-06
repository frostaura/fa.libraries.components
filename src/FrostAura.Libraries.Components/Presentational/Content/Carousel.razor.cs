using FrostAura.Libraries.Components.Abstractions;
using FrostAura.Libraries.Components.Shared.Models.Content;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FrostAura.Libraries.Components.Presentational.Content
{
    /// <summary>
    /// Component for generating an input form automatically based on a given type and instance of the type.
    /// </summary>
    public partial class Carousel : BaseComponent<Carousel>
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(1, 0, 0);
        /// <summary>
        /// The delay for animations between scenes.
        /// </summary>
        [Parameter]
        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(5);
        /// <summary>
        /// The scenes to animate through.
        /// </summary>
        [Parameter]
        public List<CarouselScene> Scenes { get; set; }

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        /// <param name="firstRender">Whether this was the component's first render.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("eval", GetJsBoostrappingCode());
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Get the JavaScript code that should be run to bootstrap the client aspects to this component.
        /// </summary>
        /// <returns>The JavaScript code that should be run to bootstrap the client aspects to this component.</returns>
        private string GetJsBoostrappingCode()
        {
            return @"
                (() => {
                    window.carouselsLoaded = window.carouselsLoaded || {};
                    const currentCarouselIsLoaded = window.carouselsLoaded['" + Id + @"'] || false;

                    if(currentCarouselIsLoaded) return;

                    setInterval(() => {
                        const track = document.getElementById('" + Id + @"').querySelector(':scope > .track');
                        const currentScene = track.querySelector('.active');
                        let nextScene = currentScene.nextElementSibling;

                        if(nextScene == null){
                            nextScene = currentScene.parentElement.querySelector('.scene')
                        }

                        const lengthToMove = nextScene.style.left;
                        const transform = 'translateX(-' + lengthToMove + ')';

                        track.style.transform = transform;

                        currentScene.classList.remove('active');
                        nextScene.classList.add('active');
                    }, " + Delay.TotalMilliseconds + @");

                    window.carouselsLoaded['" + Id + @"'] = true;
                })();
            ";
        }

        /// <summary>
        /// Determine whether a given scene is the currently focused one.
        /// </summary>
        /// <param name="scene">The scene to check.</param>
        /// <returns>Whether a given scene is the currently focused one</returns>
        private bool IsSceneActive(CarouselScene scene)
        {
            return Scenes.First().Id == scene.Id;
        }
    }
}
