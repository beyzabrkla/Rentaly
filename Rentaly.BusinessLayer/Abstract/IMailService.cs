using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IMailService
    {
        Task SendReservationApprovalMailAsync(Rental rental);
    }
}