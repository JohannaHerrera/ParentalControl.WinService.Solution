using DeviceId;
using ParentalControl.WinService.Data;
using ParentalControl.WinService.Models.Device;
using ParentalControl.WinService.Models.InfantAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Business.ParentalControl
{
    public class DeviceBO
    {

        /// <summary>
        /// Método para verificar si la cuenta Windows actual está vinculada a una cuenta infantil
        /// </summary>
        /// <param name="windowsAccountName">nombre de la cuenta Windows</param>
        /// <returns>WindowsAccountModel</returns>
        public WindowsAccountModel GetInfantAccount(string windowsAccountName)
        {
            string deviceCode = this.GetDeviceIdentifier();
            string query = $"SELECT wa.WindowsAccountId, wa.WindowsAccountName, wa.InfantAccountId" +
                           $" FROM WindowsAccount wa INNER JOIN DevicePC pc" +
                           $" ON wa.DevicePCId = pc.DevicePCId" +
                           $" WHERE pc.DevicePCCode = '{deviceCode}'" +
                           $" AND wa.WindowsAccountName = '{windowsAccountName}'";

            List<WindowsAccountModel> deviceModelList = this.ObtenerListaSQL<WindowsAccountModel>(query).ToList();
            WindowsAccountModel windowsAccountModel = null; 

            if(deviceModelList.Count > 0)
            {
                windowsAccountModel = deviceModelList.FirstOrDefault();
            }

            return windowsAccountModel;
        }

        /// <summary>
        /// Método para obtener la cuenta infantil
        /// </summary>
        /// <param name="infantAccountId">Id del infante</param>
        /// <returns>InfantAccount</returns>
        public InfantAccountModel GetInfantAccountLinked(int infantAccountId)
        {
            InfantAccountModel infantAccount = null;
            string query = $"SELECT * FROM InfantAccount WHERE InfantAccountId = {infantAccountId}";

            List<InfantAccountModel> infantAccountModelList = this.ObtenerListaSQL<InfantAccountModel>(query).ToList();

            if (infantAccountModelList.Count > 0)
            {
                infantAccount = infantAccountModelList.FirstOrDefault();
            }

            return infantAccount;
        }

        /// <summary>
        /// Método para obtener el email del Padre
        /// </summary>
        /// <returns>string</returns>
        public string GetParentEmail(int parentId)
        {
            string parentEmail = string.Empty;
            string query = $"SELECT * FROM Parent WHERE ParentId = {parentId}";
            List<ParentModel> parentModelList = this.ObtenerListaSQL<ParentModel>(query).ToList();

            if (parentModelList.Count > 0)
            {
                parentEmail = parentModelList.FirstOrDefault().ParentEmail;
            }
            return parentEmail;
        }


        /// <summary>
        /// Método para obtener el identificador del dispositivo
        /// </summary>
        /// <returns>macAddresses</returns>
        public string GetDeviceIdentifier()
        {
            string deviceId = new DeviceIdBuilder().AddMachineName().ToString();

            return deviceId;
        }

        /// <summary>
        /// Método para convertir una lista DataTable a un TModel (Modelo genérico)
        /// </summary>
        /// <param name="deviceModel">contiene la data del dispositivo PC</param>
        /// <returns>IList<TModel></returns>
        private IList<TModel> ObtenerListaSQL<TModel>(string query)
        {
            try
            {
                DataTable dataTableInformacion = SQLConexionDataBase.Query(query);
                var listaResultante = dataTableInformacion.MapDataTableToList<TModel>();

                return listaResultante;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
