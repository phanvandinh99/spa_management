using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class KhuyenMai
    {
        public int IdKhuyenMai { get; set; }

        [Required(ErrorMessage = "Mã khuyến mãi là bắt buộc")]
        [StringLength(50, ErrorMessage = "Mã khuyến mãi không được vượt quá 50 ký tự")]
        [Display(Name = "Mã khuyến mãi")]
        public string MaKhuyenMai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; } = string.Empty;

        [Display(Name = "Phần trăm giảm giá")]
        [Range(0, 100, ErrorMessage = "Phần trăm giảm giá phải từ 0-100")]
        public decimal? PhanTramGiamGia { get; set; }

        [Display(Name = "Số tiền giảm giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền giảm giá phải lớn hơn hoặc bằng 0")]
        public decimal? SoTienGiamGia { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime NgayKetThuc { get; set; }

        [Display(Name = "Áp dụng cho")]
        public string ApDungCho { get; set; } = "TatCa"; // SanPham, DichVu, TatCa

        // Navigation properties
        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
} 