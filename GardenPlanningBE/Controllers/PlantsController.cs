using GardenPlanningBE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GardenPlanningBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly PlantsContext _plantsContext;
        public PlantsController(PlantsContext plantsContext)
        {
            _plantsContext = plantsContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plants>>> GetPlants() 
        {
            if (_plantsContext.Plants == null)
            {
                return NotFound();
            }
            return await _plantsContext.Plants.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plants>> GetPlants(int id)
        {
            if (_plantsContext.Plants == null)
            {
                return NotFound();
            }
            var plants = await _plantsContext.Plants.FindAsync(id);
            if(plants == null)
            {
                return NotFound();
            }
            return plants;
        }

        [HttpPost]
        public async Task<ActionResult<Plants>> PostPlants(Plants plants)
        { 
        _plantsContext.Plants.Add(plants);
            await _plantsContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlants), new { id = plants.ID},plants);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutPlants(int id, Plants plants)
        {
            if (id != plants.ID)
            { 
            return BadRequest();
            }
        
            _plantsContext.Entry(plants).State = EntityState.Modified;
            try
            {
                await _plantsContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlants(int id)
        {
            if (_plantsContext.Plants == null)
            {
                return NotFound();
            }
            var plants = await _plantsContext.Plants.FindAsync(id);
            if (plants == null)
            {
                return NotFound();
            }
            _plantsContext.Plants.Remove(plants);
            await _plantsContext.SaveChangesAsync();
        return Ok();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredPlants(string? name = null, string? type = null)
        {
            var query = _plantsContext.Plants.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.Type.Contains(type));
            }

            var plants = await query.ToListAsync();
            return Ok(plants);
        }

        

    }
}
