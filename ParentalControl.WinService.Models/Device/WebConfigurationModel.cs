using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Models.Device
{
    public class WebConfigurationModel
    {
        public int WebConfigurationId { get; set; }
        public bool WebConfigurationAccess { get; set; }
        public int CategoryId { get; set; }
        public int InfantAccountId { get; set; }
    }
}
