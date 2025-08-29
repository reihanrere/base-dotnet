using BaseDotnet.DbContext;
using BaseDotnet.Entities;
using BaseDotnet.Helpers;
using BaseDotnet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BaseDotnet.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly BaseDotnetDbContext _context;
        private readonly AppSettings _appSettings;
        public ProductCategoryService(BaseDotnetDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<int> Add(ProductCategoryRequest item)
        {
            if (_context.ProductCategories.Any(u => u.ProductCategoryName == item.ProductCategoryName))
                throw new ApplicationException("ProductCategoryName is already in use.");

            var productCategory = new ProductCategory
            {
                ProductCategoryName = item.ProductCategoryName,
                ProductCategoryDescription = item.ProductCategoryDescription
            };

            _context.ProductCategories.Add(productCategory);
            _context.SaveChanges();
            return productCategory.ProductCategoryID;
        }

        public async Task<PagedResult<ProductCategory>> GetAll(string? keyword, int page, int pageSize, string? orderBy, string? orderType)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories;

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.ProductCategoryName.ToLower().Contains(keyword.ToLower()));
            }

            orderBy = string.IsNullOrEmpty(orderBy) ? "productCategoryName" : orderBy.ToLower();
            orderType = string.IsNullOrEmpty(orderType) ? "asc" : orderType.ToLower();

            switch (orderBy)
            {
                default:
                    query = query.OrderBy(x => x.ProductCategoryName);
                    break;
            }

            int total = await query.CountAsync();
            if (page != -1)
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return new PagedResult<ProductCategory>(await query.ToListAsync(), total, page, pageSize);
        }

        public async Task<List<OptionResponse>> GetAllOption(string? keyword, int page, int pageSize, string? orderBy, string? orderType)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories;

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.ProductCategoryName.ToLower().Contains(keyword.ToLower()));
            }

            orderBy = string.IsNullOrEmpty(orderBy) ? "productCategoryName" : orderBy.ToLower();
            orderType = string.IsNullOrEmpty(orderType) ? "asc" : orderType.ToLower();

            switch (orderBy)
            {
                default:
                    query = query.OrderBy(x => x.ProductCategoryName);
                    break;
            }

            int total = await query.CountAsync();
            if (page != -1)
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.Select(g => new
                OptionResponse
            {
                label = g.ProductCategoryName,
                value = g.ProductCategoryID.ToString(),
                desc = g.ProductCategoryDescription,
            }).ToListAsync();
        }

        public async Task<ProductCategory> GetByID(int id)
        {
            return _context.ProductCategories.SingleOrDefault(x => x.ProductCategoryID.Equals(id));
        }

        public async Task<int> Remove(ProductCategory item)
        {
            _context.ProductCategories.Remove(item);
            _context.SaveChanges();
            return item.ProductCategoryID;
        }

        public async Task<int> Update(ProductCategory item)
        {
            var data = await GetByID(item.ProductCategoryID);
            if (data == null)
                throw new ApplicationException("ProductCategory not found");

            data.ProductCategoryName = item.ProductCategoryName;
            data.ProductCategoryDescription = item.ProductCategoryDescription;
            _context.ProductCategories.Update(data);
            _context.SaveChanges();
            return data.ProductCategoryID;
        }
    }
}