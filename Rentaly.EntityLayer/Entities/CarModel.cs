namespace Rentaly.EntityLayer.Entities
{
    public class CarModel
    {
        public int CarModelId { get; set; }
        public string ModelName { get; set; }

        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; } // Modelin markası
    }
}