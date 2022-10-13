using birthreg.Data;
using birthreg.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace birthreg.Services
{
    public interface IParentService
    {
        Task<Parent> AddParent(Parent parent);
        Task<Parent> GetParentById(int id);
        Task<IEnumerable<Parent>> GetAllParent(string searchString = "");
    }
    public class ParentService : IParentService
    {
        private readonly BirthContext _context;

        public ParentService(BirthContext context)
        {
            _context = context;
        }
        public async Task<Parent> AddParent(Parent parent)
        {
            await _context.Parents.AddAsync(parent);
            await _context.SaveChangesAsync();
            return parent;
        }

        public async Task<IEnumerable<Parent>> GetAllParent(string searchString = "")
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await _context.Parents
                    .AsNoTracking()
              .ToListAsync();
            }
            else
            {
                return await _context.Parents
                    .Where(p => (p.FamilyName.ToLower().Contains(searchString.ToLower()) || p.FatherName.ToLower().Contains(searchString.ToLower()) || p.MotherName.ToLower().Contains(searchString.ToLower())))
              .ToListAsync();
            }
        }

        public async Task<Parent> GetParentById(int id)
        {
            return await _context.Parents.FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
