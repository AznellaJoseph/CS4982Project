using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL
{
    [TestClass]
    public class TestDatabaseBuilder
    {
        private static MySqlConnection _connection = new(Connection.ConnectionString);
        private static readonly int TIMEOUT_SECONDS = 30;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            StartDatabaseServer();
            _connection = new MySqlConnection(Connection.ConnectionString);

            var endTime = DateTime.Now.AddSeconds(TIMEOUT_SECONDS);
            var timeOut = false;

            while (_connection.State != ConnectionState.Open)
            {
                try
                {
                    _connection.Open();
                }
                catch
                {
                    // ignored
                }

                if (DateTime.Now.CompareTo(endTime) < 0) continue;
                timeOut = true;
                break;
            }

            if (timeOut)
                throw new TimeoutException("Timed out waiting for the Database connection to open.");
            _connection.Close();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            StopDatabaseServer();
        }

        private static void StartDatabaseServer()
        {
            var dbProcess = new Process
            {
                StartInfo = new ProcessStartInfo
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
            var dbProcess = new Process
            {
                StartInfo = new ProcessStartInfo
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
            var testDirectory = Directory
                .GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName)
                .FullName;
            var fullDirectory = Directory.GetParent(testDirectory).FullName;
            return fullDirectory;
        }
    }
}