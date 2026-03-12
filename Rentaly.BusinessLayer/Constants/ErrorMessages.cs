using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Constants
{
    public class ErrorMessages
    {
        public const string CustomerNameRequired = "Müşteri adı boş bırakılamaz.";
        public const string CustomerSurnameRequired = "Müşteri soyadı boş bırakılamaz.";
        public const string InvalidEmail = "Geçerli bir email adresi giriniz.";
        public const string InvalidPhone = "Telefon numarası geçersiz.";
        public const string IdentityNumberInvalid = "Kimlik numarası 11 haneli olmalıdır.";
        public const string DrivingLicenseRequired = "Ehliyet numarası boş bırakılamaz.";
        public const string DrivingLicenseDateInvalid = "Ehliyet tarihi gelecekte olamaz.";

    }
}
