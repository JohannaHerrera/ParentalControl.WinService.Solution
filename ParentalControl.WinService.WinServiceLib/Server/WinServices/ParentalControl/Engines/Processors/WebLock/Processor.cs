using ParentalControl.WinService.Business.Mail;
using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParentalControl.WinService.Business;
namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Processors.WebLock
{
    internal class Processor : BaseProcessor
    {
        
        #region public override properties

        public override string Name { get { return ServiceConfiguration.WebLock; } }
        int i = 1;
        public void block(int i)
        {

        }

        #endregion


        #region protected override functions

        protected override void ExecuteInternal(OperationContext operationContext)
        {
            try
            {


            }
            catch (Exception ex)
            {

            }
        }

        #endregion protected override functions
    }
}
