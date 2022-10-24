using AppCore.DataAccess.Results;
using AppCore.DataAccess.Results.Bases;
using AppCore.Records.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppCore.DataAccess.Services
{
    // servicebase'ye TEnttihy tanımladık ve where ile class yapısında olması gerektiğini söyledik ve new() belirttik bu şekilde TE yerine int,array,double,string vs. gibi değer/ref gelemez, sadece new'Lenebilir class'lar gelir. Yani kısıtlama getirdik!

    // REPOSITORY PATTERN

    public abstract class ServiceBase<TEntithy> : IDisposable where TEntithy : /*class*/ Record, new()  // class yerine recordbase ile direk classı tanımladık aksi halde class yazarsak bütün class'ları alabiliriz anlamında olur.
    {
        // entithyfreamwork sqlserver classıdır. Hiçbir projeye bağımlı halde değildir temel bir istemcidir. 
        // Temel bir yapı olup başka projelere injection yapılması gerektiği için newlemeden  belirtmemiz lazım ve dışarıda enjekte etmemiz lazım.

        //DbContext _dbContext = new DbContext();

        protected readonly DbContext _dbContext;

        const string _errorMessage = "Changes not saves!";

        protected ServiceBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ServiceBase service = new ServiceBase<Kategori>();   -- Kategori yerine ne alacaksan onu yaz burası TEntithy aslında 
        // service.Query()

        //TEntithy başka projelerde kullanılabilmesi için default bir ayar belirledik herhangi bişey olabilir.(Kategori,Urun,Oyun vs.)
        public virtual IQueryable<TEntithy> Query()     // Sorguyu oluşturduk ama çalıştırmadık
        {
            return _dbContext.Set<TEntithy>();
        }

        public virtual List<TEntithy> GetList()     // ToList() ile sorguyu çağırıp çalıştırdık
        {
            return Query().ToList();
        }

        public virtual async Task<List<TEntithy>> GetListAsync()    // async çalışacak method belirledik ekstra
        {
            return await Query().ToListAsync();
        }

        //db.Oyunlar.Where(oyun => oyun.Adi == "RDR 2").ToList().SingleOrDefault();
        public virtual List<TEntithy> GetList(Expression<Func<TEntithy, bool>> predicate)
        {
            return Query().Where(predicate).ToList();
        }

        public virtual async Task<List<TEntithy>> GetListAsync(Expression<Func<TEntithy, bool>> predicate)    // Async
        {
            return await Query().Where(predicate).ToListAsync();
        }

        public virtual TEntithy GetItem(int id)
        {
            return Query().SingleOrDefault(q => q.Id == id);          // kayıt bulamazsa null döner

            //return Query().Find(id);                              // sadece DbSet tipindeki koleksiyonlarda kullanılabilir
            //return Query().Single(q => q.Id == id);              // Birden fazla kayıt dönerse hata(exception) fırlatır
            //return Query().FirstOrDefault(q => q.Id == id);     // ilk kaydı döner bulamazsa null döner default'tan kaynaklı
            //return Query().LastOrDefault(q => q.Id == id);     // Son kaydı döner bulamazsa null döner 
            //return Query().First(q => q.Id == id);
            //return Query().Last(q => q.Id == id);
        }

        // db.Oyunlar.Add(Oyun);
        // return new SuccessResult();
        // return new ErrorResult("GTA V adında oyun mevcuttur");
        public virtual Result Add(TEntithy entithy, bool save = true)
        {
            _dbContext.Set<TEntithy>().Add(entithy);
            if (save)
            {
                Save();
                return new SuccessResult("Added successfully.");
            }
            return new ErrorResult(_errorMessage);
        }

        public virtual Result Update(TEntithy entithy, bool save = true)
        {
            _dbContext.Set<TEntithy>().Update(entithy);
            if (save)
            {
                Save();
                return new SuccessResult("Added successfully.");
            }
            return new ErrorResult(_errorMessage);
        }

        public virtual Result Delete(TEntithy entithy, bool save = true)
        {
            _dbContext.Set<TEntithy>().Remove(entithy);
            if (save)
            {
                Save();
                return new SuccessResult("Added successfully.");
            }
            return new ErrorResult(_errorMessage);
        }


        public virtual Result Delete(Expression<Func<TEntithy, bool>> predicate, bool save = true)
        {
            var entities = Query().Where(predicate).ToList();
            foreach (var entity in entities)
            {
                Delete(entity, false);
            }
            if (save)
            {
                Save();
                return new SuccessResult("Deleted successfully.");
            }
            return new ErrorResult(_errorMessage);
        }


        public virtual int Save()
        {
            try
            {
                return _dbContext.SaveChanges();    // db'ye save ettik
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
