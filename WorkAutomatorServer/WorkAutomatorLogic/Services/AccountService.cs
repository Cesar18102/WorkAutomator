using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Constants;
using Dto;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class AccountService : ServiceBase, IAccountService
    {
        [DbPermissionAspect(Action = InteractionDbType.UPDATE, Table = DbTable.Account)]
        public async Task<AccountModel> UpdateProfile(AuthorizedDto<AccountDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (dto.Data.Id.Value != dto.Session.UserId)
                        throw new NotPermittedException($"UPDATE Account #{dto.Session.UserId}");

                    AccountEntity account = await db.GetRepo<AccountEntity>().FirstOrDefault(acc => acc.id == dto.Data.Id.Value);

                    account.first_name = dto.Data.FirstName;
                    account.last_name = dto.Data.LastName;

                    await db.Save();

                    return account.ToModel<AccountModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Account, CheckSameCompany = true)]
        public async Task<WorkerModel> AddBoss(AuthorizedDto<SetRemoveBossDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<AccountEntity> accountRepo = db.GetRepo<AccountEntity>();

                    AccountEntity initiator = await accountRepo.Get(dto.Session.UserId);
                    AccountEntity sub = await accountRepo.Get(dto.Data.SubAccountId.Value);
                    AccountEntity boss = await accountRepo.Get(dto.Data.BossAccountId.Value);

                    if (!sub.Bosses.Contains(initiator) || !boss.Bosses.Contains(initiator))
                        throw new NotPermittedException($"Boss for Account #{sub.id} AND #{boss.id}");

                    if (sub.Bosses.Contains(boss))
                        return sub.ToModel<WorkerModel>();

                    if (boss.Bosses.Contains(sub)) //if the relation was directly another
                    {
                        boss.Bosses.Remove(sub);
                        sub.Subs.Remove(boss);
                    }

                    sub.Bosses.Add(boss);
                    boss.Subs.Add(sub);

                    await db.Save();

                    return sub.ToModel<WorkerModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Account, CheckSameCompany = true)]
        public async Task<WorkerModel> RemoveBoss(AuthorizedDto<SetRemoveBossDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<AccountEntity> accountRepo = db.GetRepo<AccountEntity>();

                    AccountEntity initiator = await accountRepo.Get(dto.Session.UserId);
                    AccountEntity sub = await accountRepo.Get(dto.Data.SubAccountId.Value);
                    AccountEntity boss = await accountRepo.Get(dto.Data.BossAccountId.Value);

                    if (!boss.Bosses.Contains(initiator))
                        throw new NotPermittedException($"Boss for Account #{boss.id}");

                    boss.Subs.Remove(sub);
                    sub.Bosses.Remove(boss);

                    await db.Save();

                    return sub.ToModel<WorkerModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Account)]
        public async Task<WorkerModel> Get(AuthorizedDto<AccountDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    AccountEntity account = await db.GetRepo<AccountEntity>().FirstOrDefault(acc => acc.id == dto.Data.Id.Value);
                    return account.ToModel<WorkerModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Account)]
        public async Task<AccountModel[]> GetFreeAccounts(AuthorizedDto<IdDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IList<AccountEntity> freeAccounts = await db.GetRepo<AccountEntity>().Get(acc => acc.company_id == null);
                    return ModelEntityMapper.Mapper.Map<IList<AccountModel>>(freeAccounts).ToArray();
                }
            });
        }
    }
}
