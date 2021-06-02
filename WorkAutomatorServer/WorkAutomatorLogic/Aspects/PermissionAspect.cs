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
    public class PermissionAspect : OnMethodBoundaryAspect
    {
        private static IPermissionService PermissionService = LogicDependencyHolder.Dependencies.Resolve<IPermissionService>();

        public InteractionType Type { get; set; }

        private Dictionary<InteractionType, DbTable[]> AssociatedTables = new Dictionary<InteractionType, DbTable[]>()
        {
            { InteractionType.DETECTOR, new DbTable[] { DbTable.Detector } },
            { InteractionType.PIPELINE_ITEM, new DbTable[] { DbTable.PipelineItem } },
            { InteractionType.MANUFACTORY, new DbTable[] { DbTable.Manufactory } },
            { InteractionType.STORAGE, new DbTable[] { DbTable.StorageCell } }
        };

        public override void OnEntry(MethodExecutionArgs arg)
        {
            ParameterInfo[] methodParameters = arg.Method.GetParameters();

            int initiatorAccountId = (int)arg.Arguments.GetMarkedValueFromArgumentList<InitiatorAccountIdAttribute>(methodParameters).First();

            Dictionary<(Guid, ObjectIdAttribute), object[]> objectsByTables = arg.Arguments.GetMarkedMapFromArgumentList<ObjectIdAttribute>(
                methodParameters
            );

            int[] idsToCheck = objectsByTables.Where(o => AssociatedTables[Type].Contains(o.Key.Item2.Table))
                .SelectMany(o => o.Value).OfType<int>().ToArray();

            Interaction interaction = new Interaction(Type, initiatorAccountId);
            interaction.ObjectIds = idsToCheck;

            Task.Run(() => PermissionService.CheckLegal(interaction)).GetAwaiter().GetResult();
        }
    }
}