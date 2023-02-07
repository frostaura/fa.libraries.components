using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using FrostAura.Libraries.Components.Abstractions;
using FrostAura.Libraries.Components.Presentational.Input;
using FrostAura.Libraries.Components.Shared.Attributes;
using FrostAura.Libraries.Components.Shared.Enums.Input;
using FrostAura.Libraries.Components.Shared.Models.Input;
using FrostAura.Libraries.Data.Models.EntityFramework;

namespace FrostAura.Libraries.Components.Presentational.Input
{
    /// <summary>
    /// Component for generating an input form automatically based on a given type and instance of the type.
    /// </summary>
    /// <typeparam name="TDataContextType">Data context model type.</typeparam>
    [DemoType(typeof(Player))]
    public partial class DynamicForm<TDataContextType> : BaseComponent<DynamicForm<TDataContextType>> where TDataContextType : new()
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!EnableDemoMode) return;

            var outOfRangeInteger = 0;
            var demoTeamDataSource = new List<BaseNamedEntity>
            {
                new BaseNamedEntity{ Name = "Select a Team", Id = outOfRangeInteger },
                new BaseNamedEntity{ Name = "The Red Team", Id = 1 },
                new BaseNamedEntity{ Name = "The Blue Team", Id = 2 }
            };

            SubmitButtonText = "Demo Submit Button";
            PropertyEffects = new List<FormPropertyEffect>
            {
                new ImagePickerFormPropertyEffect(nameof(Player.Logo)),
                new EntitySelectFormPropertyEffect<int, BaseNamedEntitySelectInput<int>>(nameof(Player.TeamId), demoTeamDataSource)
            };
            ValidationSummaryPosition = ValidationSummaryPosition.PerElement;
        }
    }

    /// <summary>
    /// Player demo DTO entity model.
    /// </summary>
    [Table("Players")]
    [DebuggerDisplay("Name: {Name}")]
    public class Player : BaseNamedEntity
    {
        /// <summary>
        /// Team's logo content.
        /// </summary>
        [Description("Player Logo")]
        public string Logo { get; set; } = "https://via.placeholder.com/256x256";
        /// <summary>
        /// The unique id of the team the respective player belongs to.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A valid team is required.")]
        public int TeamId { get; set; }
        /// <summary>
        /// The team context that the respective player belongs to.
        /// </summary>
        [ForeignKey(nameof(TeamId))]
        public virtual object Team { get; set; }
    }
}
