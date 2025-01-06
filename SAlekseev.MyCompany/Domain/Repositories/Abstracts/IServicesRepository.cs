using SAlekseev.MyCompany.Domain.Entities;

namespace SAlekseev.MyCompany.Domain.Repositories.Abstracts;

public interface IServicesRepository
{
    Task<IEnumerable<Service>> GetServicesAsync();

    Task<Service?> GetServiceByIdAsync(int id);

    Task SaveServiceAsync(Service entity);

    Task DeleteServiceAsync(int id);
}
