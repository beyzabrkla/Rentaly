using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.EntityFramework;

namespace Rentaly.DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable // IDisposable ekleyerek, UnitOfWork nesnesinin yaşam döngüsünü yönetebiliriz 
                                               // IDisposable, nesnenin bellekten temizlenmesini sağlar. UnitOfWork kullanımı bittiğinde, Dispose() metodu çağrılarak kaynaklar serbest bırakılır.
    {
        IBranchDal BranchDal { get; }
        IBrandDal BrandDal { get; }
        ICarDal CarDal { get; } // ICarDal, araçlarla ilgili veritabanı işlemlerini gerçekleştiren bir repository'yi temsil eder. Bu repository, araç ekleme, silme, güncelleme ve listeleme harici özel işlemleri içerebildiğinden dolayı ekleme yapıldı.
        ICarModelDal CarModelDal { get; }
        ICategoryDal CategoryDal { get; }
        ICustomerDal CustomerDal { get; }
        IRentalDal RentalDal { get; }
        IAboutDal AboutDal { get; }
        IBannerDal BannerDal {  get; }
        IFaqDal FAQDal {  get; }
        IProcessDal ProcessDal {  get; }
        ITestimonialDal TestimonialDal { get; }

        IGenericDal<T> GetRepository<T>() where T : class;    //GetRepository<T>() metodu, herhangi bir entity türü için generic repository'ye erişim sağlar.
                                                              //Bu sayede, belirli bir entity türü için özel bir repository tanımlamak yerine, tek bir generic repository üzerinden tüm entity türlerine erişebiliriz.
        RentalyContext Context { get; } // UnitOfWork üzerinden DbContext'e erişim sağlamak için ekledik. Böylece, Business katmanında ihtiyaç duyduğumuzda context üzerinden doğrudan sorgular yapabiliriz.
        Task SaveAsync(); // Tüm işlemleri tek seferde veritabanına işler
    }
}
