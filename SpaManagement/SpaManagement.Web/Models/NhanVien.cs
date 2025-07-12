using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class NhanVien
    {
        public int IdNhanVien { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [StringLength(100, ErrorMessage = "Chức vụ không được vượt quá 100 ký tự")]
        [Display(Name = "Chức vụ")]
        public string? ChucVu { get; set; }

        [Display(Name = "Tài khoản")]
        public int? IdTaiKhoan { get; set; }

        // Navigation properties
        public virtual TaiKhoan? TaiKhoan { get; set; }
        public virtual ICollection<LichLamViec> LichLamViecs { get; set; } = new List<LichLamViec>();
        public virtual ICollection<LichHen> LichHens { get; set; } = new List<LichHen>();
        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
} 