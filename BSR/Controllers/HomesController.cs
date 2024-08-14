using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BSR.Controllers;

public class HomesController: Controller
{
    private readonly HomeService _homeService;
    private readonly AddressService _addressService;

    public HomesController(HomeService homeService, AddressService addressService)
    {
        _homeService = homeService;
        _addressService = addressService;
    }

    public async Task<IActionResult> GetCities(string state)
    {
        var cities = await _addressService.GetCitiesInState(state);
        return Ok(cities);
    }

    [HttpGet]
    public async Task<IActionResult> AddHomeView()
    {
        var statesResult = await _addressService.GetAmericanStates();

        var addHomeViewModel = new AddHomeViewModel
        {
            States = statesResult,
            Cities = new List<string>()
        };

        return View(addHomeViewModel);
    }

    public async Task<IActionResult> Index(int? minPrice, int? maxPrice, int? minArea, int? maxArea)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var homesViewModel = new HomesViewModel();

        try
        {
            // Initially, get all homes
            var homes = _homeService.GetHomes();

            // Apply the price range filter if values are provided
            if (minPrice.HasValue)
            {
                homes = homes.Where(h => h.Price >= minPrice.Value).ToList();
            }
            if (maxPrice.HasValue)
            {
                homes = homes.Where(h => h.Price <= maxPrice.Value).ToList();
            }

            // Apply the area range filter if values are provided
            if (minArea.HasValue)
            {
                homes = homes.Where(h => h.Area >= minArea.Value).ToList();
            }
            if (maxArea.HasValue)
            {
                homes = homes.Where(h => h.Area <= maxArea.Value).ToList();
            }

            homesViewModel.Homes = homes;
            ViewBag.HomesCount = homes.Count;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error fetching homes from the database: {ex.Message}";
        }

        // Pass the filter values back to the view to retain them
        homesViewModel.MinPrice = minPrice;
        homesViewModel.MaxPrice = maxPrice;
        homesViewModel.MinArea = minArea;
        homesViewModel.MaxArea = maxArea;

        stopwatch.Stop();

        ViewBag.LoadTestTime = stopwatch.Elapsed.TotalSeconds.ToString("F4"); // Return the elapsed time in milliseconds

        return View(homesViewModel);
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
        }
        catch (Exception ex)
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
        }
        catch (Exception ex)
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
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting home: {ex.Message}";
            return BadRequest(new { message = ex.Message });
        }
    }
}
