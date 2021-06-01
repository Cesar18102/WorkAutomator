using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

                    IList<RoleEntity> defaultRoles = await db.GetRepo<RoleEntity>().Get(role => role.is_default);

                    company.Owner.Roles.Remove(defaultRoles.FirstOrDefault(role => role.name == DefaultRoles.AUTHORIZED.ToName()));
                    company.Owner.Roles.Add(defaultRoles.FirstOrDefault(role => role.name == DefaultRoles.OWNER.ToName()));

                    CompanyEntity created = await db.GetRepo<CompanyEntity>().Create(company);
                    company.Owner.Company = created;

                    await db.Save();

                    return await GetCompany(created.owner_id);
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

                    return await GetCompany(modifying.owner_id);
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

                    IList<RoleEntity> defaultRoles = await db.GetRepo<RoleEntity>().Get(role => role.is_default);

                    account.Roles.Remove(defaultRoles.FirstOrDefault(role => role.name == DefaultRoles.AUTHORIZED.ToName()));
                    account.Roles.Add(defaultRoles.FirstOrDefault(role => role.name == DefaultRoles.HIRED.ToName()));

                    await db.Save();

                    return await GetCompany(account.company_id.Value);
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

                    IList<RoleEntity> defaultRoles = await db.GetRepo<RoleEntity>().Get(role => role.is_default);
                    account.Roles.Add(defaultRoles.FirstOrDefault(role => role.name == DefaultRoles.AUTHORIZED.ToName()));

                    await db.Save();

                    return await GetCompany(account.company_id.Value);
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Company, CheckSameCompany = true)]
        public async Task<CompanyModel> GetCompany(AuthorizedDto<CompanyIdDto> model)
        {
            return await GetCompany(model.Data.CompanyId.Value);
        }

        public async Task<CompanyModel> GetCompany(int companyId)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CompanyEntity company = await db.GetRepo<CompanyEntity>().Get(companyId);
                    CompanyModel companyModel = company.ToModel<CompanyModel>();

                    companyModel.CheckPoints = company.Manufactories.SelectMany(m => m.CheckPoints)
                        .Distinct().Select(m => m.ToModel<CheckPointModel>()).ToList();

                    companyModel.EnterLeavePoints = company.Manufactories.SelectMany(m => m.EnterLeavePoints)
                        .Distinct().Select(m => m.ToModel<EnterLeavePointModel>()).ToList();

                    return companyModel;
                }
            });
        }

        private void ValidatePlan(AuthorizedDto<SetupPlanDto> plan)
        {
            DataValidationException dataValidationException = new DataValidationException();

            if (plan.Data.CompanyPlanPoints.Any(point => (point.Id.HasValue && point.FakeId.HasValue) || (!point.Id.HasValue && !point.FakeId.HasValue)))
            {
                dataValidationException.Add(
                    typeof(CompanyPlanPointDto), nameof(CompanyPlanPointDto.FakeId),
                    $"One of fields '{nameof(CompanyPlanPointDto.FakeId)}' and '{nameof(CompanyPlanPointDto.Id)}' must be specified"
                );
            }

            bool manufactoryPointsInvalid = plan.Data.Manufactories.Any(
                manufactory => manufactory.ManufactoryPlanPoints.Any(
                    point => (point.CompanyPlanPointId.HasValue && point.FakeCompanyPlanPointId.HasValue) ||
                             (!point.CompanyPlanPointId.HasValue && !point.FakeCompanyPlanPointId.HasValue)
                )
            );

            if (manufactoryPointsInvalid)
            {
                dataValidationException.Add(
                    typeof(ManufactoryPlanPointDto), nameof(ManufactoryPlanPointDto.FakeCompanyPlanPointId),
                    $"One of fields '{nameof(ManufactoryPlanPointDto.FakeCompanyPlanPointId)}' and '{nameof(ManufactoryPlanPointDto.CompanyPlanPointId)}' must be specified"
                );
            }

            bool checkPointsInvalid = plan.Data.CheckPoints.Any(
                checkPoint => (checkPoint.CompanyPlanUniquePoint1Id.HasValue && checkPoint.FakeCompanyPlanUniquePoint1Id.HasValue) ||
                              (!checkPoint.CompanyPlanUniquePoint1Id.HasValue && !checkPoint.FakeCompanyPlanUniquePoint1Id.HasValue) ||
                              (checkPoint.CompanyPlanUniquePoint2Id.HasValue && checkPoint.FakeCompanyPlanUniquePoint2Id.HasValue) ||
                              (!checkPoint.CompanyPlanUniquePoint2Id.HasValue && !checkPoint.FakeCompanyPlanUniquePoint2Id.HasValue)
            );

            if (checkPointsInvalid)
            {
                dataValidationException.Add(
                    typeof(CheckPointDto), nameof(CheckPointDto.FakeCompanyPlanUniquePoint1Id),
                    $"One of fields '{nameof(CheckPointDto.FakeCompanyPlanUniquePoint1Id)}' and '{nameof(CheckPointDto.CompanyPlanUniquePoint1Id)}' must be specified"
                );

                dataValidationException.Add(
                    typeof(CheckPointDto), nameof(CheckPointDto.FakeCompanyPlanUniquePoint2Id),
                    $"One of fields '{nameof(CheckPointDto.FakeCompanyPlanUniquePoint2Id)}' and '{nameof(CheckPointDto.CompanyPlanUniquePoint2Id)}' must be specified"
                );
            }

            bool enterLeavePointsInvalid = plan.Data.EnterLeavePoints.Any(
                enterLeavePoint => (enterLeavePoint.CompanyPlanUniquePoint1Id.HasValue && enterLeavePoint.FakeCompanyPlanUniquePoint1Id.HasValue) ||
                                  (!enterLeavePoint.CompanyPlanUniquePoint1Id.HasValue && !enterLeavePoint.FakeCompanyPlanUniquePoint1Id.HasValue) ||
                                  (enterLeavePoint.CompanyPlanUniquePoint2Id.HasValue && enterLeavePoint.FakeCompanyPlanUniquePoint2Id.HasValue) ||
                                  (!enterLeavePoint.CompanyPlanUniquePoint2Id.HasValue && !enterLeavePoint.FakeCompanyPlanUniquePoint2Id.HasValue)
            );

            if (enterLeavePointsInvalid)
            {
                dataValidationException.Add(
                    typeof(EnterLeavePointDto), nameof(EnterLeavePointDto.FakeCompanyPlanUniquePoint1Id),
                    $"One of fields '{nameof(EnterLeavePointDto.FakeCompanyPlanUniquePoint1Id)}' and '{nameof(EnterLeavePointDto.CompanyPlanUniquePoint1Id)}' must be specified"
                );

                dataValidationException.Add(
                    typeof(EnterLeavePointDto), nameof(EnterLeavePointDto.FakeCompanyPlanUniquePoint2Id),
                    $"One of fields '{nameof(EnterLeavePointDto.FakeCompanyPlanUniquePoint2Id)}' and '{nameof(EnterLeavePointDto.CompanyPlanUniquePoint2Id)}' must be specified"
                );
            }

            if (dataValidationException.InvalidFieldInfos.Count != 0)
                throw dataValidationException;

            int[] realCompanyPlanPointIds = plan.Data.CompanyPlanPoints.Where(point => point.Id.HasValue).Select(point => point.Id.Value).ToArray();
            int[] fakeCompanyPlanPointIds = plan.Data.CompanyPlanPoints.Where(point => point.FakeId.HasValue).Select(point => point.FakeId.Value).ToArray();

            foreach (ManufactoryDto manufactory in plan.Data.Manufactories)
            {
                int[] realManufactoryCompanyPlanPointIds = manufactory.ManufactoryPlanPoints.Where(point => point.CompanyPlanPointId.HasValue)
                    .Select(point => point.CompanyPlanPointId.Value).ToArray();

                int[] fakeManufactoryCompanyPlanPointIds = manufactory.ManufactoryPlanPoints.Where(point => point.FakeCompanyPlanPointId.HasValue)
                    .Select(point => point.FakeCompanyPlanPointId.Value).ToArray();

                if (realManufactoryCompanyPlanPointIds.Intersect(realCompanyPlanPointIds).Count() != realManufactoryCompanyPlanPointIds.Length)
                {
                    dataValidationException.Add(
                        typeof(ManufactoryPlanPointDto), nameof(ManufactoryPlanPointDto.CompanyPlanPointId),
                        "company_plan_point_id provided for manufactory is not presented in company_plan_points collection"
                    );
                }

                if (fakeManufactoryCompanyPlanPointIds.Intersect(fakeCompanyPlanPointIds).Count() != fakeManufactoryCompanyPlanPointIds.Length)
                {
                    dataValidationException.Add(
                        typeof(ManufactoryPlanPointDto), nameof(ManufactoryPlanPointDto.CompanyPlanPointId),
                        "fake_company_plan_point_id provided for manufactory is not presented in company_plan_points collection"
                    );
                }

                if (realManufactoryCompanyPlanPointIds.Distinct().Count() != realManufactoryCompanyPlanPointIds.Length)
                {
                    dataValidationException.Add(
                        typeof(ManufactoryDto), nameof(ManufactoryDto.ManufactoryPlanPoints),
                        "company_plan_point_id's for one manufactory must be unique"
                    );
                }

                if (fakeManufactoryCompanyPlanPointIds.Distinct().Count() != fakeManufactoryCompanyPlanPointIds.Length)
                {
                    dataValidationException.Add(
                        typeof(ManufactoryDto), nameof(ManufactoryDto.ManufactoryPlanPoints),
                        "fake_company_plan_point_id's for one manufactory must be unique"
                    );
                }

                if (manufactory.ManufactoryPlanPoints.Length < 3)
                {
                    dataValidationException.Add(
                        typeof(ManufactoryDto), nameof(ManufactoryDto.ManufactoryPlanPoints),
                        "at least 3 company_plan_poin_id's must be specified for a manufactory"
                    );
                }
            }

            int[] realCheckPointCompanyPlanPointIds = plan.Data.CheckPoints
                .Where(point => point.CompanyPlanUniquePoint1Id.HasValue).Select(point => point.CompanyPlanUniquePoint1Id.Value)
                .Union(plan.Data.CheckPoints.Where(point => point.CompanyPlanUniquePoint2Id.HasValue).Select(point => point.CompanyPlanUniquePoint2Id.Value))
                .ToArray();

            int[] fakeCheckPointCompanyPlanPointIds = plan.Data.CheckPoints
                .Where(point => point.FakeCompanyPlanUniquePoint1Id.HasValue).Select(point => point.FakeCompanyPlanUniquePoint1Id.Value)
                .Union(plan.Data.CheckPoints.Where(point => point.FakeCompanyPlanUniquePoint2Id.HasValue).Select(point => point.FakeCompanyPlanUniquePoint2Id.Value))
                .ToArray();

            if (realCheckPointCompanyPlanPointIds.Intersect(realCompanyPlanPointIds).Count() != realCheckPointCompanyPlanPointIds.Length)
            {
                dataValidationException.Add(
                    typeof(CheckPointDto), $"{nameof(CheckPointDto.CompanyPlanUniquePoint1Id)} or {nameof(CheckPointDto.CompanyPlanUniquePoint2Id)}",
                    "company_plan_point_id provided for check point is not presented in company_plan_points collection"
                );
            }

            if (fakeCheckPointCompanyPlanPointIds.Intersect(fakeCompanyPlanPointIds).Count() != fakeCheckPointCompanyPlanPointIds.Length)
            {
                dataValidationException.Add(
                    typeof(CheckPointDto), $"{nameof(CheckPointDto.FakeCompanyPlanUniquePoint1Id)} or {nameof(CheckPointDto.FakeCompanyPlanUniquePoint2Id)}",
                    "fake_company_plan_point_id provided for check point is not presented in company_plan_points collection"
                );
            }

            int[] realEnterLeavePointCompanyPlanPointIds = plan.Data.EnterLeavePoints
                .Where(point => point.CompanyPlanUniquePoint1Id.HasValue).Select(point => point.CompanyPlanUniquePoint1Id.Value)
                .Union(plan.Data.EnterLeavePoints.Where(point => point.CompanyPlanUniquePoint2Id.HasValue).Select(point => point.CompanyPlanUniquePoint2Id.Value))
                .ToArray();

            int[] fakeEnterLeavePointCompanyPlanPointIds = plan.Data.EnterLeavePoints
                .Where(point => point.FakeCompanyPlanUniquePoint1Id.HasValue).Select(point => point.FakeCompanyPlanUniquePoint1Id.Value)
                .Union(plan.Data.EnterLeavePoints.Where(point => point.FakeCompanyPlanUniquePoint2Id.HasValue).Select(point => point.FakeCompanyPlanUniquePoint2Id.Value))
                .ToArray();

            if (realEnterLeavePointCompanyPlanPointIds.Intersect(realCompanyPlanPointIds).Count() != realEnterLeavePointCompanyPlanPointIds.Length)
            {
                dataValidationException.Add(
                    typeof(EnterLeavePointDto), $"{nameof(EnterLeavePointDto.CompanyPlanUniquePoint1Id)} or {nameof(EnterLeavePointDto.CompanyPlanUniquePoint2Id)}",
                    "company_plan_point_id provided for enter/leave point is not presented in company_plan_points collection"
                );
            }

            if (fakeEnterLeavePointCompanyPlanPointIds.Intersect(fakeCompanyPlanPointIds).Count() != fakeEnterLeavePointCompanyPlanPointIds.Length)
            {
                dataValidationException.Add(
                    typeof(EnterLeavePointDto), $"{nameof(EnterLeavePointDto.FakeCompanyPlanUniquePoint1Id)} or {nameof(EnterLeavePointDto.FakeCompanyPlanUniquePoint2Id)}",
                    "fake_company_plan_point_id provided for enter/leave point is not presented in company_plan_points collection"
                );
            }

            if (dataValidationException.InvalidFieldInfos.Count != 0)
                throw dataValidationException;

            Dictionary<int, bool> realCompanyPlanPointUsed = realCompanyPlanPointIds.ToDictionary(id => id, id => false);
            Dictionary<int, bool> fakeCompanyPlanPointUsed = fakeCompanyPlanPointIds.ToDictionary(id => id, id => false);

            (bool isCheckPoint, int? real1, int? real2, int? fake1, int? fake2)[] companyPlanPointIds = plan.Data.CheckPoints.Select(
                checkPoint => (
                    true, checkPoint.CompanyPlanUniquePoint1Id, checkPoint.CompanyPlanUniquePoint2Id,
                    checkPoint.FakeCompanyPlanUniquePoint1Id, checkPoint.FakeCompanyPlanUniquePoint2Id
                )
            ).Concat(plan.Data.EnterLeavePoints.Select(
                enterLeavePoint => (
                    false, enterLeavePoint.CompanyPlanUniquePoint1Id, enterLeavePoint.CompanyPlanUniquePoint2Id,
                    enterLeavePoint.FakeCompanyPlanUniquePoint1Id, enterLeavePoint.FakeCompanyPlanUniquePoint2Id
                )
            )).ToArray();

            foreach ((bool isCheckPoint, int? real1, int? real2, int? fake1, int? fake2) ids in companyPlanPointIds)
            {
                bool isPoint1AlreadyUsed = false;
                bool isPoint2AlreadyUsed = false;

                if (ids.real1.HasValue)
                {
                    isPoint1AlreadyUsed = realCompanyPlanPointUsed[ids.real1.Value];
                    realCompanyPlanPointUsed[ids.real1.Value] = true;
                }
                else
                {
                    isPoint1AlreadyUsed = fakeCompanyPlanPointUsed[ids.fake1.Value];
                    fakeCompanyPlanPointUsed[ids.fake1.Value] = true;
                }

                if (ids.real2.HasValue)
                {
                    isPoint2AlreadyUsed = realCompanyPlanPointUsed[ids.real2.Value];
                    realCompanyPlanPointUsed[ids.real2.Value] = true;
                }
                else
                {
                    isPoint2AlreadyUsed = fakeCompanyPlanPointUsed[ids.fake2.Value];
                    fakeCompanyPlanPointUsed[ids.fake2.Value] = true;
                }

                if (isPoint1AlreadyUsed && isPoint2AlreadyUsed)
                {
                    if (ids.isCheckPoint)
                    {
                        dataValidationException.Add(
                            typeof(CheckPointDto), "",
                            "company_plan_point_id's specified check point are both already in use"
                        );
                    }
                    else
                    {
                        dataValidationException.Add(
                            typeof(EnterLeavePointDto), "",
                            "company_plan_point_id's specified enter/leave point are both already in use"
                        );
                    }
                }
            }

            if (dataValidationException.InvalidFieldInfos.Count != 0)
                throw dataValidationException;

            Dictionary<int, List<ManufactoryDto>> manufactoriesAtRealCompanyPoints = realCompanyPlanPointIds.ToDictionary(id => id, id => new List<ManufactoryDto>());
            Dictionary<int, List<ManufactoryDto>> manufactoriesAtFakeCompanyPoints = fakeCompanyPlanPointIds.ToDictionary(id => id, id => new List<ManufactoryDto>());

            foreach (ManufactoryDto manufactory in plan.Data.Manufactories)
            {
                foreach(ManufactoryPlanPointDto manufactoryPlanPoint in manufactory.ManufactoryPlanPoints)
                {
                    if (manufactoryPlanPoint.CompanyPlanPointId.HasValue)
                        manufactoriesAtRealCompanyPoints[manufactoryPlanPoint.CompanyPlanPointId.Value].Add(manufactory);
                    else
                        manufactoriesAtFakeCompanyPoints[manufactoryPlanPoint.FakeCompanyPlanPointId.Value].Add(manufactory);
                }
            }

            foreach(CheckPointDto checkPoint in plan.Data.CheckPoints)
            {
                List<ManufactoryDto> manufactoriesOfPoint1 = checkPoint.CompanyPlanUniquePoint1Id.HasValue ?
                    manufactoriesAtRealCompanyPoints[checkPoint.CompanyPlanUniquePoint1Id.Value] :
                    manufactoriesAtFakeCompanyPoints[checkPoint.FakeCompanyPlanUniquePoint1Id.Value];

                List<ManufactoryDto> manufactoriesOfPoint2 = checkPoint.CompanyPlanUniquePoint2Id.HasValue ?
                    manufactoriesAtRealCompanyPoints[checkPoint.CompanyPlanUniquePoint2Id.Value] :
                    manufactoriesAtFakeCompanyPoints[checkPoint.FakeCompanyPlanUniquePoint2Id.Value];

                if (manufactoriesOfPoint1.Intersect(manufactoriesOfPoint2).Count() != 2)
                {
                    dataValidationException.Add(
                        typeof(CheckPointDto), "",
                        "company_plan_point_id's and fake'company_plan_point_id's provided for check point must be associated with strictly 2 manufactories"
                    );
                }
            }

            foreach (EnterLeavePointDto enterLeavePoint in plan.Data.EnterLeavePoints)
            {
                List<ManufactoryDto> manufactoriesOfPoint1 = enterLeavePoint.CompanyPlanUniquePoint1Id.HasValue ?
                    manufactoriesAtRealCompanyPoints[enterLeavePoint.CompanyPlanUniquePoint1Id.Value] :
                    manufactoriesAtFakeCompanyPoints[enterLeavePoint.FakeCompanyPlanUniquePoint1Id.Value];

                List<ManufactoryDto> manufactoriesOfPoint2 = enterLeavePoint.CompanyPlanUniquePoint2Id.HasValue ?
                    manufactoriesAtRealCompanyPoints[enterLeavePoint.CompanyPlanUniquePoint2Id.Value] :
                    manufactoriesAtFakeCompanyPoints[enterLeavePoint.FakeCompanyPlanUniquePoint2Id.Value];

                if (manufactoriesOfPoint1.Intersect(manufactoriesOfPoint2).Count() != 1)
                {
                    dataValidationException.Add(
                        typeof(EnterLeavePointDto), "",
                        "company_plan_point_id's and fake'company_plan_point_id's provided for enter/leave point must be associated with strictly 1 manufactory"
                    );
                }
            }

            if (dataValidationException.InvalidFieldInfos.Count != 0)
                throw dataValidationException;
        }

        private async Task<IList<ManufactoryEntity>> GetCommonManufactories(
            CompanyPlanUniquePointEntity point1,
            CompanyPlanUniquePointEntity point2,
            IRepo<ManufactoryEntity> manufactoryRepo
        )
        {
            return await manufactoryRepo.Get(
                manufactory => manufactory.ManufactoryPlanPoints.Any(point => point.company_plan_unique_point_id == point1.id) &&
                               manufactory.ManufactoryPlanPoints.Any(point => point.company_plan_unique_point_id == point2.id)
            );
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Company, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.UPDATE | InteractionDbType.DELETE, Table = DbTable.Manufactory, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.UPDATE | InteractionDbType.DELETE, Table = DbTable.CompanyPlanUniquePoint, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.UPDATE | InteractionDbType.DELETE, Table = DbTable.ManufactoryPlanPoint, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.UPDATE | InteractionDbType.DELETE, Table = DbTable.CheckPoint, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.CREATE | InteractionDbType.UPDATE | InteractionDbType.DELETE, Table = DbTable.EnterLeavePoint, CheckSameCompany = true)]
        public async Task<CompanyModel> SetupPlan(AuthorizedDto<SetupPlanDto> plan)
        {
            return await Execute(async () => {
                ValidatePlan(plan);

                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<CompanyEntity> companyRepo = db.GetRepo<CompanyEntity>();
                    IRepo<ManufactoryEntity> manufactoryRepo = db.GetRepo<ManufactoryEntity>();
                    IRepo<CompanyPlanUniquePointEntity> companyPlanPointRepo = db.GetRepo<CompanyPlanUniquePointEntity>();
                    IRepo<ManufactoryPlanPointEntity> manufactoryPlanPointRepo = db.GetRepo<ManufactoryPlanPointEntity>();
                    IRepo<CheckPointEntity> checkPointRepo = db.GetRepo<CheckPointEntity>();
                    IRepo<EnterLeavePointEntity> enterLeavePointRepo = db.GetRepo<EnterLeavePointEntity>();

                    CompanyEntity company = await companyRepo.FirstOrDefault(c => c.owner_id == plan.Data.CompanyId.Value);

                    Dictionary<int, CompanyPlanUniquePointEntity> existingCompanyPlanPoints = new Dictionary<int, CompanyPlanUniquePointEntity>();
                    Dictionary<int, CompanyPlanUniquePointEntity> fakeCompanyPlanPoints = new Dictionary<int, CompanyPlanUniquePointEntity>();

                    foreach (CompanyPlanPointDto companyPlanPoint in plan.Data.CompanyPlanPoints)
                    {
                        if (companyPlanPoint.Id.HasValue)
                        {
                            CompanyPlanUniquePointEntity existingPoint = await companyPlanPointRepo.Get(companyPlanPoint.Id.Value);

                            existingPoint.x = companyPlanPoint.X;
                            existingPoint.y = companyPlanPoint.Y;

                            existingCompanyPlanPoints.Add(companyPlanPoint.Id.Value, existingPoint);
                        }
                        else
                        {
                            fakeCompanyPlanPoints.Add(
                                companyPlanPoint.FakeId.Value,
                                new CompanyPlanUniquePointEntity()
                                {
                                    company_id = plan.Data.CompanyId.Value,
                                    x = companyPlanPoint.X,
                                    y = companyPlanPoint.Y
                                }
                            );
                        }
                    }

                    List<CompanyPlanUniquePointEntity> currentCompanyPlanPoints = existingCompanyPlanPoints.Values.Union(fakeCompanyPlanPoints.Values).ToList();
                    List<CompanyPlanUniquePointEntity> companyPlanPointsToRemove = company.CompanyPlanUniquePoints.Except(currentCompanyPlanPoints).ToList();

                    company.CompanyPlanUniquePoints = currentCompanyPlanPoints.Union(companyPlanPointsToRemove).ToList();

                    Dictionary<ManufactoryDto, ManufactoryEntity> manufactories = new Dictionary<ManufactoryDto, ManufactoryEntity>();

                    foreach (ManufactoryDto manufactory in plan.Data.Manufactories)
                    {
                        if (manufactory.Id.HasValue)
                            manufactories.Add(manufactory, await manufactoryRepo.Get(manufactory.Id.Value));
                        else
                            manufactories.Add(manufactory, new ManufactoryEntity() { company_id = plan.Data.CompanyId.Value });
                    }

                    List<ManufactoryEntity> currentManufactories = manufactories.Values.ToList();
                    List<ManufactoryEntity> manufactoriesToRemove = company.Manufactories.Except(currentManufactories).ToList();

                    company.Manufactories = currentManufactories.Union(manufactoriesToRemove).ToList();
                    await db.Save();

                    foreach (ManufactoryDto manufactory in plan.Data.Manufactories)
                    {
                        List<ManufactoryPlanPointEntity> currentManufactoryPlanPoints = new List<ManufactoryPlanPointEntity>();
                        foreach (ManufactoryPlanPointDto manufactoryPlanPoint in manufactory.ManufactoryPlanPoints)
                        {
                            ManufactoryPlanPointEntity manufactoryPlanPointEntity = manufactoryPlanPoint.Id.HasValue ?
                                await manufactoryPlanPointRepo.Get(manufactoryPlanPoint.Id.Value) : new ManufactoryPlanPointEntity();

                            manufactoryPlanPointEntity.manufactory_id = manufactories[manufactory].id;
                            manufactoryPlanPointEntity.company_plan_unique_point_id = manufactoryPlanPoint.CompanyPlanPointId ??
                                fakeCompanyPlanPoints[manufactoryPlanPoint.FakeCompanyPlanPointId.Value].id;

                            currentManufactoryPlanPoints.Add(manufactoryPlanPointEntity);
                        }

                        await manufactoryPlanPointRepo.Delete(
                            manufactories[manufactory].ManufactoryPlanPoints.Except(
                                currentManufactoryPlanPoints
                            ).Select(point => point.id).ToArray()
                        );

                        manufactories[manufactory].ManufactoryPlanPoints = currentManufactoryPlanPoints;
                    }
                    await db.Save();

                    List<CheckPointEntity> currentCheckPoints = new List<CheckPointEntity>();
                    foreach (CheckPointDto checkPoint in plan.Data.CheckPoints)
                    {
                        CheckPointEntity checkPointEntity = checkPoint.Id.HasValue ?
                            await checkPointRepo.Get(checkPoint.Id.Value) : new CheckPointEntity();

                        checkPointEntity.CompanyPlanUniquePoint1 = checkPoint.CompanyPlanUniquePoint1Id.HasValue ?
                            existingCompanyPlanPoints[checkPoint.CompanyPlanUniquePoint1Id.Value] :
                            fakeCompanyPlanPoints[checkPoint.FakeCompanyPlanUniquePoint1Id.Value];

                        checkPointEntity.CompanyPlanUniquePoint2 = checkPoint.CompanyPlanUniquePoint2Id.HasValue ?
                            existingCompanyPlanPoints[checkPoint.CompanyPlanUniquePoint2Id.Value] :
                            fakeCompanyPlanPoints[checkPoint.FakeCompanyPlanUniquePoint2Id.Value];

                        IList<ManufactoryEntity> commonManufactories = await GetCommonManufactories(
                            checkPointEntity.CompanyPlanUniquePoint1, checkPointEntity.CompanyPlanUniquePoint2, 
                            manufactoryRepo
                        );

                        checkPointEntity.Manufactory1Id = commonManufactories.Except(manufactoriesToRemove).ElementAt(0).id;
                        checkPointEntity.Manufactory2Id = commonManufactories.Except(manufactoriesToRemove).ElementAt(1).id;

                        if (!checkPoint.Id.HasValue)
                            await checkPointRepo.Create(checkPointEntity);

                        currentCheckPoints.Add(checkPointEntity);
                    }

                    List<EnterLeavePointEntity> currentEnterLeavePoints = new List<EnterLeavePointEntity>();
                    foreach (EnterLeavePointDto enterLeavePoint in plan.Data.EnterLeavePoints)
                    {
                        EnterLeavePointEntity enterLeavePointEntity = enterLeavePoint.Id.HasValue ?
                            await enterLeavePointRepo.Get(enterLeavePoint.Id.Value) : new EnterLeavePointEntity();

                        enterLeavePointEntity.CompanyPlanUniquePoint1 = enterLeavePoint.CompanyPlanUniquePoint1Id.HasValue ?
                            existingCompanyPlanPoints[enterLeavePoint.CompanyPlanUniquePoint1Id.Value] :
                            fakeCompanyPlanPoints[enterLeavePoint.FakeCompanyPlanUniquePoint1Id.Value];

                        enterLeavePointEntity.CompanyPlanUniquePoint2 = enterLeavePoint.CompanyPlanUniquePoint2Id.HasValue ?
                            existingCompanyPlanPoints[enterLeavePoint.CompanyPlanUniquePoint2Id.Value] :
                            fakeCompanyPlanPoints[enterLeavePoint.FakeCompanyPlanUniquePoint2Id.Value];

                        IList<ManufactoryEntity> commonManufactories = await GetCommonManufactories(
                            enterLeavePointEntity.CompanyPlanUniquePoint1, enterLeavePointEntity.CompanyPlanUniquePoint2,
                            manufactoryRepo
                        );

                        enterLeavePointEntity.manufactory_id = commonManufactories.Except(manufactoriesToRemove).ElementAt(0).id;

                        if (!enterLeavePoint.Id.HasValue)
                            await enterLeavePointRepo.Create(enterLeavePointEntity);

                        currentEnterLeavePoints.Add(enterLeavePointEntity);
                    }
                    await db.Save();

                    IList<CheckPointEntity> allCheckPoints = await checkPointRepo.Get(
                        checkPoint => checkPoint.Manufactory1.company_id == company.owner_id
                    );
                    await checkPointRepo.Delete(allCheckPoints.Except(currentCheckPoints).Select(point => point.id).ToArray());

                    IList<EnterLeavePointEntity> allEnterLeavePoints = await enterLeavePointRepo.Get(
                        enterLeavePoint => enterLeavePoint.Manufactory.company_id == company.owner_id
                    );
                    await enterLeavePointRepo.Delete(allEnterLeavePoints.Except(currentEnterLeavePoints).Select(point => point.id).ToArray());

                    await manufactoryPlanPointRepo.Delete(
                        manufactoriesToRemove.SelectMany(
                            manufactory => manufactory.ManufactoryPlanPoints
                        ).Select(point => point.id).ToArray()
                    );

                    await manufactoryRepo.Delete(manufactoriesToRemove.Select(manufactory => manufactory.id).ToArray());
                    await companyPlanPointRepo.Delete(companyPlanPointsToRemove.Select(companyPlanPoint => companyPlanPoint.id).ToArray());

                    await db.Save();

                    return await GetCompany(company.owner_id);
                }
            });
        }
    }
}
