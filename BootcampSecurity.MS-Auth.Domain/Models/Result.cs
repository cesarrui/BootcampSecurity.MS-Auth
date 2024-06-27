using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampSecurity.MS_Auth.Domain
{
    public class Result<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public Result(bool IsSuccess, string Message, T? Data)
        {
            this.Data = Data;
            this.IsSuccess = IsSuccess;
            this.Message = Message;

        }

        public static Result<T> Success(T data) => new Result<T>(true, "Successful Request", data);
        public static Result<T> Failure(string message) => new Result<T>(false, message, default);
    }
}
