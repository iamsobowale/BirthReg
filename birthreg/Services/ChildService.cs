using birthreg.Data;
using birthreg.Models;
using birthreg.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace birthreg.Services
{
    public interface IChildService
    {
        Task<Child> AddChild(AddChildViewModel newChild);
        Task<Child> GetChildById(int id);
        Task<IEnumerable<Child>> GetChildrenByParent(int id);
        Task<IEnumerable<Child>> GetChildren(string searchString);
        Task<Child> GetChildByCertNo(string certNo);
        Task DeleteChild(int id);   

        Task<StatisticsViewModel> GetStatistics();
    }


    public class ChildService : IChildService
    {
        private readonly BirthContext _context;

        public ChildService(BirthContext context)
        {
            _context = context;
        }
        public async Task<Child> AddChild(AddChildViewModel newChild)
        {
            var child = new Child
            {
                FirstName = newChild.FirstName,
                LastName = newChild.LastName,
                Address = newChild.Address,
                ContactEmail = newChild.ContactEmail,
                ContactNumber = newChild.ContactNumber,
                Weigth = newChild.Weigth,
                Length = newChild.Length,
                MidWife = newChild.MidWife,
                Nationality = newChild.Nationality,
                Gender = newChild.Gender,
                State = newChild.State,
                DateOfBirth = newChild.DateOfBirth,
                ParentId = newChild.ParentId,
                DateRegistered = DateTime.Now,
                CertNo = "CN_"+DateTime.Now.ToString("yyyymmdd")
            };
            await _context.Children.AddAsync(child);
            await _context.SaveChangesAsync();
            return child;
        }

        public async Task DeleteChild(int id)
        {
            var child = await _context.Children.FirstOrDefaultAsync(x => x.Id == id);
            _context.Remove(child);
            await _context.SaveChangesAsync();
        }

        public async Task<Child> GetChildByCertNo(string certNo)
        {
            return await _context.Children
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(r => r.CertNo.ToLower() == certNo.ToLower());
        }

        public async Task<Child> GetChildById(int id)
        {
            return await _context.Children
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Child>> GetChildren(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return await _context.Children
                .Include(c => c.Parent)
                .ToListAsync();
            }
            else
            {
                return await _context.Children
                .Include(c => c.Parent)
                .Where(p => (p.FirstName.ToLower().Contains(searchString.ToLower()) || p.LastName.ToLower().Contains(searchString.ToLower()) || p.Parent.FamilyName.ToLower().Contains(searchString.ToLower())))
                .ToListAsync();
            }
            
        }

        public async Task<IEnumerable<Child>> GetChildrenByParent(int id)
        {
            return await _context.Children.Where(c => c.ParentId == id).ToListAsync();
        }

        public async Task<StatisticsViewModel> GetStatistics()
        {
            var totalChidren = await _context.Children.CountAsync();
            var totalParent = await _context.Parents.CountAsync();

            var statistics = new StatisticsViewModel
            {
                TotalChildren = totalChidren,
                TotalParent = totalParent,
            };

            return statistics;
        }
    }
}
