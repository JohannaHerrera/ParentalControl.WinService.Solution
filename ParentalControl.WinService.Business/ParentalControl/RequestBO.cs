using ParentalControl.WinService.Business.Enums;
using ParentalControl.WinService.Data;
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
