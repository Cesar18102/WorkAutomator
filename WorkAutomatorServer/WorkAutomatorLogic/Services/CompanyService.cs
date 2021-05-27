using System.Linq;
using System.Threading.Tasks;

using Dto;

using Constants;

using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.ServiceInterfaces;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorLogic.Services
{
    internal class CompanyService : ServiceBase, ICompanyService
    {
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.Company)]
        public async Task<CompanyModel> CreateCompany(AuthorizedDto<CompanyDto> model)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity company = model.Data.ToModel<CompanyModel>().ToEntity<CompanyEntity>();

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
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.UPDATE, Table = DbTable.Company, CheckSameCompany = true)]
        public async Task<CompanyModel> UpdateCompany(AuthorizedDto<CompanyDto> model)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity modifying = await db.GetRepo<CompanyEntity>().Get(model.Data.Id.Value);

                    modifying.name = model.Data.Name;
                    modifying.plan_image_url = model.Data.PlanImageUrl;

                    await db.Save();

                    return modifying.ToModel<CompanyModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.UPDATE, Table = DbTable.Company, CheckSameCompany = true)]
        public async Task<CompanyModel> HireMember(AuthorizedDto<FireHireDto> model)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(model.Data.AccountId.Value);

                    if (account == null)
                        throw new NotFoundException("Account");

                    if (account.company_id.HasValue)
                        throw new AlreadyHiredException();

                    account.company_id = model.Data.CompanyId;

                    await db.Save();

                    string authorizedRoleName = DefaultRoles.AUTHORIZED.ToName();
                    account.Roles.Remove(
                        await db.GetRepo<RoleEntity>().FirstOrDefault(
                            role => role.is_default && role.name == authorizedRoleName
                        )
                    );

                    CompanyEntity company = await db.GetRepo<CompanyEntity>().Get(model.Data.CompanyId.Value);
                    return company.ToModel<CompanyModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.UPDATE, Table = DbTable.Company, CheckSameCompany = true)]
        public async Task<CompanyModel> FireMember(AuthorizedDto<FireHireDto> model)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(model.Data.AccountId.Value);

                    if (account == null)
                        throw new NotFoundException("Account");

                    if (!account.company_id.HasValue || account.company_id != model.Data.CompanyId)
                        throw new NotHiredException();

                    if (account.company_id.Value == account.id) //OWNER
                        throw new NotPermittedException();

                    account.company_id = null;
                    account.Roles.Clear();

                    string authorizedRoleName = DefaultRoles.AUTHORIZED.ToName();
                    account.Roles.Add(
                        await db.GetRepo<RoleEntity>().FirstOrDefault(
                            role => role.is_default && role.name == authorizedRoleName
                        )
                    );

                    await db.Save();

                    CompanyEntity company = await db.GetRepo<CompanyEntity>().Get(model.Data.CompanyId.Value);
                    return company.ToModel<CompanyModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.DELETE, CheckSameCompany = true)]
        public async Task<CompanyModel> SetupCompanyPlanPoints(AuthorizedDto<CompanyPlanPointsDto> model)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity company = await db.GetRepo<CompanyEntity>().Get(model.Data.CompanyId.Value);

                    await db.GetRepo<CompanyPlanUniquePointEntity>().Delete(
                        company.CompanyPlanUniquePoints.Select(p => p.id).ToArray()
                    );

                    foreach (CompanyPlanPointDto point in model.Data.Points)
                    { 
                        company.CompanyPlanUniquePoints.Add(
                            new CompanyPlanUniquePointEntity()
                            {
                                x = point.X,
                                y = point.Y
                            }
                        );
                    }

                    await db.Save();
                    return company.ToModel<CompanyModel>();
                }
            });
        }
    }
}
