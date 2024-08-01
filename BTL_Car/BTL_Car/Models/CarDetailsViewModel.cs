namespace BTL_Car.Models
{
    public class CarDetailsViewModel
    {
        public Cars Car { get; set; }
        public IEnumerable<Comments> Comments { get; set; }
    }
}
