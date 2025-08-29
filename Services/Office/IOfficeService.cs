using BaseDotnet.Entities;
using BaseDotnet.Models;
namespace BaseDotnet.Services
{
    public interface IOfficeService
    {
        // Task<PagedResult<ProductCategory>> GetAll(string? keyword, int page, int pageSize, string? orderBy, string? orderType);
        // Task<List<OptionResponse>> GetAllOption(string? keyword, int page, int pageSize, string? orderBy, string? orderType);
        // Task<ProductCategory> GetByID(int id);
        Task<int> Add(Office item);
        // Task<int> Remove(ProductCategory item);
        // Task<int> Update(ProductCategory item);
    }
}