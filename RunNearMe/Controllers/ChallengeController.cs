using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

public class ChallengeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}