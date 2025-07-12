using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class DanhGia
    {
        public int IdDanhGia { get; set; }

        [Required(ErrorMessage = "Khách hàng là bắt buộc")]
        [Display(Name = "Khách hàng")]
        public int IdKhachHang { get; set; }

        [Display(Name = "Sản phẩm")]
        public int? IdSanPham { get; set; }

        [Display(Name = "Dịch vụ")]
        public int? IdDichVu { get; set; }

        [Required(ErrorMessage = "Số sao là bắt buộc")]
        [Range(1, 5, ErrorMessage = "Số sao phải từ 1-5")]
        [Display(Name = "Số sao")]
        public int SoSao { get; set; }

        [Display(Name = "Bình luận")]
        public string? BinhLuan { get; set; }

        [Display(Name = "Ngày đánh giá")]
        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual SanPham? SanPham { get; set; }
        public virtual DichVu? DichVu { get; set; }
    }
} 