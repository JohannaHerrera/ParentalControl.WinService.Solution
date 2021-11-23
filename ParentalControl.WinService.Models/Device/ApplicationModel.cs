using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Models.Device
{
    /// <summary>
    /// Modelo de la tabla App
    /// </summary>
    public class ApplicationModel
    {
        // Id de la App
        public int AppId { get; set; }
        // Id del infante
        public int InfantAccountId { get; set; }
        // Id del infante
        public int ScheduleId { get; set; }
        // Id del dispositivo PC
        public int DevicePCId { get; set; }
        // Nombre de la App
        public string AppName { get; set; }
        // Acceso a la App
        public bool AppAccessPermission { get; set; }
        // Fecha de creación
        public DateTime AppCreationDate { get; set; }
    }
}
