using System.Threading.Tasks;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.ServiceInterfaces;
using WorkAutomatorLogic.Models.Permission;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.RepoInterfaces;
using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorLogic.Services
{
    internal class CompanyService : ICompanyService
    {
        private static IRepo<CompanyEntity> CompanyRepo = RepoDependencyHolder.ResolveRealRepo<CompanyEntity>();
        private static IRepo<RoleEntity> RoleRepo = RepoDependencyHolder.ResolveRealRepo<RoleEntity>();

        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.Company)]
        public async Task<CompanyModel> CreateCompany(UserActionModel<CompanyModel> model)
        {
            //TODO
            CompanyEntity company = model.Data.ToEntity<CompanyEntity>();
            company.owner_id = model.UserAccountId;

            CompanyEntity created = await CompanyRepo.Create(company);
            //created.Owner.Roles.

            return company.ToModel<CompanyModel>();
        }

        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.DELETE, Table = DbTable.CompanyPlanUniquePoint)]
        public async Task<CompanyModel> SetupCompanyPlanPoints(UserActionModel<CompanyModel> model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CompanyModel> UpdateCompany(UserActionModel<CompanyModel> model)
        {
            throw new System.NotImplementedException();
        }
    }
}
