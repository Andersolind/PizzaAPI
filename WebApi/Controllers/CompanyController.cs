using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }
        // GET api/<controller>
        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {

            // Await the async call to GetAllCompaniesAsync
            var items = await _companyService.GetAllCompaniesAsync();

            // Map the result asynchronously (if _mapper.Map is non-blocking, this should be fine)
            return _mapper.Map<IEnumerable<CompanyDto>>(items);
        }

        // GET api/<controller>/5
        public async Task<CompanyDto> Get(string companyCode)
        {
            var item = await _companyService.GetCompanyByCodeAsync(companyCode);
            return _mapper.Map<CompanyDto>(item);
        }

        // POST api/<controller>
        public async Task<bool> PostAsync(Company company)
        {
            try
            {
                var isSaved = await _companyService.SaveCompanyAsync(company);

                return isSaved;

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        // PUT api/<controller>/5

        public async Task<IEnumerable> PutAsync(int id, [FromBody] string companyCode)
        {
            var isCompany = await _companyService.GetCompanyByCodeAsync(companyCode);

            try
            {

                var isUpdated = await _companyService.UpdateCompanyAsync(id);

                return _mapper.Map<IEnumerable<CompanyDto>>(isUpdated);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        // DELETE api/<controller>/5
        public async Task<bool> Delete(int id)
        {
            try
            {
                return await _companyService.DeleteCompanyAsync(id);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }
    }
}