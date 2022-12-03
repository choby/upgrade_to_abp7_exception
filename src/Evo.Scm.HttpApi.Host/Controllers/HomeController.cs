using Microsoft.AspNetCore.Mvc;
using NetCasbin;
using Volo.Abp.AspNetCore.Mvc;

namespace Evo.Scm.Controllers;

public class HomeController : AbpController
{
    public HomeController()
    {
      
    }

    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }
}
