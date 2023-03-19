using FrostAura.Libraries.Components.Shared.Abstractions;
using Markdig;
using Microsoft.AspNetCore.Components;
using OpenAI_API;
using OpenAI_API.Chat;
using FrostAura.Libraries.Components.Shared.Models.Cognative;
using FrostAura.Libraries.Components.Data.Interfaces;

namespace FrostAura.Libraries.Components.Presentational.Congative
{
    /// <summary>
    /// A component for providing a conversation window with ChatGPT.
    /// </summary>
	public partial class ChatGPT : BaseComponent<ChatGPT>
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(1, 0, 0);
        /// <summary>
        /// The API key to use on the OpenAI API.
        /// </summary>
        [Parameter]
        public string ApiKey { get; set; }
        /// <summary>
        /// The system prompt passed to the model which typically instructs how the model will behave in future responses.
        /// </summary>
        [Parameter]
        public string SystemPrompt { get; set; } = "";
        /// <summary>
        /// Service to manipulate and fetch content from the the client-space. Including fetching configuration.
        /// </summary>
        [Inject]
        protected IClientDataAccess ClientDataAccess { get; set; }
        /// <summary>
        /// A collection of all messages in the conversation.
        /// </summary>
        private List<KeyValuePair<string, RenderFragment>> _messages = new List<KeyValuePair<string, RenderFragment>>();
        /// <summary>
        /// The OpenAI client used for comms with the model.
        /// </summary>
        private OpenAIAPI _openAiApi;
        /// <summary>
        /// The OpenAI conversation context.
        /// </summary>
        private Conversation _conversation;
        /// <summary>
        /// Whether a request to the assitant AI model is currently pending.
        /// </summary>
        private bool _assistantIsTyping = false;
        /// <summary>
        /// A model to contain the prompt info.
        /// </summary>
        private PromptModel _prompt = new PromptModel();

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        /// <param name="firstRender">Whether this was the component's first render.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender) await ResetAsync();
        }

        /// <summary>
        /// Reset the conversation.
        /// </summary>
        private async Task ResetAsync()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                var config = await ClientDataAccess.GetClientConfigurationAsync(CancellationToken.None);

                ApiKey = config.OpenAiApiKey;
            }

            _messages.Clear();
            _openAiApi = new OpenAIAPI(ApiKey);
            _conversation = _openAiApi.Chat.CreateConversation();

            await AskAsync(SystemPrompt, true);
        }

        /// <summary>
        /// Build the HTML for a given message. This allows for special formatting like markdown to html.
        /// </summary>
        /// <param name="role">The entity who sent the message.</param>
        /// <param name="message">The message.</param>
        /// <returns>The segment to render.</returns>
        private RenderFragment GetMessageRenderFragment(string role, string message)
        {
            return (builder) =>
            {
                var html = Markdown.ToHtml(message);
                builder.AddMarkupContent(0, html);
            };
        }

        /// <summary>
        /// Ask the AI model a qurstion.
        /// </summary>
        /// <param name="question">The question to ask.</param>
        /// <param name="asSystem">Whether this question is a system message or a user question.</param>
        /// <returns>The assistant's response.</returns>
        private async Task<string> AskAsync(string question, bool asSystem = false)
        {
            if(asSystem)
            {
                _conversation.AppendSystemMessage(question);
                _messages.Add(new KeyValuePair<string, RenderFragment>("System", GetMessageRenderFragment("System", question)));
            }
            else
            {
                _conversation.AppendUserInput(question);
                _messages.Add(new KeyValuePair<string, RenderFragment>("User", GetMessageRenderFragment("User", question)));
            }

            _assistantIsTyping = true;

            StateHasChanged();

            var assistantResponse = await _conversation.GetResponseFromChatbot();
            _assistantIsTyping = false;

            _messages.Add(new KeyValuePair<string, RenderFragment>("Assistant", GetMessageRenderFragment("Assistant", assistantResponse)));

            StateHasChanged();
            return assistantResponse;
        }

        /// <summary>
        /// A handler for the form submission for asking a question.
        /// </summary>
        /// <param name="prompt">The prompt / question.</param>
        /// <returns>Void</returns>
        private async Task OnAskAsync(PromptModel prompt)
        {
            await AskAsync(prompt.Prompt);
            _prompt = new PromptModel();
            StateHasChanged();
        }
    }
}

