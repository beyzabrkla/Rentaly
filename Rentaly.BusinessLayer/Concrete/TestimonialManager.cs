using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;

namespace Rentaly.BusinessLayer.Concrete
{
    public class TestimonialManager : GenericManager<Testimonial>, ITestimonialService
    {
        public TestimonialManager(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
