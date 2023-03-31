using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Controllers {
    public class HomeController : Controller {
        
        public HomeController()
        {
            
        }

        public ActionResult Index(){
            return View();
        }
    }
}