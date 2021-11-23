using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Models.ScheduleAccount
{
    /// <summary>
    /// Modelo de la tabla Schedule
    /// </summary>
    public class ScheduleModel
    {
        //Id del horario
        public int ScheduleId { get; set; }
        //Tiempo de inicio
        public DateTime ScheduleStartTime { get; set; }
        //Tiempo de fin
        public DateTime ScheduleEndTime { get; set; }
        //Fecha de creación
        public DateTime ScheduleCreationDate { get; set; }
        //Id del Padre
        public int ParentId { get; set; }
    }
}
