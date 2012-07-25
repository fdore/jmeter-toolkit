using System.Collections.Generic;
using JMeter.Toolkit.Services.Spec;

namespace JMeter.Toolkit.Engine.Processor
{
    public abstract class DataResultsHandler
    {
        private DataResultsHandler _nextHandler;

        public DataResultsHandler()
        {
            _nextHandler = NullDataResultsHandler.NULL;
        }

        public Chart[] ProcessResults(IEnumerable<RequestDataResults> results)
        {
            if(CanHandle(results))
            {
                return HandleResults(results);
            }
            return _nextHandler.HandleResults(results);
        }

        protected abstract Chart[] HandleResults(IEnumerable<RequestDataResults> results);
       
        public abstract bool CanHandle(IEnumerable<RequestDataResults> results);

        public void AppendNextHandler(DataResultsHandler lastHandler)
        {
            if (_nextHandler == NullDataResultsHandler.NULL)
            {
                _nextHandler = lastHandler;
            } else
            {
                _nextHandler.AppendNextHandler(lastHandler);
            }
        }

        internal class NullDataResultsHandler : DataResultsHandler
        {
            private static readonly NullDataResultsHandler _nullHandler = new NullDataResultsHandler();
            
            public static NullDataResultsHandler NULL
            {
                get { return _nullHandler; }
            }

            protected override Chart[] HandleResults(IEnumerable<RequestDataResults> results)
            {
                return new Chart[0];
            }

            public override bool CanHandle(IEnumerable<RequestDataResults> results)
            {
                return true;
            }
        }
    }

     
}
