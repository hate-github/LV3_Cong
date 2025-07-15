using System;
using System.ComponentModel.DataAnnotations;

namespace LV3_Cong.Models
{
    public class Birthday
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FIO { get; set; } = string.Empty;

        [Required]
        public DateOnly Date { get; set; }

        public string PhotoPath { get; set; } = string.Empty;
    }
}
