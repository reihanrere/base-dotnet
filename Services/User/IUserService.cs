using BaseDotnet.Entities;
using BaseDotnet.Models;
namespace BaseDotnet.Services
{
    public interface IUserService
    {
        Task<PagedResult<User>> GetAll(string? keyword, string? status, int? role, int page, int pageSize, string? orderBy, string? orderType);
        Task<User> GetByID(int id);
        Task<User> GetByEmail(string email);
        Task<User> GetByUserName(string username);
        Task<int> Add(User item);
        Task<int> Remove(User id);
        Task<int> Update(User item);
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        Task<string> SignUp(SignUpRequest signUpRequest);
    }
}