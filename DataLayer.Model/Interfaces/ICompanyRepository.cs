using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Model.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllAsync();  // Asynchronous method for getting all companies
        Task<IEnumerable<Company>> GetByCodeAsync(string companyCode);  // Asynchronous method for getting companies by code
        Task<bool> SaveCompanyAsync(Company company);  // Asynchronous method for saving a company

        Task<bool> DeleteCompanyAsync(int id);

        Task<bool> UpdateCompanyAsync(int id);
    
    }
}
