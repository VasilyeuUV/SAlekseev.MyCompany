using Microsoft.AspNetCore.Mvc;
using SAlekseev.MyCompany.Domain.Entities;
using SAlekseev.MyCompany.Models.Dto;

namespace SAlekseev.MyCompany.Infrastructure;

/// <summary>
/// Помощник по преобразованию доменных сущностей в DTO
/// </summary>
public static class DtoHelper
{
    /// <summary>
    /// Превращает Объект Service в объект ServiceDto
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ServiceDto TransformService(Service entity)
    {
        ServiceDto entityDto = new ServiceDto()
        {
            Id = entity.Id,
            CategoryNamed = entity.ServiceCategory?.Title,
            Title = entity.Title,
            DescriptionShort = entity.DescriptionShort,
            Description = entity.Description,
            PhotoFileName = entity.Photo,
            Type = entity.Type.ToString(),
        };
        return entityDto;
    }


    /// <summary>
    /// Преобразовать сразу всю коллекцию.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public static IEnumerable<ServiceDto> TransformServices(IEnumerable<Service> entities)
    {
        List<ServiceDto> dtoEntities = new List<ServiceDto>();

        foreach (Service entity in entities)
        {
            dtoEntities.Add(TransformService(entity));
        }
        return dtoEntities;
        
    }
}
