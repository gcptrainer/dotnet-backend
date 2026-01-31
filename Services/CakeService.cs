using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Services
{
    public class CakeService
    {
       private readonly ApplicationDbContext _context;
        public CakeService(ApplicationDbContext context)
        {
            _context=context;
            
        }
        public async Task<IEnumerable<Cake>>GetAllCakes()
        {
            return await _context.Cakes.ToListAsync();
        }
        public async Task<Cake>GetCakeById(int cakeId)
        {
            var survey = _context.Cakes.Find(cakeId);
            return survey;
        }
        public async Task<bool>AddCake(Cake cake)
        {
            _context.Cakes.Add(cake);
            //optimize later
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool>UpdateCake(int cakeId,Cake cake)
        {
            if(cake is null) return false;
            var exist = _context.Cakes.Find(cakeId);
            if(exist is null) return false;
            exist.Name=cake.Name;
            exist.Price=cake.Price;
            exist.Category=cake.Category;
            exist.Quantity=cake.Quantity;
            exist.CakeImage=cake.CakeImage;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool>DeleteCake(int cakeId)
        {
            var exist = _context.Cakes.Find(cakeId);
            if(exist is null) return false;
           _context.Cakes.Remove(exist);
           await _context.SaveChangesAsync();
           return true;
        }
    }
}
