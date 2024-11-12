using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System.IO;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync()
        {
            // Await the result of GetAllAsync, which returns Task<IEnumerable<Company>>
            var companies = await _companyRepository.GetAllAsync();

            // Map the result to IEnumerable<CompanyInfo>
            return _mapper.Map<IEnumerable<CompanyInfo>>(companies);
        }

        public async Task<Company> GetCompanyByCodeAsync(string companyCode)
        {
            // Assuming GetByCodeAsync is implemented in the repository
            var result = await _companyRepository.GetByCodeAsync(companyCode);
            return _mapper.Map<Company>(result);
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            bool entity = await _companyRepository.DeleteCompanyAsync(id);

            return true;
        }

        public async Task<bool> SaveCompanyAsync(Company company)
        {
            // Wrap the synchronous SaveCompany in a Task to avoid blocking.
            return await Task.Run(() =>
            {
                var isSuccessful = _companyRepository.SaveCompanyAsync(company);
                return isSuccessful;
            });
        }

        public async Task<bool> UpdateCompanyAsync(int id)
        {
            // Wrap the synchronous SaveCompany in a Task to avoid blocking.
            return await Task.Run(() =>
            {
                var isSuccessful = _companyRepository.UpdateCompanyAsync(id);  
                return isSuccessful;
            });
        }
    }
}
