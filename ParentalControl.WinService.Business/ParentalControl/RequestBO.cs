using ParentalControl.WinService.Business.Enums;
using ParentalControl.WinService.Data;
using ParentalControl.WinService.Models.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Business.ParentalControl
{
    public class RequestBO
    {
        /// <summary>
        /// Método para registrar la petición de aplicaciones
        /// </summary>
        /// <returns>bool: TRUE(registro exitoso), FALSE(error al registrar)</returns>
        public bool RegisterRequestApp(int infantId, int parentId, string appName)
        {
            Constants constants = new Constants();
            var dateCreation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            bool execute = false;
            
            string query = $"INSERT INTO Request VALUES ({constants.AppConfiguration}, {infantId}," +
                           $" '{appName}', NULL, {constants.RequestStateCreated}," +
                           $" '{dateCreation}', {parentId})";
            execute = SQLConexionDataBase.Execute(query);
            
            return execute;
        }

        /// <summary>
        /// Método para verificar si ya existe una solicitud igual
        /// </summary>
        /// <returns>bool: TRUE(existe), FALSE(no existe)</returns>
        public bool VerifyRequest(int requestType, string requestObject)
        {
            Constants constants = new Constants();
            bool exist = true;
            var dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            string query = string.Empty;
            List<RequestModel> requestModelList = new List<RequestModel>();

            if (requestType == constants.WebConfiguration || requestType == constants.AppConfiguration)
            {
                query = $"SELECT * FROM Request WHERE RequestState = 0" +
                        $" AND RequestObject = '{requestObject}'";
            }
            else if (requestType == constants.DeviceConfiguration)
            {
                query = $"SELECT * FROM Request WHERE RequestState = 0" +
                        $" AND CAST(RequestCreationDate AS date) = '{dateNow}'" +
                        $" AND RequestTime IS NOT NULL";
            }

            requestModelList = this.ObtenerListaSQL<RequestModel>(query).ToList();

            if (requestModelList.Count > 0)
            {
                exist = true;
            }
            else
            {
                exist = false;
            }

            return exist;
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
