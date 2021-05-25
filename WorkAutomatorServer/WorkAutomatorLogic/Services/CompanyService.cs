using System.Linq;
using System.Threading.Tasks;

using Autofac;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Models.Roles;
using WorkAutomatorLogic.Models.Permission;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorLogic.Services
{
    internal class CompanyService : ServiceBase, ICompanyService
    {
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.Company)]
        public async Task<CompanyModel> CreateCompany(UserActionModel<CompanyModel> model)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity company = model.Data.ToEntity<CompanyEntity>();

                    company.Owner = await db.GetRepo<AccountEntity>().Get(model.UserAccountId);

                    company.Owner.Roles.Remove(
                        company.Owner.Roles.First(
                            role => role.name == DefaultRoles.AUTHORIZED.ToName()
                        )
                    );

                    string ownerRoleName = DefaultRoles.OWNER.ToName();
                    company.Owner.Roles.Add(
                        await db.GetRepo<RoleEntity>().FirstOrDefault(
                            role => role.is_default && role.name == ownerRoleName
                        )
                    );

                    CompanyEntity created = await db.GetRepo<CompanyEntity>().Create(company);
                    company.Owner.Company = created;

                    await db.Save();

                    return created.ToModel<CompanyModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.UPDATE, Table = DbTable.Company, CheckSameCompany = true)]
        public async Task<CompanyModel> UpdateCompany(UserActionModel<CompanyModel> model)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity modified = await db.GetRepo<CompanyEntity>().Get(model.Data.Id);

                    modified.name = model.Data.Name;
                    modified.plan_image_url = model.Data.PlanImageUrl;

                    await db.Save();

                    return modified.ToModel<CompanyModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.DELETE, Table = DbTable.CompanyPlanUniquePoint)]
        public async Task<CompanyModel> SetupCompanyPlanPoints(UserActionModel<CompanyModel> model)
        {
            throw new System.NotImplementedException();
        }
    }
}
