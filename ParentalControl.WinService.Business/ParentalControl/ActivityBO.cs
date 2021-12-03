using ParentalControl.WinService.Data;
using ParentalControl.WinService.Models.InfantAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Business.ParentalControl
{
    public class ActivityBO
    {
       
        /// <summary>
        /// Método para registrar la actividad
        /// </summary>
        /// <param name="infantId">Id del Infante</param>
        /// <param name="appName">Nombre de la App</param>
        /// <returns>bool: TRUE(registro exitoso), FALSE(error al registrar)</returns>
        public bool RegisterApps(int infantId, string objectActivity)
        {
            var creationDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            bool execute = false;

            string query = $"SELECT * FROM Activity WHERE InfantAccountId = {infantId}" +
                           $" AND ActivityObject = '{objectActivity}'" +
                           $" AND CAST(ActivityCreationDate AS date) = '{dateNow}'";

            List<ActivityModel> activityModelList = this.ObtenerListaSQL<ActivityModel>(query).ToList();

            if (activityModelList.Count > 0)
            {
                int activityId = activityModelList.FirstOrDefault().ActivityId;
                int timesAccess = activityModelList.FirstOrDefault().ActivityTimesAccess + 1;
                string description = $"{dateNow} - El/La infante intentó acceder a {objectActivity} por {timesAccess} ocasiones.";

                query = $"UPDATE Activity SET ActivityTimesAccess = timesAccess, ActivityDescription = '{description}'" +
                        $" WHERE ActivityId = {activityId}";

                execute = SQLConexionDataBase.Execute(query);
            }
            else
            {
                string description = $"{dateNow} - El/La infante intentó acceder a {objectActivity} por {1} ocasión.";
                query = $"INSERT INTO Activity VALUES ({infantId}, '{objectActivity}', '{description}'," +
                        $" '{creationDate}', {1})";
                execute = SQLConexionDataBase.Execute(query);
            }

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
