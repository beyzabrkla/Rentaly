namespace Rentaly.DTOLayer.RentalDTOs
{
    public class ResultRentalDTO : CreateRentalDTO
    {
        public int RentalId { get; set; }
        public string? Status { get; set; } // "Beklemede", "Onaylandı", "Tamamlandı", "İptal"
        public bool IsApproved { get; set; }

        // Araç Detayları (UI'da ToString() faciasını önlemek için)
        public string? CarPlate { get; set; }
        public string? BrandName { get; set; }
        public string? ModelName { get; set; }
        public string? CarImageUrl { get; set; }

        // Şube İsimleri (ID'lerin yanına isimleri de ekliyoruz)
        public string? PickupBranchName { get; set; }
        public string? ReturnBranchName { get; set; }

        // Operasyonel Veriler
        public DateTime CreatedDate { get; set; } // Rezervasyonun yapıldığı an
    }
}