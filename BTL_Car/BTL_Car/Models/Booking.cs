using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_Car.Models
{
    [Table("tblBooking")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int booking_id { get; set; }
        public int user_booking_id { get; set; } // ID của người đặt xe
        public int car_booking_id { get; set; } // ID của xe
        public DateTime booking_date { get; set; } // Ngày đặt xe
        public DateTime rental_start_date { get; set; } // Ngày bắt đầu thuê
        public DateTime rental_end_date { get; set; } // Ngày kết thúc thuê
        public decimal total_cost { get; set; } // Tổng chi phí
        public string status_car { get; set; } // Trạng thái của xe (đã đặt, còn trống, v.v.)
    }
}
