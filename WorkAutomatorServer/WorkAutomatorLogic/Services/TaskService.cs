using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Constants;

using Dto;
using Dto.Tasks;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;

using WorkAutomatorLogic.Aspects;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Models;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Services
{
    internal class TaskService : ServiceBase, ITaskService
    {
        [DbPermissionAspect(Action = InteractionDbType.CREATE, Table = DbTable.Task)]
        public async Task<TaskModel> Create(AuthorizedDto<TaskDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    TaskEntity task = dto.Data.ToModel<TaskModel>().ToEntity<TaskEntity>();
                    task.creator_account_id = dto.Session.UserId;

                    await db.GetRepo<TaskEntity>().Create(task);
                    await db.Save();

                    return task.ToModel<TaskModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.Task, CheckSameCompany = true)]
        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Account, CheckSameCompany = true)]
        public async Task<TaskModel> Assign(AuthorizedDto<AssignTaskDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IRepo<AccountEntity> accountRepo = db.GetRepo<AccountEntity>();

                    TaskEntity task = await db.GetRepo<TaskEntity>().Get(dto.Data.Id.Value);

                    if (task.creator_account_id != dto.Session.UserId)
                        throw new NotPermittedException($"Creator of task #{task.id}");

                    AccountEntity initiator = await accountRepo.Get(dto.Session.UserId);
                    AccountEntity assignee = dto.Data.AssigneeAccountId.HasValue ? await accountRepo.Get(dto.Data.AssigneeAccountId.Value) : null;
                    AccountEntity reviewer = dto.Data.ReviewerAccountId.HasValue ? await accountRepo.Get(dto.Data.ReviewerAccountId.Value) : null;

                    if(assignee != null && !assignee.Bosses.Contains(initiator) && assignee != initiator)
                        throw new NotPermittedException($"Boss for assignee Account #{assignee.id}");

                    if (reviewer != null && !reviewer.Bosses.Contains(initiator) && reviewer != initiator)
                        throw new NotPermittedException($"Boss for reviewer Account #{reviewer.id}");

                    task.assignee_account_id = assignee?.id;
                    task.reviewer_account_id = reviewer?.id;

                    await db.Save();

                    return task.ToModel<TaskModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.Task, CheckSameCompany = true)]
        public async Task<TaskModel> NotifyDone(AuthorizedDto<TaskDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    TaskEntity task = await db.GetRepo<TaskEntity>().Get(dto.Data.Id.Value);

                    if (task.assignee_account_id != dto.Session.UserId)
                        throw new NotPermittedException($"Assignee of task #{task.id}");

                    task.is_done = true;

                    await db.Save();
                    return task.ToModel<TaskModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ | InteractionDbType.UPDATE, Table = DbTable.Task, CheckSameCompany = true)]
        public async Task<TaskModel> NotifyReviewed(AuthorizedDto<ReviewTaskDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    TaskEntity task = await db.GetRepo<TaskEntity>().Get(dto.Data.Id.Value);

                    if (task.reviewer_account_id != dto.Session.UserId)
                        throw new NotPermittedException($"Reviewer of task #{task.id}");

                    if (dto.Data.ReviewResult.Value)
                        task.is_reviewed = true;
                    else
                        task.is_done = false;

                    await db.Save();
                    return task.ToModel<TaskModel>();
                }
            });
        }

        [DbPermissionAspect(Action = InteractionDbType.READ, Table = DbTable.Task)]
        public async Task<TaskModel[]> GetMyTasks(AuthorizedDto<IdDto> dto)
        {
            return await Execute(async () => {
                using (UnitOfWork db = new UnitOfWork())
                {
                    IList<TaskEntity> tasks = await db.GetRepo<TaskEntity>().Get(
                        task => task.assignee_account_id == dto.Session.UserId || 
                                task.reviewer_account_id == dto.Session.UserId ||
                                task.creator_account_id == dto.Session.UserId
                    );

                    return ModelEntityMapper.Mapper.Map<IList<TaskModel>>(tasks).ToArray();
                }
            });
        }
    }
}
