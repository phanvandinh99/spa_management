using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class TinNhanHoTro
    {
        public int IdTinNhan { get; set; }

        [Required(ErrorMessage = "Người gửi là bắt buộc")]
        [Display(Name = "Người gửi")]
        public int IdNguoiGui { get; set; }

        [Required(ErrorMessage = "Người nhận là bắt buộc")]
        [Display(Name = "Người nhận")]
        public int IdNguoiNhan { get; set; }

        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        [Display(Name = "Nội dung")]
        public string NoiDung { get; set; } = string.Empty;

        [Display(Name = "Thời gian gửi")]
        public DateTime ThoiGianGui { get; set; } = DateTime.Now;

        [Display(Name = "Đã xem")]
        public bool DaXem { get; set; } = false;

        // Navigation properties
        public virtual TaiKhoan? NguoiGui { get; set; }
        public virtual TaiKhoan? NguoiNhan { get; set; }
    }
} 