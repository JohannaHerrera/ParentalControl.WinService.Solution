using ParentalControl.WinService.Business.Mail;
using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Processors.DeviceLock
{
    internal class Processor : BaseProcessor
    {

        #region public override properties

        public override string Name { get { return ServiceConfiguration.DeviceLock; } }

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
