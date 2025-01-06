using SAlekseev.MyCompany.Domain.Entities;

namespace SAlekseev.MyCompany.Domain.Repositories.Abstracts;

public interface IServiceCategoriesRepository
{
    Task<IEnumerable<ServiceCategory>> GetServiceCategoriesAsync();

    Task<ServiceCategory?> GetServiceCategoryByIdAsync(int id);

    Task SaveServiceCategoryAsync(ServiceCategory entity);

    Task DeleteServiceCategoryAsync(int id);
}
