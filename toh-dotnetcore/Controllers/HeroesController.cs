using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using tohdotnetcore;
using tohdotnetcore.domain;
using tohdotnetcore.domain.Models;


namespace toh_dotnetcore.Controllers
{
    [Produces("application/json")]
    [Route("api/Heroes")]
    public class HeroesController : Controller
    {
        private readonly IHeroService _service;

        public HeroesController(tohdotnetcoreContext context, IHeroService service)
        {
            _service = service;
        }

        // GET: api/Heroes
        [HttpGet]
        public List<Hero> GetHero(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return _service.GetHeros();
            }
            else
            {
                var heros = _service.SearchHeros(name);
                return heros;
            }
        }

        // GET: api/Heroes/5
        [HttpGet("{id}")]
        public IActionResult GetHero([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hero = _service.GetHero(id);
            if (hero == null)
            {
                return NotFound();
            }

            return Ok(hero);
        }

        // PUT: api/Heroes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHero([FromRoute] int id, [FromBody] Hero hero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hero.Id)
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateHero(id, hero);
            }
            catch (DbUpdateConcurrencyException)
            {
                var tempHero = _service.GetHero(id);
                if (tempHero == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Heroes
        [HttpPost]
        public async Task<IActionResult> PostHero([FromBody] Hero hero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.CreateHero(hero);

            return CreatedAtAction("GetHero", new { id = hero.Id }, hero);
        }

        // DELETE: api/Heroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHero([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hero = _service.GetHero(id);
            if (hero == null)
            {
                return NotFound();
            }

            await _service.DeleteHero(hero);

            return Ok(hero);
        }
    }
}