using Constants;

namespace WorkAutomatorLogic.Models.Permission
{
    public abstract class PermissionModelBase : IdModel
    {
        public InteractionType InteractionType { get; private set; }

        public PermissionModelBase(InteractionType interactionType)
        {
            InteractionType = interactionType;
        }
    }
}
