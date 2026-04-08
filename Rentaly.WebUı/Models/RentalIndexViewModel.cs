using Rentaly.DTOLayer.RentalDTOs;

namespace Rentaly.WebUI.Models
{
    public class RentalIndexViewModel
    {
        //Formdan toplayacağımız ve veritabanına göndereceğimiz kısım
        public CreateRentalDTO CreateRentalDto { get; set; }

        //Sadece sağ taraftaki "Özet Kartı"nı doldurmak için kullanacağımız kısım
        //ResultDTO mantığında
        public string? BrandName { get; set; }
        public string? ModelName { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public string? PlateNumber { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal DepositAmount { get; set; }
        public string? CurrentBranchName { get; set; }
    }
}
