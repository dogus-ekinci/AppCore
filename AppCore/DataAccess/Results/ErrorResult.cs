using AppCore.DataAccess.Results.Bases;

namespace AppCore.DataAccess.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message) : base(false, message)
        {

        }

        // overload yöntemiyle yani imzayı değiştirdik parametereye yazmadan mesajsız göndermek için sadece true verdik
        public ErrorResult() : base(false, "")
        {

        }

    }
}


/* Örnek Kullanım:
Result result = new ErrorResult("Record exists in database!");
if(result.IsSuccessful) // result.IsSuccessful = false;
{
    ...
}
else
{
    (burası çalışacak..)
}
*/
