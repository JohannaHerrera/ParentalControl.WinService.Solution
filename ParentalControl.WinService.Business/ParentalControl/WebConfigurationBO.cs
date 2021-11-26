using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParentalControl.WinService.Data;
using System.Data;
using ParentalControl.WinService.Models.Device;
using DeviceId;



namespace ParentalControl.WinService.Business.ParentalControl
{
    class WebConfigurationBO
    {
        public IList<TModel> ObtenerListaSQL<TModel>(string query)
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

        public WebConfigurationModel GetWebConfiguration(string windowsAccountName)
        {
            string deviceCode = this.GetDeviceIdentifier();
            string query = $"SELECT WebConfigurationId, WebConfigurationAccess, CategoryId, WebConfiguration.InfantAccountId " +
                           $" FROM WebConfiguration INNER JOIN WindowsAccount " +
                           $" ON WebConfiguration.InfantAccountId = WindowsAccount.InfantAccountId" +
                           $" WHERE WindowsAccount.WindowsAccountName = '{windowsAccountName}'";
            List<WebConfigurationModel> webConfigurationModelList = this.ObtenerListaSQL<WebConfigurationModel>(query).ToList();
            WebConfigurationModel webConfigurationModel = null;

            if (webConfigurationModelList.Count > 0)
            {
                //windowsAccountModel = deviceModelList.FirstOrDefault(); Por que se saca el primero
                webConfigurationModel = webConfigurationModelList.FirstOrDefault();
            }

            return webConfigurationModel;

        }
        public string GetDeviceIdentifier()
        {
            string deviceId = new DeviceIdBuilder().AddMachineName().ToString();

            return deviceId;
        }


    }
}
