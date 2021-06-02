using System;
using System.Text;

using Newtonsoft.Json;

using Constants;

using WorkAutomatorLogic.Exceptions;

namespace WorkAutomatorLogic.Services
{
    internal class DataTypeService
    {
        public object ConvertDataToType(string dataBase64, DataType dataType)
        {
            string data = Encoding.UTF8.GetString(
                Convert.FromBase64String(dataBase64)
            );

            switch (dataType)
            {
                case DataType.BOOL: return Convert.ToBoolean(data);
                case DataType.FLOAT: return Convert.ToSingle(data);
                case DataType.INT: return Convert.ToInt32(data);
                case DataType.BOOL_ARR: return JsonConvert.DeserializeObject<bool[]>(data);
                case DataType.FLOAT_ARR: return JsonConvert.DeserializeObject<float[]>(data);
                case DataType.INT_ARR: return JsonConvert.DeserializeObject<int[]>(data);
            }

            return null;
        }

        public void CheckIsDataOfType(string dataBase64, DataType dataType)
        {
            try { ConvertDataToType(dataBase64, dataType); }
            catch { throw new DataTypeException(); }            
        }
    }
}
