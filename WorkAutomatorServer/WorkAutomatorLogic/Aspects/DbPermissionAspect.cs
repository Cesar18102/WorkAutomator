using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Autofac;

using MethodBoundaryAspect.Fody.Attributes;
using WorkAutomatorLogic.Exceptions;
using WorkAutomatorLogic.Extensions;
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

            int initiatorAccountId = (int)arg.Arguments.GetMarkedValueFromArgumentList<InitiatorAccountIdAttribute>(methodParameters);

            if(Table == DbTable.None)
            {
                object tableNameParameterValue = arg.Arguments.GetMarkedValueFromArgumentList<TableNameParameterAttribute>(methodParameters);

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