using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class ChiTietDonHang
    {
        public int IdChiTietDonHang { get; set; }

        [Required(ErrorMessage = "Đơn hàng là bắt buộc")]
        [Display(Name = "Đơn hàng")]
        public int IdDonHang { get; set; }

        [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
        [Display(Name = "Sản phẩm")]
        public int IdSanPham { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Đơn giá")]
        public decimal DonGia { get; set; }

        // Navigation properties
        public virtual DonHang DonHang { get; set; } = null!;
        public virtual SanPham SanPham { get; set; } = null!;
    }
} 