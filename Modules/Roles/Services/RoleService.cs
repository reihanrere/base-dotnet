using BaseDotnet.Core.DbContext;
using BaseDotnet.Core.Models;
using Microsoft.EntityFrameworkCore;
using BaseDotnet.Core.Entities;

namespace BaseDotnet.Modules.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly BaseDotnetDbContext _context;

        public RoleService(BaseDotnetDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Role>> GetAll(string? keyword, int page, int pageSize, string? orderBy, string? orderType)
        {
            IQueryable<Role> query = _context.Roles;

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.RoleName.ToLower().Contains(keyword.ToLower()) ||
                                   (m.Description != null && m.Description.ToLower().Contains(keyword.ToLower())));
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

            return new PagedResult<Role>(await query.ToListAsync(), total, page, pageSize);
        }

        public async Task<List<OptionResponse>> GetAllOption()
        {
            return await _context.Roles.Select(g => new OptionResponse
            {
                label = g.RoleName,
                value = g.RoleID.ToString(),
                desc = g.Description
            }).ToListAsync();
        }

        public async Task<Role> GetByID(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.RoleID == id);
        }

        public async Task<int> Add(Role item)
        {
            if (_context.Roles.Any(u => u.RoleName == item.RoleName))
                throw new ApplicationException("RoleName is already in use.");

            _context.Roles.Add(item);
            await _context.SaveChangesAsync();
            return item.RoleID;
        }

        public async Task<int> Remove(Role item)
        {
            _context.Roles.Remove(item);
            await _context.SaveChangesAsync();
            return item.RoleID;
        }

        public async Task<int> Update(Role item)
        {
            var data = await GetByID(item.RoleID);
            if (data == null)
                throw new ApplicationException("Role Not Found");

            if (_context.Roles.Any(u => u.RoleName == item.RoleName && u.RoleID != item.RoleID))
                throw new ApplicationException("RoleName is already in use.");

            data.RoleName = item.RoleName;
            data.Description = item.Description;
            data.Permissions = item.Permissions;

            _context.Roles.Update(data);
            await _context.SaveChangesAsync();
            return data.RoleID;
        }
    }
}