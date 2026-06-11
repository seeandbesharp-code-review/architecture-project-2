using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleApi.Models;
using SaleApi.Services;
using static SaleApi.Dto.CategoryDto;
using static SaleApi.Dto.DonerDto;
using static SaleApi.Dto.GiftDto;

namespace SaleApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService cotegoryService, ILogger<CategoryController> logger)
        {
            _categoryService = cotegoryService;
            _logger = logger;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategory()
        {
            var category = await _categoryService.GetAllCategory();
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> NewCategory([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _categoryService.NewCategory(dto);
                if (created == null)
                    return BadRequest("Failed to create category.");

                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoner(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error יש מוצרים בקטגוריה הזו");
            }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                    return NotFound();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> UpdateCategory([FromBody] GetCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updated = await _categoryService.UpdateCategory(dto);
                if (updated == null)
                    return BadRequest("Failed to update category.");
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("gift/{categoryId}")]
        public async Task<ActionResult<List<GiftResponseDto>>> GetGiftByCategoryId(int categoryId)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(categoryId);
                if (category == null)
                    return NotFound($"Category with ID {categoryId} not found.");

                var gifts = await _categoryService.GetGiftByCategoryId(categoryId);
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
