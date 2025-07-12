using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class ThanhToan
    {
        public int IdThanhToan { get; set; }

        [Display(Name = "Đơn hàng")]
        public int? IdDonHang { get; set; }

        [Display(Name = "Lịch hẹn")]
        public int? IdLichHen { get; set; }

        [Required(ErrorMessage = "Số tiền là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Số tiền")]
        public decimal SoTien { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        [StringLength(100, ErrorMessage = "Phương thức thanh toán không được vượt quá 100 ký tự")]
        public string? PhuongThucThanhToan { get; set; }

        [Display(Name = "Thời gian thanh toán")]
        public DateTime ThoiGianThanhToan { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "ChuaThanhToan"; // ChuaThanhToan, DaThanhToan, ThatBai

        [Display(Name = "Mã giao dịch")]
        [StringLength(255, ErrorMessage = "Mã giao dịch không được vượt quá 255 ký tự")]
        public string? MaGiaoDich { get; set; }

        // Navigation properties
        public virtual DonHang? DonHang { get; set; }
        public virtual LichHen? LichHen { get; set; }
    }
} 