using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class LichLamViec
    {
        public int IdLichLamViec { get; set; }

        [Required(ErrorMessage = "Nhân viên là bắt buộc")]
        [Display(Name = "Nhân viên")]
        public int IdNhanVien { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc")]
        [Display(Name = "Thời gian bắt đầu")]
        [DataType(DataType.DateTime)]
        public DateTime ThoiGianBatDau { get; set; }

        [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc")]
        [Display(Name = "Thời gian kết thúc")]
        [DataType(DataType.DateTime)]
        public DateTime ThoiGianKetThuc { get; set; }

        // Navigation properties
        public virtual NhanVien NhanVien { get; set; } = null!;
    }
} 