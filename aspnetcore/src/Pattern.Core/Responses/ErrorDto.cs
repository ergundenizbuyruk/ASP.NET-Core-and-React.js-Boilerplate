namespace Pattern.Core.Responses
{
    public class ErrorDto
    {
        public List<string> Errors { get; private set; }
        public bool IsShow { get; set; }

        public ErrorDto(string error, bool isShow = true)
        {
            Errors = new List<string>() { error };
            IsShow = isShow;
        }

        public ErrorDto(List<string> errors, bool isShow = true)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}