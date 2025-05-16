using System.Text.Json.Serialization;

namespace Pattern.Core.Responses
{
    public class ResponseDto
    {
        private ResponseDto()
        {
        }

        public object? Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto Error { get; private set; }

        public static ResponseDto Success(object? data = null, int statusCode = 200)
        {
            return new ResponseDto
            {
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ResponseDto Success(int statusCode)
        {
            return new ResponseDto
            {
                Data = null,
                StatusCode = statusCode
            };
        }

        public static ResponseDto Fail(ErrorDto errorDto, int statusCode)
        {
            return new ResponseDto
            {
                Error = errorDto,
                StatusCode = statusCode
            };
        }

        public static ResponseDto Fail(string errorMessage, int statusCode, bool isShow = true)
        {
            return new ResponseDto
            {
                Error = new ErrorDto(errorMessage, isShow),
                StatusCode = statusCode
            };
        }
    }
}