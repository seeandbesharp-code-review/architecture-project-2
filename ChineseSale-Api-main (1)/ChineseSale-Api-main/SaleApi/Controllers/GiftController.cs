using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SaleApi.Extensions;
using SaleApi.Models;
using SaleApi.Services;
using System.Text.Json;
using static SaleApi.Dto.DonerDto;
using static SaleApi.Dto.GiftDto;

namespace SaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private const string AllGiftsCacheKey = "all_gifts";

        private readonly IGiftService _giftService;
        private readonly IDistributedCache _cache;
        private readonly ILogger<GiftController> _logger;
        private readonly TimeSpan _cacheDuration;

        public GiftController(
            IGiftService giftService,
            IDistributedCache cache,
            ILogger<GiftController> logger,
            IConfiguration configuration)
        {
            _giftService = giftService;
            _cache = cache;
            _logger = logger;
            var ttlMinutes = configuration.GetValue("Redis:TtlMinutes", 10);
            _cacheDuration = TimeSpan.FromMinutes(ttlMinutes);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetGiftDto>>> GetAllGift()
        {
            try
            {
                try
                {
                    var cachedGifts = await _cache.GetStringAsync(AllGiftsCacheKey);
                    if (!string.IsNullOrEmpty(cachedGifts))
                    {
                        var giftsFromCache = JsonSerializer.Deserialize<List<GetGiftDto>>(cachedGifts);
                        if (giftsFromCache != null)
                        {
                            return Ok(giftsFromCache);
                        }
                    }
                }
                catch (Exception cacheEx)
                {
                    _logger.LogWarning(cacheEx, "Redis unavailable – serving gifts from database");
                }

                var gifts = await _giftService.GetAllGift();
                var giftsList = gifts.ToList();

                try
                {
                    await _cache.SetStringAsync(
                        AllGiftsCacheKey,
                        JsonSerializer.Serialize(giftsList),
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = _cacheDuration
                        });
                }
                catch (Exception cacheEx)
                {
                    _logger.LogWarning(cacheEx, "Could not write gifts to Redis cache");
                }

                return Ok(giftsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all gifts");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Gift>> NewGift([FromForm] CreateGiftDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var created = await _giftService.NewGift(dto);
                if (created == null)
                    return BadRequest("Failed to create gift.");

                await _cache.RemoveAsync(AllGiftsCacheKey);

                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletGift(int id)
        {
            try
            {
                await _giftService.DeletGift(id);
                await _cache.RemoveAsync(AllGiftsCacheKey);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting gift {GiftId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetGiftDto>> GetGiftById(int id)
        {
            try
            {
                var gift = await _giftService.GetGiftById(id);
                if (gift == null)
                    return NotFound();
                return Ok(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gift {GiftId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GiftResponseDto>> UpdateGift([FromForm] UpdateGiftDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updated = await _giftService.UpdateGift(dto);
                if (updated == null)
                    return BadRequest("Failed to update gift.");

                await _cache.RemoveAsync(AllGiftsCacheKey);

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/doner")]
        public async Task<ActionResult<GiftDonerDto>> GetDoner(int id)
        {
            var doner = await _giftService.GetGiftDoner(id);
            if (doner == null)
            {
                return NotFound($"Doner for gift ID {id} not found.");
            }
            return Ok(doner);
        }

        [HttpGet("doner/")]
        public async Task<ActionResult<IEnumerable<GetGiftDto>>> GetGiftByDoner([FromQuery] string name)
        {
            try
            {
                var gift = await _giftService.GetGiftByDoner(name);
                return Ok(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gifts by doner name {DonerName}", name);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name/")]
        public async Task<ActionResult<IEnumerable<UpdateGiftDto>>> GetGiftByName([FromQuery] string name)
        {
            try
            {
                var gift = await _giftService.GetGiftByName(name);
                return Ok(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gifts by name {GiftName}", name);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
