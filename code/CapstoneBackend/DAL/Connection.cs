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
        public static readonly string connectionString = "server=127.0.0.1;;port=3306;uid=root;database=capstone;";
    }
}
