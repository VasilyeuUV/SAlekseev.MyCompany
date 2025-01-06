using SAlekseev.MyCompany.Domain.Repositories.Abstracts;

namespace SAlekseev.MyCompany.Domain;

/// <summary>
/// Менеджер репозиториев - Класс-обертка над репозиториями для концентрации их в одном месте.
/// </summary>
public class DataManager
{
    /// <summary>
    /// CTOR
    /// </summary>
    public DataManager(
        IServiceCategoriesRepository serviceCategoriesRepository,
        IServicesRepository servicesRepository
        )
    {
        ServiceCategories = serviceCategoriesRepository;
        Services = servicesRepository;
    }

    public IServiceCategoriesRepository ServiceCategories { get; set; }
    public IServicesRepository Services { get; set; }
}
