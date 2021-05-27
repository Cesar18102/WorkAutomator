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
        [DbPermissionAspect(Table = DbTable.Manufactory, Action = InteractionDbType.CREATE)]
        [DbPermissionAspect(Table = DbTable.ManufactoryPlanPoint, Action = InteractionDbType.CREATE)]
        [DbPermissionAspect(Table = DbTable.CompanyPlanUniquePoint, Action = InteractionDbType.READ, CheckSameCompany = true)]
        public async Task<ManufactoryModel> CreateManufactory(AuthorizedDto<ManufactoryDto> model)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    ManufactoryEntity manufactory = model.Data.ToModel<ManufactoryModel>().ToEntity<ManufactoryEntity>();

                    ManufactoryEntity created = await db.GetRepo<ManufactoryEntity>().Create(manufactory);
                    await db.Save();

                    return created.ToModel<ManufactoryModel>();
                }
            });
        }
    }
}
