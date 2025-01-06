namespace SAlekseev.MyCompany.Domain.Entities;

/// <summary>
/// Сущность для Категории услуг
/// </summary>
public class ServiceCategory : EntityBase
{
    /// <summary>
    /// Навигационное свойство к Услугам.
    /// </summary>
    public ICollection<Service>? Services { get; set; }
}
