namespace FlightReservation.MVC.Controllers;
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}
