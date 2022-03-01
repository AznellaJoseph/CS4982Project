using CapstoneBackend.Utils;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     Response Class used in the managers to specify the status and error messages of server actions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        /// <summary>
        ///     The status code of the response
        /// </summary>
        public uint StatusCode { get; set; } = (uint)Ui.StatusCode.Success;

        /// <summary>
        ///     The data of the response
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        ///     The error message of the response
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}