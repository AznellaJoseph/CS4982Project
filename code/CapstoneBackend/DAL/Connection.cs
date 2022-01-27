using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneBackend.DAL
{
    /// <summary>
    /// Connection Class to connect to local database
    /// </summary>
    public class Connection
    {
        public static readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CapstoneDatabase;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
    }
}
