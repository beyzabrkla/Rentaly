namespace Rentaly.EntityLayer.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? IconUrl { get; set; }
        public List<Car> Cars { get; set; }
    }
}
