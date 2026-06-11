using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleApi.Services;

namespace SaleApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RandomController: ControllerBase
    {
        private readonly IRandomService _randomService;

        public RandomController(IRandomService randomService)
        {
           _randomService = randomService;
        }


        [HttpPost("{giftId}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> RunDraw(int giftId)
        {
            try
            {
                var winner = await _randomService.ExecuteDraw(giftId);
                if (winner == null)
                {
                    return NotFound("לא נמצאו כרטיסים זכאים להגרלה עבור מוצר זה.");
                }

                return Ok(winner);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("is-drawn/{giftId}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CheckIfGiftDrawn(int giftId)
        {
            var result = await _randomService.IsGiftDrawnAsync(giftId);
            return Ok(result); 
        }

        [HttpGet("Winner")]

        public async Task<IActionResult> GetWinners()
        {
            var winners = await _randomService.GetAllWinnersAsync();
            return Ok(winners); 
        }
    }
}
