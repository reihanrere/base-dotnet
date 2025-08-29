namespace BaseDotnet.Controllers;

using Microsoft.AspNetCore.Mvc;
using BaseDotnet.Services;
using BaseDotnet.Entities;
using BaseDotnet.Models;
using BaseDotnet.Validation;
using BaseDotnet.Helpers;
using Serilog;

[ApiController]
[Route("[controller]")]

public class ProductCategoryController : BaseController
{
    private IProductCategoryService productCategoryService;

    private readonly ILogger<ProductCategoryController> _logger;
    private const string TAG = "ProductCategory {0}";
    private const string LOG = "ProductCategory {x}";
    public ProductCategoryController(IProductCategoryService productCategoryService, ILogger<ProductCategoryController> logger)
    {
        this.productCategoryService = productCategoryService;
        _logger = logger;
    }

    [Authorize(Page.MasterProductCategory, Action.Read)]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery(Name = "keyword")] string? keyword = null,
        [FromQuery(Name = "orderBy")] string? orderBy = "productCategoryName",
        [FromQuery(Name = "orderType")] string? orderType = "asc",
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "pageSize")] int pageSize = 10)
    {
        try
        {
            var productCategory = await productCategoryService.GetAll(keyword, page, pageSize, orderBy, orderType);
            if (productCategory == null)
                return _NotFound(string.Format(TAG, "Not Found"));

            return Ok(new
            {
                data = productCategory.Data,
                totalCount = productCategory.TotalCount,
                page = productCategory.Page,
                pageSize = productCategory.PageSize,
                totalPages = productCategory.TotalPages
            });
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }

    [Authorize(Page.MasterProductCategory, Action.Create)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCategoryRequest item)
    {
        try
        {
            if (item == null)
                return _BadRequest(string.Format(TAG, "Body Null"));

            var productCategoryValidator = new ProductCategoryCreateValidator();
            var result = productCategoryValidator.Validate(item);

            if (result.IsValid)
            {
                var id = await productCategoryService.Add(item);
                return _Ok(id, string.Format(TAG, "data has been successfully saved"));
            }
            else
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return _BadRequest(errorMessages);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }

    [Authorize(Page.MasterProductCategory, Action.Read)]
    [HttpGet("{id}")]
    public async Task<IActionResult> getDetail(int id)
    {
        try
        {
            var data = await productCategoryService.GetByID(id);
            if (data == null)
                return _NotFound(string.Format(TAG, "Not Found"));
            return _Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }

    [Authorize(Page.MasterProductCategory, Action.Read)]
    [HttpGet("option")]
    public async Task<IActionResult> getOption([FromQuery(Name = "keyword")] string? keyword = null,
        [FromQuery(Name = "orderBy")] string? orderBy = "productCategoryName",
        [FromQuery(Name = "orderType")] string? orderType = "asc",
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "pageSize")] int pageSize = 10)
    {
        try
        {
            var productCategory = await productCategoryService.GetAllOption(keyword, page, pageSize, orderBy, orderType);
            if (productCategory == null)
                return _NotFound(string.Format(TAG, "Not Found"));

            return Ok(new
            {
                data = productCategory,
            });
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }

    [Authorize(Page.MasterProductCategory, Action.Update)]
    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] ProductCategory item)
    {
        try
        {
            if (item == null)
                return _BadRequest(string.Format(TAG, "Body Null"));

            var productCategoryValidator = new ProductCategoryUpdateValidator();
            var result = productCategoryValidator.Validate(item);

            if (result.IsValid)
            {
                var id = await productCategoryService.Update(item);
                return _Ok(id, string.Format(TAG, "data has been successfully updated"));
            }
            else
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return _BadRequest(errorMessages);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }

    [Authorize(Page.MasterProductCategory, Action.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var data = await productCategoryService.GetByID(id);
            if (data == null)
                return _NotFound(string.Format(TAG, "Not Found"));

            await productCategoryService.Remove(data);

            return _Ok(id, string.Format(TAG, "data has been successfully deleted"));
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }
}

