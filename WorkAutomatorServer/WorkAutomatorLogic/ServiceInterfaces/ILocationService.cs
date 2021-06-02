using System.Threading.Tasks;

using Dto.Interaction;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface ILocationService
    {
        Task TryCheckout(CheckoutDto dto);
        Task TryEnter(EnterLeaveDto dto);
        Task TryLeave(EnterLeaveDto dto);
    }
}
