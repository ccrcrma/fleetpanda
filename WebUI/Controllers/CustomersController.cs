using Microsoft.AspNetCore.Mvc;
using fleetpanda.dataaccess.Entities;
using fleetpanda.dataaccess.Repositories.Abstractions;

namespace fleetpanda.webui.Controllers;
public class CustomersController(ICustomerRepository repository) : Controller
{
    private readonly ICustomerRepository _customerRepository = repository;

    public async Task<IActionResult> Source()
    {
        var response = await _customerRepository.GetCustomersAtSourceAsync();
        if (response.Success)
        {
            var data = response.Data as IEnumerable<Customer>;
            ViewBag.Title = "Source Customers List";
            return View("Index", data);
        }
        return StatusCode(500);
    }

    public async Task<IActionResult> Target()
    {
        var response = await _customerRepository.GetCustomersAtTargetAsync();
        if (response.Success)
        {
            var data = response.Data as IEnumerable<Customer>;
            ViewBag.Title = "Destination Customers List";
            return View("Index",data);
        }
        return StatusCode(500);
    }

    public async Task<IActionResult> Sync()
    {
        var response = await _customerRepository.SyncCustomersToTargetAsync();
        if (response.Success)
        {
            TempData["Message"] = "Synced Successfully";
            return RedirectToAction("Target");
        }
        return StatusCode(500);
    }

}
