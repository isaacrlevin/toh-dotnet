using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tohdotnet.domain;
using tohdotnet.domain.Models;


namespace toh_dotnet.AngularApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _service;

        public HeroesController(tohdotnetContext context, IHeroService service)
        {
            _service = service;
        }

        // GET: api/Heroes
        [HttpGet]
        public async Task<List<Hero>> GetHero(string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return await _service.SearchHeros(name);
            }
            else
            {
                return await _service.GetHeros();
            }
        }

        // GET: api/Heroes/5
        [HttpGet("{id}")]
        public Task<Hero> GetHero([FromRoute] int id)
        {
            var hero = _service.GetHero(id);
            return hero;
        }

        // PUT: api/Heroes/5
        [HttpPut("{id}")]
        public async Task PutHero([FromRoute] int id, [FromBody] Hero hero)
        {
            try
            {
                await _service.UpdateHero(id, hero);
            }
            catch (DbUpdateConcurrencyException)
            {
                var tempHero = _service.GetHero(id);
                if (tempHero != null)
                {
                    throw;
                }
            }
        }

        // POST: api/Heroes
        [HttpPost]
        public async Task<Hero> PostHero([FromBody] Hero hero)
        {
            return await _service.CreateHero(hero);
        }

        // DELETE: api/Heroes/5
        [HttpDelete("{id}")]
        public async Task DeleteHero([FromRoute] int id)
        {
            var hero = _service.GetHero(id);
            if (hero != null)
            {
                await _service.DeleteHero(hero.Result);
            }
        }
    }
}