namespace CommonLayer.Models.Dto.General
{
    public class ResultDto
    {
        public bool IsSucceeded { get; set; }
        public string? Error { get; set; }

        public ResultDto() { }

        public ResultDto(bool isSucceeded, string? error = null)
        {
            IsSucceeded = isSucceeded;
            Error = error;
        }
    }

    public class ResultDto<T> : ResultDto
    {
        public T? Data { get; set; }

        public ResultDto() : base() { }

        public ResultDto(bool isSucceeded, string? error = null, T? data = default) : base(isSucceeded, error)
        {
            IsSucceeded = isSucceeded;
            Error = error;
            Data = data;
        }
    }
}
