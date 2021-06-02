using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Scripting;

using Autofac;

using Constants;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorLogic.Services
{
    internal class FaultConditionParseService
    {
        private const string DETECTOR_SETTING_PREFIX = "DETECTOR";
        private const string PIPELINE_ITEM_SETTING_PREFIX = "PITEM";
        private const string DATA_PREFIX = "DATA";

        private static DataTypeService DataTypeService = LogicDependencyHolder.Dependencies.Resolve<DataTypeService>();

        public async Task<bool> ParseCondition(
            DetectorFaultPrefabEntity faultPrefab, 
            DetectorDataEntity[] detectorDatas,
            DetectorSettingsValueEntity[] detectorSettings, 
            PipelineItemSettingsValueEntity[] pipelineItemSettings
        )
        {
            string condition = faultPrefab.fault_condition;

            foreach(DetectorSettingsValueEntity detectorSettingsValue in detectorSettings)
            {
                condition = condition.Replace(
                    $"{DETECTOR_SETTING_PREFIX}.{detectorSettingsValue.detector_settings_prefab.option_name}",
                    GetStringValue(detectorSettingsValue)
                );
            }

            foreach (PipelineItemSettingsValueEntity pipelineItemSettingsValue in pipelineItemSettings)
            {
                condition = condition.Replace(
                    $"{PIPELINE_ITEM_SETTING_PREFIX}.{pipelineItemSettingsValue.PipelineItemSettingsPrefab.option_name}",
                    GetStringValue(pipelineItemSettingsValue)
                );
            }

            foreach (DetectorDataEntity detectorData in detectorDatas)
            {
                condition = condition.Replace(
                    $"{DATA_PREFIX}.{detectorData.detector_data_prefab.field_name}",
                    GetStringValue(detectorData)
                );
            }

            return await CSharpScript.EvaluateAsync<bool>(condition);
        }

        private string GetStringValue(DetectorSettingsValueEntity value)
        {
            return GetStringValue(
                value.option_data_value_base64, 
                value.detector_settings_prefab.DataType.name.FromName()
            );
        }

        private string GetStringValue(PipelineItemSettingsValueEntity value)
        {
            return GetStringValue(
                value.option_data_value_base64,
                value.PipelineItemSettingsPrefab.DataType.name.FromName()
            );
        }

        private string GetStringValue(DetectorDataEntity value)
        {
            return GetStringValue(
                value.field_data_value_base64,
                value.detector_data_prefab.DataType.name.FromName()
            );
        }

        private string GetStringValue(string valueBase64, DataType dataType)
        {
            object value = DataTypeService.ConvertDataToType(valueBase64, dataType);

            switch(dataType)
            {
                case DataType.BOOL:
                case DataType.INT:
                case DataType.FLOAT:
                    return value.ToString();
                case DataType.BOOL_ARR:
                    return "(new bool[] {" + string.Join(", ", value) + "})";
                case DataType.INT_ARR:
                    return "(new int[] {" + string.Join(", ", value) + "})";
                case DataType.FLOAT_ARR:
                    return "(new float[] {" + string.Join(", ", value) + "})";
            }

            return null;
        }
    }
}
