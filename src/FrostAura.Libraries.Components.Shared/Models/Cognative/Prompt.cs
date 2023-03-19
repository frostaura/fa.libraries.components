using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrostAura.Libraries.Components.Shared.Models.Cognative
{
    /// <summary>
    /// A model for encaptulating prompt information.
    /// </summary>
    public class PromptModel
    {
        /// <summary>
        /// The prompt text.
        /// </summary>
        [Description("Question:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid question is required.")]
        public string Prompt { get; set; }
    }
}

