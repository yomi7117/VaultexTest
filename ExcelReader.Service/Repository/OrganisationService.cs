
using ExcelReader.Domain.Data;
using ExcelReader.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Service.Repository
{
    public class OrganisationService : IOrganisationService
    {
        private readonly AppDbContext _context;
        public OrganisationService(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Organisation> GetAllOrganisations()
        {
            return _context.Organisations.AsNoTracking();
        }
    }
}
