using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class DichVu
    {
        public int IdDichVu { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tên dịch vụ không được vượt quá 255 ký tự")]
        [Display(Name = "Tên dịch vụ")]
        public string TenDichVu { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Giá dịch vụ là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá (VNĐ)")]
        public decimal Gia { get; set; }

        [Display(Name = "Thời lượng (phút)")]
        [Range(1, 480, ErrorMessage = "Thời lượng phải từ 1-480 phút")]
        public int? ThoiLuongPhut { get; set; }

        [Display(Name = "Danh mục")]
        public int? IdDanhMuc { get; set; }

        // Navigation properties
        public virtual DanhMucDichVu? DanhMucDichVu { get; set; }
        public virtual ICollection<LichHen> LichHens { get; set; } = new List<LichHen>();
        public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
    }
} 