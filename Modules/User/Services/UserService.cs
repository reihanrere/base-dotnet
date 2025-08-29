using BaseDotnet.Core.DbContext;
using BaseDotnet.Core.Helpers;
using BaseDotnet.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BaseDotnet.Core.Entities;


namespace BaseDotnet.Modules.User.Services
{
    public class UserService : IUserService
    {
        private readonly BaseDotnetDbContext _context;
        private readonly AppSettings _appSettings;
        public UserService(BaseDotnetDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<PagedResult<User>> GetAll(string? keyword, string? status, int? role, int page, int pageSize, string? orderBy, string? orderType)
        {

            IQueryable<User> userQuery = _context.Users;

            var query = userQuery.GroupJoin(_context.Roles, user => user.RoleID, role => role.RoleID, (user, role) => new { user, role })
                    .SelectMany(x => x.role.DefaultIfEmpty(), (user, role) => new { user.user, role });

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.user.Email.ToLower().Contains(keyword.ToLower())
                        || (m.user.UserName != null && m.user.UserName.ToLower().Contains(keyword.ToLower()))
                        || (m.user.Status != null && m.user.Status.ToLower().Contains(keyword.ToLower()))
                        || (m.user.DisplayName != null && m.user.DisplayName.ToLower().Contains(keyword.ToLower()))
                );
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(m => m.user.Status.ToLower().Equals(status.ToLower()));
            }

            if (role != null)
            {
                query = query.Where(m => m.user.RoleID == role);
            }

            orderBy = string.IsNullOrEmpty(orderBy) ? "userid" : orderBy.ToLower();
            orderType = string.IsNullOrEmpty(orderType) ? "asc" : orderType.ToLower();

            switch (orderBy)
            {
                case "userid":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.user.UserID) : query.OrderByDescending(x => x.user.UserID);
                    break;
                case "email":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.user.Email) : query.OrderByDescending(x => x.user.Email);
                    break;
                case "username":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.user.UserName) : query.OrderByDescending(x => x.user.UserName);
                    break;
                case "status":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.user.Status) : query.OrderByDescending(x => x.user.Status);
                    break;
                case "displayname":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.user.DisplayName) : query.OrderByDescending(x => x.user.DisplayName);
                    break;
                case "createddate":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.user.CreatedDate) : query.OrderByDescending(x => x.user.CreatedDate);
                    break;
                case "lastlogin":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.user.LastLogin) : query.OrderByDescending(x => x.user.LastLogin);
                    break;
                case "role":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.role.RoleName) : query.OrderByDescending(x => x.role.RoleName);
                    break;
                case "rolename":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.role.RoleName) : query.OrderByDescending(x => x.role.RoleName);
                    break;
                case "roleid":
                    query = orderType.Equals("asc") ? query.OrderBy(x => x.role.RoleID) : query.OrderByDescending(x => x.role.RoleID);
                    break;

                default:
                    query = query.OrderBy(x => x.user.UserID);
                    break;
            }

            var q = query.Select(x =>
            new User
            {
                UserID = x.user.UserID,
                UserName = x.user.UserName,
                DisplayName = x.user.DisplayName,
                Email = x.user.Email,
                Status = x.user.Status,
                LastLogin = x.user.LastLogin,
                CreatedDate = x.user.CreatedDate,
                RoleID = x.user.RoleID,
                RoleName = x.role.RoleName
            });

            int total = await q.CountAsync();
            if (page != -1)
                q = q.Skip((page - 1) * pageSize).Take(pageSize);

            return new PagedResult<User>(await q.ToListAsync(), total, page, pageSize);
        }

        public async Task<User> GetByID(int id)
        {
            return _context.Users.SingleOrDefault(x => x.UserID.Equals(id));
        }

        public async Task<int> Add(User item)
        {

            if (_context.Users.Any(u => u.Email == item.Email))
                throw new ApplicationException("Email is already in use.");

            if (_context.Users.Any(u => u.UserName == item.UserName))
                throw new ApplicationException("Username is already in use.");

            byte[] salt = Utils.GenerateSalt();
            item.Password = Utils.HashPassword(item.Password, salt);
            item.Salt = Convert.ToBase64String(salt);
            item.CreatedDate = DateTime.UtcNow;

            _context.Users.Add(item);
            _context.SaveChanges();
            return item.UserID;
        }
        public async Task<int> Remove(User item)
        {
            _context.Users.Remove(item);
            _context.SaveChanges();
            return item.UserID;
        }

        public async Task<int> Update(User item)
        {

            var user = await GetByID(item.UserID);
            if (user == null)
                throw new ApplicationException("User Not Found");

            if (_context.Users.Any(u => u.Email == item.Email && u.UserID != item.UserID))
                throw new ApplicationException("Email is already in use.");

            if (_context.Users.Any(u => u.UserName == item.UserName && u.UserID != item.UserID))
                throw new ApplicationException("Username is already in use.");


            if (!string.IsNullOrEmpty(item.Password))
            {
                byte[] salt = Utils.GenerateSalt();
                user.Password = Utils.HashPassword(item.Password, salt);
                user.Salt = Convert.ToBase64String(salt);
            }

            user.Email = item.Email;
            user.UserName = item.UserName;
            user.DisplayName = item.DisplayName;
            user.Status = item.Status;
            user.RoleID = item.RoleID;

            _context.Users.Update(user);
            _context.SaveChanges();
            return user.UserID;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email.Equals(model.Email));
            if (user != null && Utils.VerifyPassword(model.Password, user.Password, Convert.FromBase64String(user.Salt)))
            {
                var token = Utils.generateJwtToken(user, _appSettings.Secret);

                user.LastLogin = DateTime.UtcNow;
                _context.Users.Update(user);
                _context.SaveChanges();
                return new AuthenticateResponse(user, token);
            }
            else
            {
                return null;
            }
        }

        public async Task<string> SignUp(SignUpRequest item)
        {
            if (_context.Users.Any(u => u.Email == item.Email))
                throw new ApplicationException("Email is already in use.");

            if (_context.Users.Any(u => u.UserName == item.UserName))
                throw new ApplicationException("Username is already in use.");

            byte[] salt = Utils.GenerateSalt();

            var users = new User
            {
                UserName = item.UserName,
                Email = item.Email,
                Password = Utils.HashPassword(item.Password, salt),
                DisplayName = item.DisplayName,
                RoleID = 2,
                Status = "Active",
                CreatedDate = DateTime.UtcNow,
                Salt = Convert.ToBase64String(salt)
            };
            await _context.Users.AddAsync(users); // Menambahkan secara asinkron
            await _context.SaveChangesAsync();
            return item.DisplayName;
        }

        public async Task<User> GetByEmail(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<User> GetByUserName(string username)
        {
            return _context.Users.SingleOrDefault(x => x.UserName.ToLower().Equals(username.ToLower()));
        }
    }
}