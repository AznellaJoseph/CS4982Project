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
        public static readonly string connectionString = "server=localhost;port=3308;database=capstone;uid=root;password=test";
    }
}
