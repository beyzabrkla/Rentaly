using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class MailManager : IMailService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MailManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SendReservationApprovalMailAsync(Rental rental)
        {
            string templatePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmailTemplates", "ReservationApproval.html");
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "discount-coupon.png");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Email şablonu bulunamadı!", templatePath);

            string mailBody = await File.ReadAllTextAsync(templatePath);

            mailBody = mailBody
                .Replace("{CustomerName}", $"{rental.Name} {rental.Surname}")
                .Replace("{CarInfo}", $"{rental.Car?.Brand?.BrandName} {rental.Car?.CarModel?.ModelName}")
                .Replace("{PickupDate}", rental.PickupDate?.ToString("dd.MM.yyyy") ?? "-")
                .Replace("{ReturnDate}", rental.ReturnDate?.ToString("dd.MM.yyyy") ?? "-");

            var fromAddress = new MailAddress("beyzailetisimapp@gmail.com", "Rentaly");
            var toAddress = new MailAddress(rental.Email);

            using var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("beyzailetisimapp@gmail.com", "cuhnsotkrdfpxerh");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Rezervasyon Onayı - Rentaly",
                IsBodyHtml = true
            };

            if (File.Exists(imagePath))
            {
                var res = new LinkedResource(imagePath, "image/png") { ContentId = "DiscountCoupon" };
                var htmlView = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                htmlView.LinkedResources.Add(res);
                message.AlternateViews.Add(htmlView);
            }
            else
            {
                message.Body = mailBody;
            }

            await smtp.SendMailAsync(message);
        }
    }
}