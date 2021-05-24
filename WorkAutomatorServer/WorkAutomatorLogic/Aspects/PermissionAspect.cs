using MethodBoundaryAspect.Fody.Attributes;
using System;
using WorkAutomatorLogic.Models.Permission;

namespace WorkAutomatorLogic.Aspects
{
    public class PermissionAspect : OnMethodBoundaryAspect
    {
        public InteractionType Type { get; set; }
        public string TargetIdDtoPath { get; set; }
        public string CompanyIdPath { get; set; }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            base.OnEntry(arg);
        }
    }
}