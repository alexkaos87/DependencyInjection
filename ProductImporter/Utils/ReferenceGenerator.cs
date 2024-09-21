namespace ProductImporter.Utils
{
    public class ReferenceGenerator : IReferenceGenerator
    {
        private readonly IDataTimeProvider _dataTimeProvider;
        private readonly IIncrementingCounter _incrementingCounter;

        public ReferenceGenerator(IDataTimeProvider dataTimeProvider, IIncrementingCounter incrementingCounter)
        {
            _dataTimeProvider = dataTimeProvider;
            _incrementingCounter = incrementingCounter;
        }

        public string GetReference()
        {
            var dateTime = _dataTimeProvider.GetUtcDateTime();

            return $"{dateTime:yyyy-MM-ddTHH:mm:ss.FFF}-{_incrementingCounter.GetNext():D4}";
        }
    }
}
