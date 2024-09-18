using Microsoft.AspNetCore.Mvc;
using CSharpRPG.Models;
using CSharpRPG.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpRPG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BattleController : ControllerBase
    {
        private readonly IBattleService _battleService;

        public BattleController(IBattleService battleService)
        {
            _battleService = battleService;
        }

        // POST: api/battle/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult> CreateBattlesBulk([FromBody] IEnumerable<Battle> battles)
        {
            foreach (var battle in battles)
            {
                await _battleService.CreateBattleAsync(battle);
            }
            return Ok("Bulk battles created successfully.");
        }

        // Get all battles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Battle>>> GetBattles()
        {
            var battles = await _battleService.GetBattlesAsync();
            return Ok(battles);
        }

        // Get a battle by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Battle>> GetBattle(string id)
        {
            var battle = await _battleService.GetBattleByIdAsync(id);
            if (battle == null)
            {
                return NotFound();
            }
            return Ok(battle);
        }

        [HttpPost("{battleId}/fight")]
        public async Task<ActionResult<BattleResult>> FightBattle(string battleId, [FromBody] FightRequest request)
        {
            try
            {
                var result = await _battleService.ExecuteBattle(request.CharacterId, battleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class FightRequest
        {
            public string CharacterId { get; set; }
        }

    }
}
