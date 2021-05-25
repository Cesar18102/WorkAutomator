using WorkAutomatorLogic.Aspects;

namespace WorkAutomatorLogic.Models
{
    public class UserActionModel<TModel> where TModel : ModelBase
    {
        [InitiatorAccountId]
        public int UserAccountId { get; set; }
        public TModel Data { get; set; }
    }
}
