using ParentalControl.WinService.Data;
using ParentalControl.WinService.Models.Request;
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
        public bool CompareScheduleWithSystemTime(ScheduleModel scheduleModel)
        {
            string horaInicio = scheduleModel.ScheduleStartTime.ToString("HH:mm");
            string horaFin = scheduleModel.ScheduleEndTime.ToString("HH:mm");

            string horaActual = DateTime.Now.ToString("HH:mm");
            

            DateTime.ParseExact(horaActual, "HH:mm", null);
            DateTime.ParseExact(horaInicio, "HH:mm", null);
            DateTime.ParseExact(horaFin, "HH:mm", null);


            if (horaActual.CompareTo(horaInicio) >= 0 && horaActual.CompareTo(horaFin) <= 0)
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
        public bool ShowMessageScheduleWithSystemTime(ScheduleModel scheduleModel, int horaMensaje)
        {
            string horaFin = scheduleModel.ScheduleEndTime.ToString("HH:mm");
            string horaActual = DateTime.Now.ToString("HH:mm");

            DateTime ObtenerHora = scheduleModel.ScheduleEndTime.AddMinutes(-(horaMensaje));

            string HoraAUtilizar = ObtenerHora.ToString("HH:mm");

            DateTime.ParseExact(horaActual, "HH:mm", null);
            DateTime.ParseExact(horaFin, "HH:mm", null);
            DateTime.ParseExact(HoraAUtilizar, "HH:mm", null);

            if (horaActual.CompareTo(HoraAUtilizar) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método para comprobar si existe una solicitud de ampliar el dispositivo aprobada
        /// </summary>
        /// <returns></returns>
        public bool DeviceUseRequestApproved(int infantAccountId)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            string query = $"SELECT * FROM Request WHERE RequestTypeId = {3}" +
                           $" AND InfantAccountId = {infantAccountId}" +
                           $" AND RequestState = 1" +
                           $" AND CAST(RequestCreationDate AS date) = '{today}'";
            List<RequestModel> requestModelList = this.ObtenerListaSQL<RequestModel>(query).ToList();

            if (requestModelList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Método para obtener el nuevo horario, según el tiempo que pidió extender el infante
        /// </summary>
        /// <param name="infantId"></param>
        /// <param name="dia"></param>
        /// <returns></returns>
        public ScheduleModel GetNewScheduleIfDeviceUseRequestIsApproved(ScheduleModel scheduleModel,int infantAccountId)
        {
            ScheduleModel newSchedule = new ScheduleModel();
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            string query = $"SELECT * FROM Request WHERE RequestTypeId = {3}" +
                           $" AND InfantAccountId = {infantAccountId}" +
                           $" AND RequestState = {1}" +
                           $" AND CAST(RequestCreationDate AS date) = '{today}'";
            List<RequestModel> requestModelList = this.ObtenerListaSQL<RequestModel>(query).ToList();
            

            if (requestModelList.Count > 0)
            {
                newSchedule.ScheduleStartTime = scheduleModel.ScheduleStartTime;
                var horasAdicionales = Decimal.ToDouble(Math.Truncate(requestModelList.FirstOrDefault().RequestTime));
                string numStr = requestModelList.FirstOrDefault().RequestTime.ToString();
                decimal numDecimal = Decimal.Parse("0," + numStr.Split('.')[1]);
                var minutosAdicionales = Decimal.ToDouble(numDecimal);
                newSchedule.ScheduleEndTime = scheduleModel.ScheduleEndTime.AddHours(horasAdicionales).AddMinutes(minutosAdicionales);

            }
            else
            {
                newSchedule = scheduleModel;
            }
            
            return newSchedule;
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
