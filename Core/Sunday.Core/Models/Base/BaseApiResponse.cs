using System.Collections.Generic;

namespace Sunday.Core.Models.Base
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
            Errors ??= new List<string>();
            Errors.Add(error);
            Success = false;
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            Errors ??= new List<string>();
            Errors.AddRange(errors);
            Success = false;
        }
        public void AddMessage(string message)
        {
            Messages ??= new List<string>();
            Messages.Add(message);
        }
    }
}
