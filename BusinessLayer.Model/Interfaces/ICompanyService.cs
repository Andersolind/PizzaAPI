using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync();  
        Task<Company> GetCompanyByCodeAsync(string companyCode);  
        Task<bool> SaveCompanyAsync(Company company); 
        Task<bool> UpdateCompanyAsync(int id);

        Task<bool> DeleteCompanyAsync(int companyId);
    }
}
