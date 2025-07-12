using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class TaiKhoan
    {
        public int IdTaiKhoan { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên đăng nhập không được vượt quá 100 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(255, ErrorMessage = "Mật khẩu không được vượt quá 255 ký tự")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhauHash { get; set; } = string.Empty;

        [Display(Name = "Vai trò")]
        public int? IdVaiTro { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "HoatDong"; // HoatDong, Khoa

        // Navigation properties
        public virtual VaiTro? VaiTro { get; set; }
        public virtual KhachHang? KhachHang { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
        // TODO: Uncomment khi cần sử dụng TinNhanHoTro
        // public virtual ICollection<TinNhanHoTro> TinNhanGui { get; set; } = new List<TinNhanHoTro>();
        // public virtual ICollection<TinNhanHoTro> TinNhanNhan { get; set; } = new List<TinNhanHoTro>();
    }
} 