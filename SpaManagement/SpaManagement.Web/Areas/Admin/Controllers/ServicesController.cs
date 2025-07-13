using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SpaManagement.Web.Models.EF;
using SpaManagement.Web.Models;

namespace SpaManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly SpaDbContext _context;
        public ServicesController(SpaDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var list = await _context.DichVu.Include(dv => dv.DanhMucDichVu).ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.DanhMucDichVu.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            // Debug: Log all form data
            System.Diagnostics.Debug.WriteLine("=== CREATE REQUEST DEBUG ===");
            foreach (var key in form.Keys)
            {
                System.Diagnostics.Debug.WriteLine($"{key}: {form[key]}");
            }
            
            // Extract form data
            var tenDichVu = form["TenDichVu"].ToString();
            var idDanhMucStr = form["IdDanhMuc"].ToString();
            var giaStr = form["Gia"].ToString();
            var thoiLuongStr = form["ThoiLuongPhut"].ToString();
            var moTa = form["MoTa"].ToString();
            
            System.Diagnostics.Debug.WriteLine($"TenDichVu: {tenDichVu}");
            System.Diagnostics.Debug.WriteLine($"IdDanhMuc: {idDanhMucStr}");
            System.Diagnostics.Debug.WriteLine($"Gia: {giaStr}");
            System.Diagnostics.Debug.WriteLine($"ThoiLuongPhut: {thoiLuongStr}");
            System.Diagnostics.Debug.WriteLine($"MoTa: {moTa}");
            
            // Validate required fields
            if (string.IsNullOrWhiteSpace(tenDichVu))
            {
                ModelState.AddModelError("TenDichVu", "Tên dịch vụ là bắt buộc");
            }
            
            if (!int.TryParse(idDanhMucStr, out int idDanhMuc) || idDanhMuc <= 0)
            {
                ModelState.AddModelError("IdDanhMuc", "Danh mục là bắt buộc");
            }
            
            if (!decimal.TryParse(giaStr, out decimal gia) || gia <= 0)
            {
                ModelState.AddModelError("Gia", "Giá phải lớn hơn 0");
            }
            
            if (!int.TryParse(thoiLuongStr, out int thoiLuong) || thoiLuong <= 0)
            {
                ModelState.AddModelError("ThoiLuongPhut", "Thời lượng phải lớn hơn 0");
            }
            
            System.Diagnostics.Debug.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            
            if (!ModelState.IsValid)
            {
                // Log validation errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                ViewBag.Categories = _context.DanhMucDichVu.ToList();
                return View();
            }

            try
            {
                var newService = new DichVu
                {
                    TenDichVu = tenDichVu,
                    IdDanhMuc = idDanhMuc,
                    Gia = gia,
                    ThoiLuongPhut = thoiLuong,
                    MoTa = moTa
                };
                
                System.Diagnostics.Debug.WriteLine("Adding service to context...");
                _context.DichVu.Add(newService);
                System.Diagnostics.Debug.WriteLine("Saving changes...");
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("Changes saved successfully!");
                
                TempData["Success"] = "Thêm dịch vụ thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Lỗi khi lưu: {ex.Message}");
                ViewBag.Categories = _context.DanhMucDichVu.ToList();
                return View();
            }
        }

        // Test action để kiểm tra routing
        [HttpPost]
        public IActionResult TestCreate()
        {
            System.Diagnostics.Debug.WriteLine("=== TEST CREATE ACTION CALLED ===");
            return Json(new { success = true, message = "Test action called successfully" });
        }

        // Test action đơn giản
        [HttpPost]
        public IActionResult SimpleTest()
        {
            System.Diagnostics.Debug.WriteLine("=== SIMPLE TEST ACTION CALLED ===");
            return Content("Simple test action called successfully");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dv = await _context.DichVu.FindAsync(id);
            if (dv == null) return NotFound();
            ViewBag.Categories = _context.DanhMucDichVu.ToList();
            return View(dv);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormCollection form)
        {
            // Debug: Log all form data
            System.Diagnostics.Debug.WriteLine("=== EDIT REQUEST DEBUG ===");
            foreach (var key in form.Keys)
            {
                System.Diagnostics.Debug.WriteLine($"{key}: {form[key]}");
            }
            
            // Extract form data
            var idDichVuStr = form["IdDichVu"].ToString();
            var tenDichVu = form["TenDichVu"].ToString();
            var idDanhMucStr = form["IdDanhMuc"].ToString();
            var giaStr = form["Gia"].ToString();
            var thoiLuongStr = form["ThoiLuongPhut"].ToString();
            var moTa = form["MoTa"].ToString();
            
            System.Diagnostics.Debug.WriteLine($"IdDichVu: {idDichVuStr}");
            System.Diagnostics.Debug.WriteLine($"TenDichVu: {tenDichVu}");
            System.Diagnostics.Debug.WriteLine($"IdDanhMuc: {idDanhMucStr}");
            System.Diagnostics.Debug.WriteLine($"Gia: {giaStr}");
            System.Diagnostics.Debug.WriteLine($"ThoiLuongPhut: {thoiLuongStr}");
            System.Diagnostics.Debug.WriteLine($"MoTa: {moTa}");
            
            // Validate required fields
            if (!int.TryParse(idDichVuStr, out int idDichVu) || idDichVu <= 0)
            {
                ModelState.AddModelError("IdDichVu", "ID dịch vụ không hợp lệ");
            }
            
            if (string.IsNullOrWhiteSpace(tenDichVu))
            {
                ModelState.AddModelError("TenDichVu", "Tên dịch vụ là bắt buộc");
            }
            
            if (!int.TryParse(idDanhMucStr, out int idDanhMuc) || idDanhMuc <= 0)
            {
                ModelState.AddModelError("IdDanhMuc", "Danh mục là bắt buộc");
            }
            
            if (!decimal.TryParse(giaStr, out decimal gia) || gia <= 0)
            {
                ModelState.AddModelError("Gia", "Giá phải lớn hơn 0");
            }
            
            if (!int.TryParse(thoiLuongStr, out int thoiLuong) || thoiLuong <= 0)
            {
                ModelState.AddModelError("ThoiLuongPhut", "Thời lượng phải lớn hơn 0");
            }
            
            System.Diagnostics.Debug.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            
            if (!ModelState.IsValid)
            {
                // Log validation errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                ViewBag.Categories = _context.DanhMucDichVu.ToList();
                return View();
            }

            try
            {
                var existingService = await _context.DichVu.FindAsync(idDichVu);
                if (existingService == null)
                {
                    return NotFound();
                }

                // Update only the properties that should be updated
                existingService.TenDichVu = tenDichVu;
                existingService.MoTa = moTa;
                existingService.Gia = gia;
                existingService.ThoiLuongPhut = thoiLuong;
                existingService.IdDanhMuc = idDanhMuc;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật dịch vụ thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Lỗi khi cập nhật: {ex.Message}");
                ViewBag.Categories = _context.DanhMucDichVu.ToList();
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dv = await _context.DichVu.FindAsync(id);
            if (dv != null)
            {
                _context.DichVu.Remove(dv);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa dịch vụ thành công!";
            }
            return RedirectToAction("Index");
        }
    }
} 