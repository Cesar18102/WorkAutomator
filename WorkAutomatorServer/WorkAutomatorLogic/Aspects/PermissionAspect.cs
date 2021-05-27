using MethodBoundaryAspect.Fody.Attributes;

using Constants;

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