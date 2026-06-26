namespace PetCare.Application.Common
{
    public class ApiResponse<T>
    {
        // Indica si la operación salió bien o mal
        public bool IsSuccess { get; set; }
        
        public string Message { get; set; } = string.Empty;
       
        public T? Data { get; set; }
        
        public List<string>? Errors { get; set; }
       
        public static ApiResponse<T> Success(T? data, string message = "Operación exitosa")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                Errors = null
            };
        }
       
        public static ApiResponse<T> Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
}