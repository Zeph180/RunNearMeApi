using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

public class PostController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}