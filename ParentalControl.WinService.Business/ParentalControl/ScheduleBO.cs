using ParentalControl.WinService.Data;
using ParentalControl.WinService.Models.ScheduleAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Business.ParentalControl
{
    public class ScheduleBO
    {
        public ScheduleModel GetSchedule(int scheduleId)
        {
            string query = $"SELECT * FROM Schedule WHERE ScheduleId = {scheduleId}";
            List<ScheduleModel> scheduleModelList = this.ObtenerListaSQL<ScheduleModel>(query).ToList();
            ScheduleModel scheduleModel = scheduleModelList.FirstOrDefault();

            return scheduleModel;
        }

        //hola
        public bool CompareScheduleWithSystemTime(int scheduleId)
        {
            ScheduleModel scheduleModel = GetSchedule(scheduleId);
            DateTime horaInicio = scheduleModel.ScheduleStartTime;
            DateTime horaFin = scheduleModel.ScheduleEndTime;
            string horaActual = DateTime.Now.ToString("HH:mm");
            DateTime.ParseExact(horaActual, "HH:mm", null);

            if (horaActual.CompareTo(horaInicio) >= 0 && horaActual.CompareTo(horaFin) <= 0)
            {
                return true;
            }
            return false;
        }
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
