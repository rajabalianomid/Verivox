namespace Verivox.Service.API.Models
{
    public class CommonResponse<T> where T : class
    {
        public T Result { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
    }
}
