using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tohdotnetcore.domain.Models;

namespace tohdotnetcore.domain
{
    public interface IHeroService
    {
        List<Hero> GetHeros();
        Hero GetHero(int id);
        List<Hero> SearchHeros(string name);

        Task CreateHero(Hero hero);

        Task DeleteHero(Hero hero);

        Task UpdateHero(int id, Hero hero);
    }
    public class HeroService : IHeroService
    {
        private readonly tohdotnetcoreContext context;

        public HeroService(tohdotnetcoreContext _context)
        {
            context = _context;
        }
        public async Task CreateHero(Hero hero)
        {
            context.Hero.Add(hero);
            await context.SaveChangesAsync();
        }

        public async Task DeleteHero(Hero hero)
        {
            context.Hero.Remove(hero);
            await context.SaveChangesAsync();
        }

        public Hero GetHero(int id)
        {
           return context.Hero.FirstOrDefault(m => m.Id == id);
        }

        public List<Hero> GetHeros()
        {
            return context.Hero.ToList();
        }

        public List<Hero> SearchHeros(string name)
        {
            var heros = context.Hero.Where(m => m.Name.Contains(name)).ToList();
            var willThrow = heros[3];
            return heros;
        }

        public async Task UpdateHero(int id, Hero hero)
        {
            context.Entry(hero).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
