namespace ApplicationKernel.Domain.MediatorSystem
{
    public class Response
    {
        internal Response(string errorMessage, bool isSuccess)
        {
            ErrorMessage = errorMessage;
            IsSuccess = isSuccess;
        }

        public string ErrorMessage { get; }
        public bool IsSuccess { get; }
    }

    public class Response<T> : Response
    {
        internal Response(T payload, string errorMessage, bool isSuccess) : base(errorMessage, isSuccess)
        {
            Payload = payload;
        }

        public T Payload { get; }
    }

    public static class Responses
    {
        public static Response Success() => new Response(null, true);
        public static Response Success<T>(T data) => new Response<T>(data, null, true);

        public static Response Failure() => new Response(null, false);
        public static Response Failure(string errorMessage) => new Response(errorMessage, false);
    }
}
