using System;

using MethodBoundaryAspect.Fody.Attributes;

using WorkAutomatorDataAccess.Exceptions;
using WorkAutomatorDataAccess.Repos;

namespace WorkAutomatorDataAccess.Aspects
{
    public class AddRepoInfoToDatabaseActionValidationExceptionAspect : OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            Type genericRepoType = args.Instance.GetType();
            while (!genericRepoType.IsGenericType)
                genericRepoType = genericRepoType.BaseType;

            if (!typeof(RepoBase<>).IsAssignableFrom(genericRepoType.GetGenericTypeDefinition()))
                throw new InvalidOperationException();

            if (args.Exception is DatabaseActionValidationException ex)
            {
                ex.RepoType = genericRepoType;
                ex.EntityType = genericRepoType.GetGenericArguments()[0];
            }
            
            args.FlowBehavior = FlowBehavior.RethrowException;
        }
    }
}
