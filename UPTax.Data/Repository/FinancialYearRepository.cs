using System;
using System.Data.Entity;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IFinancialYearRepository : IRepository<FinancialYear>
    {
        int GetCurrentYear(int month);
    }
    public class FinancialYearRepository : Repository<FinancialYear>, IFinancialYearRepository
    {
        AdminContext _context;
        public FinancialYearRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }

        public int GetCurrentYear(int month)
        {
            var today = DateTime.Now;
            var currentFinancialYear = _context.FinancialYears.Where(r => r.StartDate <= today && r.EndDate >= today).FirstOrDefault();
            var tempDate = new DateTime(today.Year, month, DateTime.DaysInMonth(today.Year, month));
            if (tempDate > currentFinancialYear.EndDate)
            {
                return today.Year - 1;
            }
            else if (tempDate < currentFinancialYear.StartDate)
            {
                return today.Year + 1;
            }
            return today.Year;

        }
    }
}
