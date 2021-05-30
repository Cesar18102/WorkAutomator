using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using MethodBoundaryAspect.Fody.Attributes;

using Constants;
using Attributes;

using WorkAutomatorLogic.Models.Permission;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorLogic.Aspects
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DbPermissionAspect : OnMethodBoundaryAspect
    {
        private static IPermissionService PermissionService = LogicDependencyHolder.Dependencies.Resolve<IPermissionService>();

        public DbTable Table { get; set; } = DbTable.None;
        public InteractionDbType Action { get; set; }
        public Type DbTableConverterType { get; set; }
        public bool CheckSameCompany { get; set; }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            ParameterInfo[] methodParameters = arg.Method.GetParameters();

            int initiatorAccountId = (int)arg.Arguments.GetMarkedValueFromArgumentList<InitiatorAccountIdAttribute>(methodParameters).First();

            if (Table == DbTable.None)
            {
                object tableNameParameterValue = arg.Arguments.GetMarkedValueFromArgumentList<TableNameParameterAttribute>(methodParameters).FirstOrDefault();

                if (DbTableConverterType != null)
                {
                    IValueConverter converter = (IValueConverter)Activator.CreateInstance(DbTableConverterType);
                    Table = (DbTable)converter.Convert(tableNameParameterValue);
                }
                else if (tableNameParameterValue != null)
                    Table = (DbTable)tableNameParameterValue;
            }

            int? companyId = (int?)arg.Arguments.GetMarkedValueFromArgumentList<CompanyIdAttribute>(methodParameters).FirstOrDefault();

            Dictionary<(Guid, ObjectIdAttribute), object[]> objectsByTables = arg.Arguments.GetMarkedMapFromArgumentList<ObjectIdAttribute>(
                methodParameters
            );

            int[] idsToCheck = objectsByTables.Where(o => o.Key.Item2.Table == Table)
                .SelectMany(o => o.Value).OfType<int>().ToArray();

            Interaction interaction = new Interaction(Action, Table, initiatorAccountId);

            interaction.CheckSameCompany = this.CheckSameCompany;
            interaction.CompanyId = companyId;
            interaction.ObjectIds = idsToCheck;

            Task.Run(() => PermissionService.CheckLegal(interaction)).GetAwaiter().GetResult();
        }
    }
}