using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _employeeDbWrapper.FindAll();
        }

        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {
            var itemRepo = await _employeeDbWrapper.FindAsync(t =>
                t.SiteId.Equals(employee.SiteId) && t.CompanyCode.Equals(employee.CompanyCode));

            var getEmployee = itemRepo.FirstOrDefault();

            if (getEmployee != null)
            {
                getEmployee.EmployeeCode = employee.EmployeeCode;
                getEmployee.EmployeeName = employee.EmployeeName;
                getEmployee.Occupation = employee.Occupation;
                getEmployee.EmployeeStatus = employee.EmployeeStatus;
                getEmployee.CompanyCode = employee.CompanyCode;
                getEmployee.EmailAddress = employee.EmailAddress;
                getEmployee.Phone = employee.Phone;
                getEmployee.LastModified = employee.LastModified;
                return await _employeeDbWrapper.UpdateAsync(getEmployee);
            }

            return await _employeeDbWrapper.InsertAsync(getEmployee);
        }
    }
}
