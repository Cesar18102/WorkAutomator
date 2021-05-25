using MethodBoundaryAspect.Fody.Attributes;

using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic.Aspects
{
    public class PermissionAspect : OnMethodBoundaryAspect
    {
        public InteractionType Type { get; set; }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            //TODO
            //
        }
    }
}