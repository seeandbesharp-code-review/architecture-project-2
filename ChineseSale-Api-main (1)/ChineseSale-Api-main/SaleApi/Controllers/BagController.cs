using Microsoft.AspNetCore.Mvc;
using SaleApi.Models;
using SaleApi.Services;
using static SaleApi.Dto.BagDto;

namespace SaleApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BagController : ControllerBase
    {

        private readonly IBagService _bagService;
        private readonly ILogger<BagController> _logger;


        public BagController(IBagService bagService, ILogger<BagController> logger)
        {
            _bagService = bagService;
            _logger = logger;
        }

        // כל הסלים
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBagDto>>> GetAllBag()
        {
            var bag = await _bagService.GetAllBag();
            return Ok(bag);
        }
        //new  bag
        [HttpPost("add")]
        public async Task<IActionResult> AddToBag([FromBody] CreateBagDto bagDto)
        {
            try
            {
                var result = await _bagService.NewGiftToBag(bagDto);
                if (result == null) return BadRequest();

                // חשו0ב מאוד: מחזירים את האובייקט המלא כולל ה-Gift כדי שהתצוגה לא תימחק
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //מחיקת מהסל
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletBag(int id)
        {
            try
            {
                await _bagService.DeleteBag(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        //get by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Bag>> GetBagById(int id)
        {
            try
            {
                var bag = await _bagService.GetBagById(id);
                if (bag == null)
                    return NotFound();
                return Ok(bag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // get by user id
        [HttpGet("user/{id}")]
        public async Task<ActionResult<Bag>> GetBagByUser(int id)
        {
            try
            {
                var bag = await _bagService.GetBagByUser(id);
                if (bag == null)
                    return NotFound();
                return Ok(bag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // get by gift id
        [HttpGet("gift/{id}")]
        public async Task<ActionResult<Bag>> GetBagByGift(int id)
        {
            try
            {
                var bag = await _bagService.GetBagByGift(id);
                if (bag == null)
                    return NotFound();
                return Ok(bag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("checkout/{userId}")]
        public async Task<IActionResult> Checkout(int userId)
        {
            var result = await _bagService.ProcessCheckout(userId);

            if (result)
            {
                return Ok(new { message = "הרכישה הושלמה בהצלחה, הסל רוקן והכרטיסים עברו להזמנות." });
            }

            return BadRequest("לא ניתן לבצע רכישה. ייתכן והסל ריק.");
        }

        ////האם יש זוכה במתנה 
        //[HttpGet("is-drawn/{giftId}")]
        //public async Task<IActionResult> CheckIfDrawn(int giftId)
        //{
        //    bool isDrawn = await _bagService.(giftId);
        //    return Ok(isDrawn);
        //}


    }
}


//using Microsoft.AspNetCore.Mvc;
//using SaleApi.Models;
//using SaleApi.Services;
//using static SaleApi.Dto.BagDto;

//namespace SaleApi.Controllers
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class BagController : ControllerBase
//    {

//        private readonly IBagService _bagService;
//        private readonly ILogger<BagController> _logger;


//        public BagController(IBagService bagService, ILogger<BagController> logger)
//        {
//            _bagService = bagService;
//            _logger = logger;
//        }

//        // כל הסלים
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<GetBagDto>>> GetAllBag()
//        {
//            var bag = await _bagService.GetAllBag();
//            return Ok(bag);
//        }
//        // סל חדש

//        [HttpPost]
//        public async Task<ActionResult<CreateBagDto>> NewGiftToBag([FromBody] CreateBagDto bagDto)
//        {
//            if (!ModelState.IsValid)
//            {
//             _logger.LogWarning("Invalid model state for NewGiftToBag request.");
//           return BadRequest(ModelState);
//            }

//            try
//            {
//                var created = await _bagService.NewGiftToBag(bagDto);
//                if (created == null)
//                    return BadRequest("Failed to create bag.");
//                return Ok(created);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        //מחיקת מהסל
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeletBag(int id)
//        {
//            try
//            {
//                await _bagService.DeleteBag(id);
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Internal server error");
//            }
//        }


//        //get by id
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Bag>> GetBagById(int id)
//        {
//            try
//            {
//                var bag = await _bagService.GetBagById(id);
//                if (bag == null)
//                    return NotFound();
//                return Ok(bag);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Internal server error");
//            }
//        }

//       // get by user id
//        [HttpGet("user/{id}")]
//        public async Task<ActionResult<Bag>> GetBagByUser(int id)
//        {
//            try
//            {
//                var bag = await _bagService.GetBagByUser(id);
//                if (bag == null)
//                    return NotFound();
//                return Ok(bag);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        // get by gift id
//        [HttpGet("gift/{id}")]
//        public async Task<ActionResult<Bag>> GetBagByGift(int id)
//        {
//            try
//            {
//                var bag = await _bagService.GetBagByGift(id);
//                if (bag == null)
//                    return NotFound();
//                return Ok(bag);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }
//}
