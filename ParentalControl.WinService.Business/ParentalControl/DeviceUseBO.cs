using ParentalControl.WinService.Data;
using ParentalControl.WinService.Models.Device;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Business.ParentalControl
{
    public class DeviceUseBO
    {
        /// <summary>
        /// Obtiene el uso del dispositivo para un día especificado
        /// </summary>
        /// <param name="infantId"></param>
        /// <param name="dia"></param>
        /// <returns></returns>
        public List<DeviceUseModel> GetDeviceUse(int infantId, string dia)
        {
            string query = $"SELECT * FROM DeviceUse WHERE InfantAccountId = '{infantId}'" +
                           $" AND DeviceUseDay = '{dia}'";

            List<DeviceUseModel> deviceUseModelList = this.ObtenerListaSQL<DeviceUseModel>(query).ToList();

            return deviceUseModelList;
        }

        /// <summary>
        /// Método para convertir una lista DataTable a un TModel (Modelo genérico)
        /// </summary>
        /// <param name="TModel"></param>
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
