namespace ContaCorrente.Domain._Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorType { get; }
        public string? Message { get; }

        public Result(bool success, string? errorType, string? message)
        {
            IsSuccess = success;
            ErrorType = errorType;
            Message = message;
        }
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        protected Result(bool success, string? errorType, string? message, T? value)
            : base (success, errorType, message)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, null, null, value);

        public static Result<T> Failure(string errorType, string message) => new Result<T>(false, errorType, message, default);
    }
}
