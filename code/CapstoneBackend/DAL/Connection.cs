namespace CapstoneBackend.DAL
{
    /// <summary>
    ///     Connection Class to connect to local database
    /// </summary>
    public class Connection
    {
        public static readonly string ConnectionString =
            "server=localhost;port=3308;database=capstone;uid=root;password=test";
    }
}