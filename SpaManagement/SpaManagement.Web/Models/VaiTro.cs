using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class VaiTro
    {
        public int IdVaiTro { get; set; }

        [Required(ErrorMessage = "Tên vai trò là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên vai trò không được vượt quá 50 ký tự")]
        [Display(Name = "Tên vai trò")]
        public string TenVaiTro { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
    }
} 