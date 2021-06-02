using System;
using System.Linq;
using System.Threading.Tasks;

using Constants;
using Dto.Interaction;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;
using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class LocationService : ServiceBase, ILocationService
    {
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Manufactory, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.CheckPoint, CheckSameCompany = true)]
        public async Task TryCheckout(CheckoutDto dto)
        {
            await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CheckPointEntity checkPoint = await db.GetRepo<CheckPointEntity>().Get(dto.CheckPointId.Value);
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(dto.AccountId.Value);

                    if (dto.CurrentManufactoryId.Value != checkPoint.Manufactory1Id && dto.CurrentManufactoryId.Value != checkPoint.Manufactory2Id)
                    {
                        DataValidationException exception = new DataValidationException();

                        exception.Add(
                            typeof(CheckoutDto), nameof(CheckoutDto.CurrentManufactoryId),
                            $"{nameof(CheckoutDto.CurrentManufactoryId)} must be one of manufactories associated with check point"
                        );

                        throw exception;
                    }

                    CheckPointEventEntity checkPointEvent = new CheckPointEventEntity()
                    {
                        account_id = account.id,
                        check_point_id = checkPoint.id,
                        is_direct = checkPoint.Manufactory1Id == dto.CurrentManufactoryId.Value,
                        timespan = DateTime.Now
                    };

                    ManufactoryEntity targetManufactory = checkPointEvent.is_direct ? checkPoint.Manufactory2 : checkPoint.Manufactory1;
                    NotPermittedException ex = null;

                    if (account.Roles.SelectMany(r => r.ManufactoryPermissions).Any(m => m.id == targetManufactory.id))
                        checkPointEvent.log = $"Checkout to Manufactory #{targetManufactory.id} via Check Point ${checkPoint.id} by Account #{account.id}: SUCCESS";
                    else
                    {
                        checkPointEvent.log = $"Checkout to Manufactory #{targetManufactory.id} via Check Point ${checkPoint.id} by Account #{account.id}: ACCESS DENIED";
                        ex = new NotPermittedException(checkPointEvent.log);
                    }

                    await db.GetRepo<CheckPointEventEntity>().Create(checkPointEvent);
                    await db.Save();

                    if (ex != null)
                        throw ex;
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.EnterLeavePoint, CheckSameCompany = true)]
        public async Task TryEnter(EnterLeaveDto dto)
        {
            await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    EnterLeavePointEntity enterLeavePoint = await db.GetRepo<EnterLeavePointEntity>().Get(dto.EnterLeavePointId.Value);
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(dto.AccountId.Value);

                    EnterLeavePointEventEntity enterLeavePointEvent = new EnterLeavePointEventEntity()
                    {
                        account_id = account.id,
                        enter_leave_point_id = enterLeavePoint.id,
                        is_enter = true,
                        timespan = DateTime.Now
                    };

                    NotPermittedException ex = null;

                    if (account.Roles.SelectMany(r => r.ManufactoryPermissions).Any(m => m.id == enterLeavePoint.Manufactory.id))
                        enterLeavePointEvent.log = $"Enter to Manufactory #{enterLeavePoint.Manufactory.id} via Enter Point ${enterLeavePoint.id} by Account #{account.id}: SUCCESS";
                    else
                    {
                        enterLeavePointEvent.log = $"Enter to Manufactory #{enterLeavePoint.Manufactory.id} via Enter Point ${enterLeavePoint.id} by Account #{account.id}: ACCESS DENIED";
                        ex = new NotPermittedException(enterLeavePointEvent.log);
                    }

                    await db.GetRepo<EnterLeavePointEventEntity>().Create(enterLeavePointEvent);
                    await db.Save();

                    if (ex != null)
                        throw ex;
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.EnterLeavePoint, CheckSameCompany = true)]
        public async Task TryLeave(EnterLeaveDto dto)
        {
            await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    EnterLeavePointEntity enterLeavePoint = await db.GetRepo<EnterLeavePointEntity>().Get(dto.EnterLeavePointId.Value);
                    AccountEntity account = await db.GetRepo<AccountEntity>().Get(dto.AccountId.Value);

                    EnterLeavePointEventEntity enterLeavePointEvent = new EnterLeavePointEventEntity()
                    {
                        account_id = account.id,
                        enter_leave_point_id = enterLeavePoint.id,
                        is_enter = false,
                        timespan = DateTime.Now
                    };

                    NotPermittedException ex = null;

                    if (account.Roles.SelectMany(r => r.ManufactoryPermissions).Any(m => m.id == enterLeavePoint.Manufactory.id))
                        enterLeavePointEvent.log = $"Leave from Manufactory #{enterLeavePoint.Manufactory.id} via Leave Point ${enterLeavePoint.id} by Account #{account.id}: SUCCESS";
                    else
                    {
                        enterLeavePointEvent.log = $"Leave from Manufactory #{enterLeavePoint.Manufactory.id} via Leave Point ${enterLeavePoint.id} by Account #{account.id}: ACCESS DENIED";
                        ex = new NotPermittedException(enterLeavePointEvent.log);
                    }

                    await db.GetRepo<EnterLeavePointEventEntity>().Create(enterLeavePointEvent);
                    await db.Save();

                    if (ex != null)
                        throw ex;
                }
            });
        }
    }
}
