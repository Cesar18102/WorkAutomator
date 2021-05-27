using System.Threading.Tasks;

using Constants;
using Dto;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class ManufactoryService : ServiceBase, IManufactoryService
    {
        [DbPermissionAspect(Table = DbTable.Manufactory, Action = InteractionDbType.CREATE, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.DELETE, CheckSameCompany = true)]
        public async Task<ManufactoryModel> CreateManufactory(AuthorizedDto<ManufactoryDto> model)
        {
            /*return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    ManufactoryEntity manufactory = model.Data.ToModel<ManufactoryModel>().ToEntity<ManufactoryEntity>();

                    company.Owner = await db.GetRepo<AccountEntity>().Get(model.Session.UserId);

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
            });*/
            return null;
        }
    }
}
