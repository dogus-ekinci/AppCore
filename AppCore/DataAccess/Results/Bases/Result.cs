namespace AppCore.DataAccess.Results.Bases
{
    public abstract class Result
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public Result(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

    }
}


/* 
 * 
Result result = new Result()
{
    IsSıccessful = true,
    Message = "Operation successful."

    VEYA

    Result result = new Result(true, "Operation success.");
}

*/
