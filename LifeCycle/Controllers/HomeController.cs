using LifeCycle.Models;
using LifeCycle.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LifeCycle.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICsvImportService _csvImportService;

        public HomeController(ICsvImportService csvImportService)
        {
            _csvImportService = csvImportService;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportCSV(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["Error"] = "Bitte wählen Sie eine CSV Datei aus.";
            }

            try
            {
                using var stream = csvFile.OpenReadStream();
                var result = await _csvImportService.ImportCsvDataAsync(stream);

                if (result.Success)
                {
                    TempData["Success"] = $"CSV Datei erfolgreich importiert. Anzahl der importierten Datensätze: {result.TImportedCount}";

                }
                else
                {
                    TempData["Error"] = $"Fehler beim Importieren der CSV Datei. Fehler: {string.Join(", ", result.ErrorMessages)}";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Fehler beim Importieren der CSV Datei. Fehler: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
