using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneTest.BackendTests.DAL
{
    [TestClass]
    public class TestDatabaseBuilder
    {
        private static MySqlConnection _connection;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            StartDatabaseServer();
            _connection = new MySqlConnection(Connection.ConnectionString);

            var endTime = DateTime.Now.AddSeconds(_connection.ConnectionTimeout);
            var timeOut = false;

            while (_connection.State != ConnectionState.Open)
            {
                try
                {
                    _connection.Open();
                }
                catch { }

                if (DateTime.Now.CompareTo(endTime) >= 0)
                {
                    timeOut = true;
                    break;
                }
            }

            if (timeOut)
                throw new TimeoutException("Timed out waiting for the Database connection to open.");
            else
                _connection.Close();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            StopDatabaseServer();
        }

        private static void StartDatabaseServer()
        {
            var dbProcess = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "capstone.sh",
                    Arguments = "server run",
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    WorkingDirectory = GetRootPath()
                }
            };

            dbProcess.Start();
            dbProcess.WaitForExit();
            dbProcess.Close();
        }

        private static void StopDatabaseServer()
        {
            var dbProcess = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "capstone.sh",
                    Arguments = "server stop",
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    WorkingDirectory = GetRootPath()
                }
            };

            dbProcess.Start();
            dbProcess.WaitForExit();
            dbProcess.Close();
        }

        private static string GetRootPath()
        {
            var testDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName;
            var fullDirectory = Directory.GetParent(testDirectory).FullName;
            return fullDirectory;
        }
    }
}
