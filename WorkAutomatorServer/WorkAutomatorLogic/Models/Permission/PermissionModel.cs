using System;

namespace WorkAutomatorLogic.Models.Permission
{
    public class PermissionModel : PermissionModelBase
    {
        public int InteractionTargetId { get; private set; }

        public PermissionModel(InteractionType interactionType, int interactionTargetId) : base(interactionType)
        {
            if (InteractionType == InteractionType.DB)
                throw new InvalidCastException("Common permission cannot take interaction type DB");

            InteractionTargetId = interactionTargetId;
        }
    }
}
