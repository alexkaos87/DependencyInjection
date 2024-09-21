using System.Diagnostics.Metrics;

namespace ProductImporter.Utils
{
    public class ReferenceGenerator : IReferenceGenerator
    {
        private readonly IDataTimeProvider _dataTimeProvider;
        private int _referenceCount = -1;

        public ReferenceGenerator(IDataTimeProvider dataTimeProvider)
        {
            _dataTimeProvider = dataTimeProvider;
        }

        public string GetReference()
        {
            ++_referenceCount;
            var dateTime = _dataTimeProvider.GetUtcDateTime();

            return $"{dateTime:yyyy-MM-ddTHH:mm:ss.FFF}-{_referenceCount:D4}";
        }
    }

    public interface IReferenceGenerator
    {
        string GetReference();
    }
}
