namespace Pattern.Core.Responses
{
    public class ErrorDto
    {
        public List<string> Errors { get; private set; }

        public ErrorDto(string error)
        {
            Errors = new List<string>() { error };
        }

        public ErrorDto(List<string> errors)
        {
            Errors = errors;
        }
    }
}
