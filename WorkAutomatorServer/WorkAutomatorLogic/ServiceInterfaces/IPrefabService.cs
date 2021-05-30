using System.Threading.Tasks;

using Dto;
using Dto.Prefabs;

using WorkAutomatorLogic.Models.Prefabs;

namespace WorkAutomatorLogic.ServiceInterfaces
{
    public interface IPrefabService
    {
        Task<PipelineItemPrefabModel> CreatePipelineItemPrefab(AuthorizedDto<PipelineItemPrefabDto> dto);
        Task<StorageCellPrefabModel> CreateStorageCellPrefab(AuthorizedDto<StorageCellPrefabDto> dto);
        Task<DetectorPrefabModel> CreateDetectorPrefab(AuthorizedDto<DetectorPrefabDto> dto);
    }
}
