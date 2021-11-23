using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Models.Device
{
    /// <summary>
    /// Modelo de la tabla DevicePC
    /// </summary>
    public class DeviceModel
    {
        // Id del dispositivo
        public int DevicePCId { get; set; }
        // Nombre del dispositivo
        public string DevicePCName { get; set; }
        // Código del dispositivo
        public string DevicePCCode { get; set; }
        // Fecha de creación
        public DateTime DevicePCCreationDate { get; set; }
        // Id del Padre
        public int ParentId { get; set; }

    }
}
