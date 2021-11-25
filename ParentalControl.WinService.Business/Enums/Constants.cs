using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Business.Enums
{
    public class Constants
    {
        public int Access
        {
            get
            {
                return 0;
            }
        }

        public int NoAccess
        {
            get
            {
                return 1;
            }
        }

        public int RequestStateCreated
        {
            get
            {
                return 0;
            }
        }

        public int RequestStateApproved
        {
            get
            {
                return 1;
            }
        }

        public int RequestStateDisapproved
        {
            get
            {
                return 2;
            }
        }

        public int WebConfiguration
        {
            get
            {
                return 1;
            }
        }

        public int AppConfiguration
        {
            get
            {
                return 2;
            }
        }

        public int DeviceConfiguration
        {
            get
            {
                return 3;
            }
        }
    }
}
