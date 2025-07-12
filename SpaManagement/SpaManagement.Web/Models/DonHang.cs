using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class DonHang
    {
        public int IdDonHang { get; set; }

        [Required(ErrorMessage = "Khách hàng là bắt buộc")]
        [Display(Name = "Khách hàng")]
        public int IdKhachHang { get; set; }

        [Display(Name = "Nhân viên")]
        public int? IdNhanVien { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public DateTime NgayDatHang { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Tổng tiền là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Tổng tiền")]
        public decimal TongTien { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "ChoThanhToan"; // ChoThanhToan, DangXuLy, DangGiao, DaGiao, DaHuy

        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChiGiaoHang { get; set; } = string.Empty;

        [Display(Name = "Khuyến mãi")]
        public int? IdKhuyenMai { get; set; }

        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual NhanVien? NhanVien { get; set; }
        public virtual KhuyenMai? KhuyenMai { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    }
} 