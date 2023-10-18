

namespace BookStore.API.Common
{   
    public enum ServiceResultCode
    {
            Ok=200,
            BadRequest=400,
            NotFound=404,
            Conflict=500,
            Code = 501,
            Unauthorized=401
    }
    public class ServiceResult<T>
    {
        public ServiceResultCode Code { get; set; }
        public T Result { get; set; }
        public string ErrorMessage { get; set; }
        public string accessToken { get; set; }
        public static ServiceResult<T> Success(T result)
        {
            return new ServiceResult<T> { Code = ServiceResultCode.Ok, Result = result };
        }
        public static ServiceResult<T> Error(ServiceResultCode code)
        {
            return new ServiceResult<T> { Code = code };
        }
        public static ServiceResult<T> Error(ServiceResultCode code, T result, string errorMessage)
        {
            return new ServiceResult<T> { Code = code, Result = result, ErrorMessage = errorMessage};
        }
    }
}
