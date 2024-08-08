using Microsoft.AspNetCore.Mvc;
using RealEstateApplication.Models;
using RealEstateApplication.Services;

namespace RealEstateApplication.Controllers
{
    public class HomesController : Controller
    {
        private readonly HomeService _homeService;

        public HomesController(HomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index(int? minPrice, int? maxPrice, int? minArea, int?maxArea)
        {
            var homesViewModel = new HomesViewModel();

            try
            {
                var homes = _homeService.GetHomes();
                // filter homes by MinPrice and MaxPrice if they are provided
                if (minPrice.HasValue)
                {
                    homes = homes.Where(h => h.Price >= minPrice.Value).ToList();
                }
                if (maxPrice.HasValue)
                {
                    homes = homes.Where(h => h.Price <= maxPrice.Value).ToList();
                }
                // filter homes by MinArea and MaxArea if they are provided
                if (minArea.HasValue)
                {
                    homes = homes.Where(h => h.Area >= minArea.Value).ToList();
                }
                if (maxArea.HasValue)
                {
                    homes = homes.Where(h => h.Area <= maxArea.Value).ToList();
                }
                homesViewModel.Homes = homes;
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error getting homes from the database: {ex.Message}";
            }
            // pass the MinPrice and MaxPrice to the view so that the user can see the filter values
            homesViewModel.MinPrice = minPrice;
            homesViewModel.MaxPrice = maxPrice;
            // pass the MinArea and MaxArea to the view so that the user can see the filter values
            homesViewModel.MinArea = minArea;
            homesViewModel.MaxArea = maxArea;

            return View(homesViewModel);
        }

        [HttpGet]
        public IActionResult ClearFilters()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddHomeView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddHome(Home newHome)
        {
            if (!ModelState.IsValid)
            {
                return View("AddHomeView", newHome);
            }

            try
            {
                _homeService.AddHome(newHome);
                TempData["SuccessMessage"] = "Home added successfully!";
                return RedirectToAction("Index", "Homes");
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding home: {ex.Message}";
                return View("AddHomeView", newHome);
            }
        }

        [HttpGet]
        public IActionResult HomeDetailView(int id)
        {
            var home = _homeService.GetHomeById(id);
            return View(home);
        }

        [HttpPost]
        public IActionResult Update(Home updatedHome)
        {
            if (!ModelState.IsValid)
            {
                return View("HomeDetailView", updatedHome);
            }

            try
            {
                _homeService.UpdateHome(updatedHome);
                TempData["SuccessMessage"] = "Home updated successfully!";
                return RedirectToAction("Index", "Homes");
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating home: {ex.Message}";
                return View("HomeDetailView", updatedHome);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _homeService.DeleteHome(id);
                TempData["SuccessMessage"] = "Home deleted successfully!";
                return new OkResult();
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting home: {ex.Message}";
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
