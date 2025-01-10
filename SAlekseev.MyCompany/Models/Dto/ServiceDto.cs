namespace SAlekseev.MyCompany.Models.Dto;

/// <summary>
/// DTO для Услуг. Предназначен для передачи необходимых данных на клиент (View, HTML)
/// Скрывает за собой архитектуру реальной доменной модели Service.
/// Состоит только из тех свойств, которые будут отображаться и/или изменяться на клиенте.
/// </summary>
public class ServiceDto
{
    public int Id { get; set; }
    public string? CategoryName { get; set; }
    public string? Title { get; set; }
    public string? DescriptionShort { get; set; }
    public string? Description { get; set; }
    public string? PhotoFileName { get; set; }
    public string? Type { get; set; }
}
