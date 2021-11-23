using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Models.Device
{
    /// <summary>
    /// Modelo de la tabla WindowsAccount
    /// </summary>
    public class WindowsAccountModel
    {
        // Id de la cuenta
        public int WindowsAccountId { get; set; }
        // Nombre de la cuenta
        public string WindowsAccountName { get; set; }
        // Fecha de creación
        public DateTime WindowsAccountCreationDate { get; set; }
        // Id del dispositivo
        public int DevicePCId { get; set; }
        // Id de la cuenta infantil
        public int InfantAccountId { get; set; }
    }
}
