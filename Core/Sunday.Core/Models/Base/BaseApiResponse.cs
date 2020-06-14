using System.Collections.Generic;

namespace Sunday.Core
{
    public class BaseApiResponse
    {
        public BaseApiResponse()
        {
            Errors = new List<string>();
            Success = true;
            Messages = new List<string>();
        }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Messages { get; set; }

        public void AddError(string error)
        {
            if (Errors == null) Errors = new List<string>();
            Errors.Add(error);
            Success = false;
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            if (Errors == null) Errors = new List<string>();
            Errors.AddRange(errors);
            Success = false;
        }
        public void AddMessage(string message)
        {
            if (Messages == null) Messages = new List<string>();
            Messages.Add(message);
        }
    }
}
