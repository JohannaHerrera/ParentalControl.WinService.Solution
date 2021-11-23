using ParentalControl.WinService.Business.Mail;
using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines
{
    public class WinServiceEngine : ApplicationException
    {

        #region private members
        private System.Timers.Timer _timer = null;
        #endregion


        #region public functions
        public void Start()
        {
            try
            {
                string buildMode = "RELEASE";

#if DEBUG
                buildMode = "DEBUG";
#endif

                // 1. Create the operation context.  This validates the server settings 
                //    (makes sure that the output culture name is valid, the required
                //    folders exist, the multilanguage rules are valid).
                OperationContext operationContext = new OperationContext();

                // 2. Initialize timer.
                _timer = new System.Timers.Timer();
                _timer.Enabled = false;
                _timer.Interval = operationContext.ServiceSettings.TimerInterval;
                _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
                _timer.Enabled = true;

                operationContext.ServiceSettings.ImprimirProximaHoraEjecucion();

            }
            catch (Exception exception)
            {
                // throw the exception.
                throw;
            }
            finally
            {

            }
        }


        public void Stop()
        {
            try
            {
                // 1. Release structures.
                _timer.Enabled = false;
                _timer = null;
            }
            catch (Exception exception)
            {
                // throw the exception.
                throw;
            }
            finally
            {
                // Log the stop (end).
            }
        }


        public void ExecuteEngineOperation()
        {
            try
            {

                // 1. Create the operation context.  This validates the server settings 
                //    (makes sure that the output culture name is valid, the required
                //    folders exist, the multilanguage rules are valid).
                OperationContext operationContext = new OperationContext();

                // 2. Set the culture info to be used on the service.
                if (operationContext.ServiceConfiguration.ServiceCultureInfo != null)
                {
                    Thread.CurrentThread.CurrentCulture = operationContext.ServiceConfiguration.ServiceCultureInfo;
                }

                // 3. List all the processors.
                IList<BaseProcessor> processorList = new List<BaseProcessor>();
                processorList.Add(new Processors.WebLock.Processor());
                processorList.Add(new Processors.AppLock.Processor());
                processorList.Add(new Processors.DeviceLock.Processor());


                // 4. Invoke all the processors.
                foreach (BaseProcessor processor in processorList)
                {
                    InvokeProcessor(operationContext, processor);
                }

                // 5. Update the interval on the timer (maybe the interval setting was 
                //    changed since the last time that the timer fired).  We check if
                //    _timer was set to null because this function may be invoked by a
                //    test program, and that test program may not have called the Start
                //    function (which creates and initializes the timer).
                operationContext = new OperationContext();
                if (_timer != null)
                {
                    _timer.Interval = operationContext.ServiceSettings.TimerInterval;
                    operationContext.ServiceSettings.ImprimirProximaHoraEjecucion();
                }

            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                
            }
        }

        #endregion


        #region event handlers
        private void _timer_Elapsed(
            object sender,
            ElapsedEventArgs e)
        {
            // 1. Disable the timer (to stop other Elapsed events from entering this 
            //    code).
            _timer.Enabled = false;

            // 2. Process the input folder.
            ExecuteEngineOperation();

            // 3. Enable back the timer.
            _timer.Enabled = true;
        }
        #endregion


        #region private functions
        private void InvokeProcessor(
            OperationContext operationContext,
            BaseProcessor processor)
        {
            string processorName = "";
            try
            {
                processorName = processor.Name;
                processor.Execute(operationContext);
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {

            }
        }
        #endregion
    }
}
