using ExcelReader.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Service.Repository
{
    public interface IEmployeeService
    {
        IQueryable<Employee> GetAllEmployees();
       
    }
}
