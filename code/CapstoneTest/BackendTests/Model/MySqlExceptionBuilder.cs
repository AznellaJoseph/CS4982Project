using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.Model
{
    public class MySqlExceptionBuilder
    {
        private string? errorMessage = null;
        private uint? code = null;
        public MySqlException Build()
        {
            if (this.errorMessage is null && this.code is null)
            {
                var ctor1 = typeof(MySqlException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
                return (MySqlException) ctor1.Invoke(Array.Empty<object>());
            }
           
            var ctor2 = typeof(MySqlException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[7];
            return (MySqlException) ctor2.Invoke(new object[] {this.code ?? 500, "", this.errorMessage ?? ""});
        }

        public MySqlExceptionBuilder WithError(uint errorCode, string message)
        {
            this.errorMessage = message;
            this.code = errorCode;
            return this;
        }
    }
}