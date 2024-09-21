namespace ProductImporter.Utils
{
    public class DataTimeProvider : IDataTimeProvider
    {
        private readonly DateTime _currentDateTime;

        public DataTimeProvider()
        {
            _currentDateTime = DateTime.UtcNow;
        }

        public DateTime GetUtcDateTime() => _currentDateTime;
    }
}
