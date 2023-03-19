using FrostAura.Libraries.Components.Shared.Abstractions;

namespace FrostAura.Libraries.Components.Presentational.Congative
{
    /// <summary>
    /// A component for providing a conversation window with ChatGPT.
    /// </summary>
    public partial class ChatGPT : BaseComponent<ChatGPT>
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!EnableDemoMode) return;

            SystemPrompt = "You are a budget generating chatbot. You are the financial expert that will guide me and ask me any questions you think I should know and then generate me a budget. Also, avoid any disclaimers. I am fully aware of the disclaimers already. Format your responses in the Markdown language and make it look good.";
        }
    }
}

