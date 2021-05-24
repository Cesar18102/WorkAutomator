using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Autofac;

using MethodBoundaryAspect.Fody.Attributes;

using WorkAutomatorLogic.Models.Permission;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Aspects
{
    public class DbPermissionAspect : OnMethodBoundaryAspect
    {
        private static IPermissionService PermissionService = LogicDependencyHolder.Dependencies.Resolve<IPermissionService>();

        public DbTable Table { get; set; } = DbTable.None;
        public InteractionDbType Action { get; set; }
        public Type DbTableConverterType { get; set; }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            ParameterInfo[] methodParameters = arg.Method.GetParameters();

            ParameterInfo initiatorParameter = methodParameters.First(
                param => param.GetCustomAttribute<InitiatorAccountIdAttribute>() != null
            );

            int initiatorAccountId = (int)arg.Arguments[initiatorParameter.Position];

            if(Table == DbTable.None)
            {
                ParameterInfo tableNameParameter = methodParameters.First(
                    param => param.GetCustomAttribute<TableNameParameterAttribute>() != null
                );

                object tableNameParameterValue = arg.Arguments[tableNameParameter.Position];

                if (DbTableConverterType != null)
                {
                    IValueConverter converter = (IValueConverter)Activator.CreateInstance(DbTableConverterType);
                    Table = (DbTable)converter.Convert(tableNameParameterValue);
                }
                else
                    Table = (DbTable)tableNameParameterValue;
            }

            Interaction interaction = new Interaction(Action, Table, initiatorAccountId);
            Task.Run(() => PermissionService.CheckLegal(interaction)).GetAwaiter().GetResult();
        }
    }
}