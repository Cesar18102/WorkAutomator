﻿using System.Threading.Tasks;

using Dto;

using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface ICompanyService
    {
        Task<CompanyModel> CreateCompany(AuthorizedDto<CompanyDto> model);
        Task<CompanyModel> UpdateCompany(AuthorizedDto<CompanyDto> model);

        Task<CompanyModel> HireMember(AuthorizedDto<FireHireDto> model);
        Task<CompanyModel> FireMember(AuthorizedDto<FireHireDto> model);

        Task<CompanyModel> SetupCompanyPlanPoints(AuthorizedDto<CompanyDto> model);

        //CompanyModel SetupManufactory()
    }
}
