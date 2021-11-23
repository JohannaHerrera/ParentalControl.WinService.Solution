using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines
{
    internal abstract class BaseProcessor
    {

        #region public abstract properties
        public abstract string Name { get; }
        #endregion

        #region protected abstract functions
        protected abstract void ExecuteInternal(OperationContext operationContext);

        #endregion

        #region public function

        public void Execute(
            OperationContext operationContext)
        {
            // 1. Validate if we need to run this processor.
            if (!ProcessorIsEnabled(operationContext))
            {
                // This processor is not enabled, we don't anything else on this 
                // operation.
                return;
            }

            // 2. Execute the business logic.
            this.ExecuteInternal(operationContext);
        }
        #endregion


        #region private functions
        private bool ProcessorIsEnabled(
            OperationContext operationContext)
        {
            return operationContext.ServiceConfiguration.ProcessorIsEnabledByProcessorName[this.Name];
        }

        #endregion


    }
}
