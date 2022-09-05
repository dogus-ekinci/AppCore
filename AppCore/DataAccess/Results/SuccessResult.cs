using AppCore.DataAccess.Results.Bases;

namespace AppCore.DataAccess.Results
{
    internal class SuccessResult : Result
    {
        // başarılı sonucu döneceğimiz için parametre tanımlamadan base yapıda true dönmek yeterli
        public SuccessResult(string message) : base(true, message)  
        {

        }

        // overload yöntemiyle yani imzayı değiştirdik parametereye yazmadan mesajsız göndermek için sadece true verdik
        public SuccessResult() : base(true, "")
        {

        }

    }
}

/* Örnek Kullanım:
SuccessResult result = new SuccessResult(true, "Operation seccessful.");
Result result = new SuccessResult();
if(result.IsSuccessful) // result.IsSuccessful = true;
{
    (burası çalışacak...)    
}
else
{
    ...
}
*/
