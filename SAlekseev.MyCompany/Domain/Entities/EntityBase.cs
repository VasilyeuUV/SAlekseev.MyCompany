using System.ComponentModel.DataAnnotations;

namespace SAlekseev.MyCompany.Domain.Entities;

/// <summary>
/// Базовая сущность.
/// </summary>
public abstract class EntityBase
{
    public int Id{ get; set; }

    [Required(ErrorMessage = "Заполните название")]
    [Display(Name = "Название")]
    [MaxLength(200)]
    public string? Title { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
