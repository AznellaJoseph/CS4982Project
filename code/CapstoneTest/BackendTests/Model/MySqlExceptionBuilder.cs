using System;
using System.Reflection;
using CapstoneBackend.Utils;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.Model
{
    /// <summary>
    ///     An Exception Builder for MySQL to be used in testing
    /// </summary>
    public class MySqlExceptionBuilder
    {
        private uint? _code;
        private string? _errorMessage;

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns></returns>
        public MySqlException Build()
        {
            if (_errorMessage is null && _code is null)
            {
                var ctor1 = typeof(MySqlException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
                return (MySqlException) ctor1.Invoke(Array.Empty<object>());
            }

            var ctor2 = typeof(MySqlException).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[7];
            return (MySqlException) ctor2.Invoke(new object[]
                {_code ?? (uint) Ui.StatusCode.InternalServerError, "", _errorMessage ?? ""});
        }

        /// <summary>
        ///     Withes the error.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public MySqlExceptionBuilder WithError(uint errorCode, string message)
        {
            _errorMessage = message;
            _code = errorCode;
            return this;
        }
    }
}