using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class SanPham
    {
        public int IdSanPham { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string TenSanPham { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá (VNĐ)")]
        public decimal Gia { get; set; }

        [Display(Name = "Số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int SoLuongTon { get; set; } = 0;

        [Display(Name = "Hình ảnh URL")]
        [StringLength(255, ErrorMessage = "URL hình ảnh không được vượt quá 255 ký tự")]
        public string? HinhAnhURL { get; set; }

        [Display(Name = "Danh mục")]
        public int? IdDanhMuc { get; set; }

        // Navigation properties
        public virtual DanhMucSanPham? DanhMucSanPham { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
    }
} 