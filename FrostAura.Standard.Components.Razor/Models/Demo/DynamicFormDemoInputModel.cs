using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrostAura.Standard.Components.Razor.Models.Demo
{
    /// <summary>
    /// Sample model for illustrating dynamic form generation and validation.
    /// </summary>
    public class DynamicFormDemoInputModel
    {
        [Description("Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid name is required.")]
        public string Name { get; set; } = "Deano";

        [Range(18, int.MaxValue, ErrorMessage = "A valid age from 18+ is required")]
        public int Age { get; set; } = 20;

        public DateTime Birthday { get; set; } = DateTime.Now;

        public bool LovesAnimals { get; set; }
    }
}
