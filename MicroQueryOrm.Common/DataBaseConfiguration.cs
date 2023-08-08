namespace MicroQueryOrm.Common
{
    public class DataBaseConfiguration : IDataBaseConfiguration
    {
        public string ConnectionStringName { get; set; }
        public string ConnectionString { get; set; }
        public int CommandTimeout { get; set; }
    }
}
