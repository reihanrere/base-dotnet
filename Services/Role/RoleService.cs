using BaseDotnet.DbContext;
using BaseDotnet.Entities;
using Microsoft.EntityFrameworkCore;
using BaseDotnet.Models;
using Microsoft.Extensions.Options;
using BaseDotnet.Helpers;


namespace BaseDotnet.Services
{
    public class RoleService : IRoleService
    {
        private readonly BaseDotnetDbContext _context;
        private readonly AppSettings _appSettings;
        public RoleService(BaseDotnetDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<PagedResult<Role>> GetAll(string? keyword, int page, int pageSize, string? orderBy, string? orderType)
        {

            IQueryable<Role> query = _context.Roles;


            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.RoleName.ToLower().Contains(keyword.ToLower()));
            }


            orderBy = string.IsNullOrEmpty(orderBy) ? "rolename" : orderBy.ToLower();
            orderType = string.IsNullOrEmpty(orderType) ? "asc" : orderType.ToLower();

            switch (orderBy)
            {
                case "roleid":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.RoleID) : query.OrderByDescending(x => x.RoleID);
                    break;
                default:
                    query = query.OrderBy(x => x.RoleName);
                    break;
            }

            int total = await query.CountAsync();
            if (page != -1)
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return new PagedResult<Role>(await query.ToListAsync(), total);
        }

        public async Task<List<OptionResponse>> GetAllOption(string? keyword, int page, int pageSize, string? orderBy, string? orderType)
        {
            IQueryable<Role> query = _context.Roles;

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.RoleName.ToLower().Contains(keyword.ToLower()));
            }

            orderBy = string.IsNullOrEmpty(orderBy) ? "rolename" : orderBy.ToLower();
            orderType = string.IsNullOrEmpty(orderType) ? "asc" : orderType.ToLower();

            switch (orderBy)
            {
                case "roleid":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.RoleID) : query.OrderByDescending(x => x.RoleID);
                    break;
                default:
                    query = query.OrderBy(x => x.RoleName);
                    break;
            }

            int total = await query.CountAsync();
            if (page != -1)
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.Select(g => new
                OptionResponse
            {
                label = g.RoleName,
                value = g.RoleID.ToString()
            }).ToListAsync();
        }

        public async Task<Role> GetByID(int id)
        {
            return _context.Roles.SingleOrDefault(x => x.RoleID.Equals(id));
        }

        public async Task<int> Add(Role item)
        {

            if (_context.Roles.Any(u => u.RoleName == item.RoleName))
                throw new ApplicationException("RoleName is already in use.");

            _context.Roles.Add(item);
            _context.SaveChanges();
            return item.RoleID;
        }
        public async Task<int> Remove(Role item)
        {
            _context.Roles.Remove(item);
            _context.SaveChanges();
            return item.RoleID;
        }

        public async Task<int> Update(Role item)
        {
            var data = await GetByID(item.RoleID);
            if (data == null)
                throw new ApplicationException("Role Not Found");

            if (_context.Roles.Any(u => u.RoleName == item.RoleName && u.RoleID != item.RoleID))
                throw new ApplicationException("Email is already in use.");

            data.RoleName = item.RoleName;

            _context.Roles.Update(data);
            _context.SaveChanges();
            return data.RoleID;
        }

        public async Task<RolePageAccess> GetPageAccessByPageCode(Page page, int roleId)
        {
            Console.WriteLine("cek role service");
            Console.WriteLine(page.ToString());
            Console.WriteLine(roleId);
            return _context.RolePageAccesses.SingleOrDefault(x => x.RoleID.Equals(roleId) && x.PageCode.Equals(page.ToString()));
        }

        public async Task<List<RolePageAccess>> GetPageAccessByRoleID(int roleId)
        {
            return _context.RolePageAccesses.Where(m => m.RoleID == roleId).ToList();
        }

        public async Task<List<RolePage>> GetAllPageRole()
        {
            return _context.RolePages.ToList();
        }

        public async Task<int> deletePageAccessByRole(int roleId)
        {
            var data = _context.RolePageAccesses.Where(x => x.RoleID == roleId).ToList();
            _context.RolePageAccesses.RemoveRange(data);
            return _context.SaveChanges();
        }

        public async Task<int> AddPageAccess(List<RolePageAccess> items)
        {
            _context.RolePageAccesses.AddRange(items);
            return _context.SaveChanges();
        }
        public async Task<int> AddPageAccess(RolePageAccess item)
        {
            _context.RolePageAccesses.Add(item);
            return _context.SaveChanges();
        }

    }


}