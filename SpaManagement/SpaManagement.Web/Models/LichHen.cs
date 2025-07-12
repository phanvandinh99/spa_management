using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Web.Models
{
    public class LichHen
    {
        public int IdLichHen { get; set; }

        [Required(ErrorMessage = "Khách hàng là bắt buộc")]
        [Display(Name = "Khách hàng")]
        public int IdKhachHang { get; set; }

        [Required(ErrorMessage = "Dịch vụ là bắt buộc")]
        [Display(Name = "Dịch vụ")]
        public int IdDichVu { get; set; }

        [Display(Name = "Nhân viên")]
        public int? IdNhanVien { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc")]
        [Display(Name = "Thời gian bắt đầu")]
        [DataType(DataType.DateTime)]
        public DateTime ThoiGianBatDau { get; set; }

        [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc")]
        [Display(Name = "Thời gian kết thúc")]
        [DataType(DataType.DateTime)]
        public DateTime ThoiGianKetThuc { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "DaDat"; // DaDat, DaHoanThanh, DaHuy, KhongDen, ChoThanhToan

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual DichVu DichVu { get; set; } = null!;
        public virtual NhanVien? NhanVien { get; set; }
        public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    }
} 