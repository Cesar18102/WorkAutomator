using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using AutoMapper;

using WorkAutomatorDataAccess.Exceptions;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Models;

namespace WorkAutomatorLogic.Services
{
    public class ServiceBase
    {
        public async Task Execute<TIn>(Action<TIn> executor, TIn parameter)
        {
            Func<object> func = new Func<object>(() =>
            {
                executor(parameter);
                return null;
            });

            Func<Task<object>> task = new Func<Task<object>>(
                () => Task.Run(func)
            );

            await ProtectedExecute(task);
        }

        public async Task Execute(Func<Task> executor)
        {
            Func<Task<object>> task = new Func<Task<object>>(
                async () =>
                {
                    await executor();
                    return null;
                }
            );

            await ProtectedExecute(task);
        }

        public async Task Execute<TIn>(Func<TIn, Task> executor, TIn parameter)
        {
            Func<Task<object>> task = new Func<Task<object>>(
                async () =>
                {
                    await executor(parameter);
                    return null;
                }
            );

            await ProtectedExecute(task);
        }

        public async Task<TOut> Execute<TOut>(Func<TOut> executor)
        {
            return await ProtectedExecute(() => Task.Run(executor));
        }

        public async Task<TOut> Execute<TOut>(Func<Task<TOut>> executor)
        {
            return await ProtectedExecute(executor);
        }

        public async Task<TOut> Execute<TIn, TOut>(Func<TIn, TOut> executor, TIn parameter)
        {
            Func<Task<TOut>> task = new Func<Task<TOut>>(
                () => Task.Run(
                    () => executor(parameter)
                )
            );

            return await ProtectedExecute(task);
        }

        public async Task<TOut> Execute<TIn, TOut>(Func<TIn, Task<TOut>> executor, TIn parameter)
        {
            return await ProtectedExecute(() => executor(parameter));
        }

        private async Task<Tout> ProtectedExecute<Tout>(Func<Task<Tout>> task)
        {
            try { return await task(); }
            catch (DatabaseActionValidationException ex)
            {
                DataValidationException dataValidationException = new DataValidationException();

                PropertyInfo[] properties = ex.EntityType.GetProperties();
                foreach (ValidationResult validationResult in ex.Errors)
                {
                    TypeMap modelToEntityMap = ModelEntityMapper.Mapper.ConfigurationProvider.GetAllTypeMaps().First(
                        map => typeof(ModelBase).IsAssignableFrom(map.SourceType) && map.DestinationType.Equals(ex.EntityType)
                    );

                    if (validationResult.MemberNames.Count() == 0)
                    {
                        InvalidFieldInfo info = new InvalidFieldInfo(modelToEntityMap.SourceType, null, validationResult.ErrorMessage);
                        dataValidationException.InvalidFieldInfos.Add(info);
                    }

                    foreach (string invalidFieldName in validationResult.MemberNames)
                    {
                        string realPropertyName = properties.FirstOrDefault(
                            property => property.GetCustomAttribute<ColumnAttribute>()?.Name == invalidFieldName
                        )?.Name ?? invalidFieldName;

                        PropertyMap foundPropertyMap = modelToEntityMap.PropertyMaps.First(
                            map => map.DestinationMember.Name == realPropertyName
                        );

                        InvalidFieldInfo info = new InvalidFieldInfo(
                            modelToEntityMap.SourceType, 
                            foundPropertyMap.SourceMember.Name,
                            validationResult.ErrorMessage
                        );

                        dataValidationException.InvalidFieldInfos.Add(info);
                    }
                }

                throw dataValidationException;
            }
            catch (EntityNotFoundException ex)
            {
                throw new NotFoundException(ex.NotFoundSubject);
            }
        }
    }
}
