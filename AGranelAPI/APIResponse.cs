using System.Net;

namespace AGranelAPI
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsExitoso { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public object Resultado { get; set; } = new object();
    }
}
