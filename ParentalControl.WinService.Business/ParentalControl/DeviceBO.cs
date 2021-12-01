using DeviceId;
using ParentalControl.WinService.Data;
using ParentalControl.WinService.Models.Device;
using ParentalControl.WinService.Models.InfantAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
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
            string query = $"SELECT wa.WindowsAccountId, wa.WindowsAccountName, wa.InfantAccountId, wa.DevicePCId" +
                           $" FROM WindowsAccount wa INNER JOIN DevicePC pc" +
                           $" ON wa.DevicePCId = pc.DevicePCId" +
                           $" WHERE pc.DevicePCCode = '{deviceCode}'" +
                           $" AND wa.WindowsAccountName = '{windowsAccountName}'" +
                           $" AND wa.InfantAccountId IS NOT NULL";

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
        /// Método para obtener las cuentas Windows del dispositivo
        /// </summary>
        /// <returns>List<string></returns>
        public List<string> GetWindowsAccounts()
        {
            List<string> users = new List<string>();
            var path = string.Format("WinNT://{0},computer", Environment.MachineName);

            using (var computerEntry = new DirectoryEntry(path))
            {
                foreach (DirectoryEntry childEntry in computerEntry.Children)
                {
                    if (childEntry.SchemaClassName == "User")
                    {
                        users.Add(childEntry.Name);
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Método para obtener las cuentas Windows registradas en la base de datos
        /// </summary>
        /// <returns>List<WindowsAccountModel></returns>
        public List<WindowsAccountModel> GetWindowsAccountsFromDB()
        {
            string deviceCode = this.GetDeviceIdentifier();
            string query = $"SELECT wa.WindowsAccountId, wa.WindowsAccountName, wa.InfantAccountId, wa.DevicePCId" +
                           $" FROM WindowsAccount wa INNER JOIN DevicePC pc" +
                           $" ON wa.DevicePCId = pc.DevicePCId" +
                           $" WHERE pc.DevicePCCode = '{deviceCode}'";

            List<WindowsAccountModel> windowsAccountModelList = this.ObtenerListaSQL<WindowsAccountModel>(query).ToList();

            return windowsAccountModelList;
        }

        /// <summary>
        /// Método para verificar si ya está registrada una cuenta Windows de la PC
        /// </summary>
        /// <param name="windowsAccountName">nombre de la cuenta Windows</param>
        /// <returns>List<WindowsAccountModel></returns>
        public List<WindowsAccountModel> VerifyWindowsAccount(string windowsAccountName)
        {
            string deviceCode = this.GetDeviceIdentifier();
            string query = $"SELECT wa.WindowsAccountId, wa.WindowsAccountName, wa.InfantAccountId" +
                           $" FROM WindowsAccount wa INNER JOIN DevicePC pc" +
                           $" ON wa.DevicePCId = pc.DevicePCId" +
                           $" WHERE pc.DevicePCCode = '{deviceCode}'" +
                           $" AND wa.WindowsAccountName = '{windowsAccountName}'";

            List<WindowsAccountModel> deviceModelList = this.ObtenerListaSQL<WindowsAccountModel>(query).ToList();

            return deviceModelList;
        }

        /// <summary>
        /// Método para vincular la cuenta Windows con una cuenta Infantil
        /// </summary>
        /// <param name="deviceModel">contiene la data del dispositivo PC</param>
        /// <returns>bool: TRUE(registro exitoso), FALSE(error al registrar)</returns>
        public bool RegisterWindowsAccount(string windowsAccountName)
        {
            var creationDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            DeviceBO deviceBO = new DeviceBO();
            string deviceCode = deviceBO.GetDeviceIdentifier();
            bool execute = false;

            string query = $"SELECT * FROM DevicePC WHERE DevicePCCode = '{deviceCode}'";
            List<DeviceModel> deviceModelList = this.ObtenerListaSQL<DeviceModel>(query).ToList();

            if (deviceModelList.Count > 0)
            {
                int deviceId = deviceModelList.FirstOrDefault().DevicePCId;
                query = $"INSERT INTO WindowsAccount VALUES ('{windowsAccountName}'," +
                               $" '{creationDate}', {deviceId}, NULL)";

                execute = SQLConexionDataBase.Execute(query);
            }

            return execute;
        }

        /// <summary>
        /// Método para eliminar la cuenta Windows
        /// </summary>
        /// <param name="deviceModel">contiene la data del dispositivo PC</param>
        /// <returns>bool: TRUE(registro exitoso), FALSE(error al registrar)</returns>
        public bool DeleteWindowsAccount(string windowsAccountName)
        {
            DeviceBO deviceBO = new DeviceBO();
            string deviceCode = deviceBO.GetDeviceIdentifier();
            bool execute = false;

            string query = $"SELECT * FROM DevicePC WHERE DevicePCCode = '{deviceCode}'";
            List<DeviceModel> deviceModelList = this.ObtenerListaSQL<DeviceModel>(query).ToList();

            if (deviceModelList.Count > 0)
            {
                int deviceId = deviceModelList.FirstOrDefault().DevicePCId;
                query = $"DELETE FROM WindowsAccount WHERE DevicePCId = {deviceId}" +
                        $" AND WindowsAccountName = '{windowsAccountName}'";

                execute = SQLConexionDataBase.Execute(query);
            }

            return execute;
        }

        /// <summary>
        /// Método para verificar si hay cuentas Windows relacionadas a un infante
        /// </summary>
        /// <param name="infantId">Id del Infante</param>
        /// <returns>List<WindowsAccountModel></returns>
        public List<WindowsAccountModel> VerifyWindowsAccountFromInfants(int infantId)
        {
            DeviceBO deviceBO = new DeviceBO();
            string deviceCode = deviceBO.GetDeviceIdentifier();
            string query = $"SELECT wa.WindowsAccountId, wa.WindowsAccountName" +
                           $" FROM WindowsAccount wa INNER JOIN DevicePC pc" +
                           $" ON wa.DevicePCId = pc.DevicePCId" +
                           $" WHERE pc.DevicePCCode = '{deviceCode}'" +
                           $" AND wa.InfantAccountId = {infantId}";

            List<WindowsAccountModel> deviceModelList = this.ObtenerListaSQL<WindowsAccountModel>(query).ToList();

            return deviceModelList;
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
