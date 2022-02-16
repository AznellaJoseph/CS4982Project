namespace CapstoneBackend.Model
{
    /// <summary>
    ///     Response Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        /// <summary>
        ///     The status code of the response
        /// </summary>
        public uint StatusCode { get; set; } = 200;

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