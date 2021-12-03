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
        /// <summary>
        /// Método para obtener los horarios
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public ScheduleModel GetSchedule(int scheduleId)
        {
            string query = $"SELECT * FROM Schedule WHERE ScheduleId = {scheduleId}";
            List<ScheduleModel> scheduleModelList = this.ObtenerListaSQL<ScheduleModel>(query).ToList();
            ScheduleModel scheduleModel = scheduleModelList.FirstOrDefault();

            return scheduleModel;
        }

        /// <summary>
        /// Método para verificar si la hora actual está entre los horarios de bloqueo del dispositivo
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public bool CompareScheduleWithSystemTime(int scheduleId)
        {
            ScheduleModel scheduleModel = GetSchedule(scheduleId);
            string horaInicio = scheduleModel.ScheduleStartTime.ToString("HH:mm");
            string horaFin = scheduleModel.ScheduleEndTime.ToString("HH:mm");

            string horaActual = DateTime.Now.ToString("HH:mm");
            

            DateTime.ParseExact(horaActual, "HH:mm", null);
            DateTime.ParseExact(horaInicio, "HH:mm", null);
            DateTime.ParseExact(horaFin, "HH:mm", null);


            if (horaActual.CompareTo(horaInicio) <= 0 && horaActual.CompareTo(horaFin) >= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método para verificar si la hora actual está entre los horarios de bloqueo del dispositivo
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public bool ShowMessageScheduleWithSystemTime(int scheduleId)
        {
            ScheduleModel scheduleModel = GetSchedule(scheduleId);
            string horaFin = scheduleModel.ScheduleEndTime.ToString("HH:mm");
            string horaActual = DateTime.Now.ToString("HH:mm");

            DateTime ObtenerHora = scheduleModel.ScheduleEndTime.AddMinutes(-10);

            string HoraAUtilizar = ObtenerHora.ToString("HH:mm");

            DateTime.ParseExact(horaActual, "HH:mm", null);
            DateTime.ParseExact(horaFin, "HH:mm", null);
            DateTime.ParseExact(HoraAUtilizar, "HH:mm", null);

            if (horaActual.CompareTo(HoraAUtilizar) >= 0  && horaActual.CompareTo(horaFin) <= 0)
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
