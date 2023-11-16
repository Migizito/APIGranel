using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace AGranelAPI
{
    public class APIResponseVenta<T>
    {
        [AllowNull]
        public T Resultado { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public bool IsExitoso { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
