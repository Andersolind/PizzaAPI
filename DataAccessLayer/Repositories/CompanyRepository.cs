using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbWrapper<Company> _companyDbWrapper;

        public CompanyRepository(IDbWrapper<Company> companyDbWrapper)
        {
            _companyDbWrapper = companyDbWrapper;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyDbWrapper.FindAll();
        }

        public async Task<IEnumerable<Company>> GetByCodeAsync(string companyCode)
        {
            // Use FindAsync to return all matching companies as an IEnumerable
            var companies = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));

            // Return the entire IEnumerable collection
            return companies;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var entity = await _companyDbWrapper.FindAsync(siteId => siteId.SiteId.Equals(id));
            if (entity == null)
            {
                return false;
            }

            await _companyDbWrapper.DeleteAsync(idxCompany => idxCompany.SiteId.Equals(id));


            return true;
        }

        public async Task<bool> UpdateCompanyAsync(int id)
        {
            var entity = await _companyDbWrapper.FindAsync(siteId => siteId.SiteId.Equals(id));
            if (entity == null)
            {
                return false;
            }
            var getCompany = entity.FirstOrDefault();

            await _companyDbWrapper.UpdateAsync(getCompany);


            return true;
        }
        public async Task<bool> SaveCompanyAsync(Company company)
        {
            
            var savedCompany = await _companyDbWrapper.FindAsync(t =>
                            t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode));

            var getCompany = savedCompany.FirstOrDefault();


            if (getCompany != null)
            {
                // Update the existing company information
                getCompany.CompanyName = company.CompanyName;
                getCompany.AddressLine1 = company.AddressLine1;
                getCompany.AddressLine2 = company.AddressLine2;
                getCompany.AddressLine3 = company.AddressLine3;
                getCompany.Country = company.Country;
                getCompany.EquipmentCompanyCode = company.EquipmentCompanyCode;
                getCompany.FaxNumber = company.FaxNumber;
                getCompany.PhoneNumber = company.PhoneNumber;
                getCompany.PostalZipCode = company.PostalZipCode;
                getCompany.LastModified = company.LastModified;

                // Use async Update method
                return await _companyDbWrapper.UpdateAsync(getCompany);
            }

            // Use async Insert method
            return await _companyDbWrapper.InsertAsync(company);
        }
    }
}
