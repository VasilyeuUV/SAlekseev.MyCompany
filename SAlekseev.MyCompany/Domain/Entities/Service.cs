using System.ComponentModel.DataAnnotations;
using SAlekseev.MyCompany.Domain.Enums;

namespace SAlekseev.MyCompany.Domain.Entities;

/// <summary>
/// Сущность Услуги.
/// </summary>
public class Service : EntityBase
{
    /// <summary>
    /// Идентификатор Категории, к которой относится Услуга.
    /// </summary>
    [Display(Name = "Выберите категорию, к которой относится услуга")]           // - это будет показано в браузере
    public int? ServiceCategoryId { get; set; }                                  // - идентификатор Категории
    public ServiceCategory? ServiceCategory{ get; set; }                         // - навигационное свойство к Категории Услуги.

    /// <summary>
    /// Краткое описание услуги.
    /// </summary>
    [Display(Name = "Краткое описание")]
    [MaxLength(3_000)]
    public string? DescriptionShort { get; set; }

    /// <summary>
    /// Полное описание Услуги.
    /// </summary>
    [Display(Name = "Описание")]
    [MaxLength(100_000)]
    public string? Description { get; set; }

    /// <summary>
    /// Титульная картинка для Услуги.
    /// </summary>
    [Display(Name = "Титульная картинка")]
    [MaxLength(300)]
    public string? Photo { get; set; }

    /// <summary>
    /// Тип услуги.
    /// </summary>
    [Display(Name = "Тип услуги")]
    public ServiceTypeEnum Type{ get; set; }
}