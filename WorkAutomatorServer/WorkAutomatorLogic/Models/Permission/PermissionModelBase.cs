namespace WorkAutomatorLogic.Models.Permission
{
    public abstract class PermissionModelBase : ModelBase
    {
        public InteractionType InteractionType { get; private set; }

        public PermissionModelBase(InteractionType interactionType)
        {
            InteractionType = interactionType;
        }
    }
}
