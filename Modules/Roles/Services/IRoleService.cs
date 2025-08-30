using BaseDotnet.Core.Models;
using BaseDotnet.Core.Entities;

namespace BaseDotnet.Modules.Roles.Services
{
    public interface IRoleService
    {
        Task<PagedResult<Role>> GetAll(string? keyword, int page, int pageSize, string? orderBy, string? orderType);
        Task<List<OptionResponse>> GetAllOption();
        Task<Role> GetByID(int id);
        Task<int> Add(Role item);
        Task<int> Remove(Role item);
        Task<int> Update(Role item);
    }
}