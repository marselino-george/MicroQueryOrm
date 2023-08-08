namespace MicroQueryOrm.Common
{
    public interface IDataBaseConfiguration
    {
        string ConnectionStringName { get; }

        string ConnectionString { get; }

        int CommandTimeout { get; }
    }
}
