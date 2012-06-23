using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Container.Models;

namespace Container.Controllers
{
    public class ContainerController : Controller
    {
        //
        // GET: /Container/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContainerProcessor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContainerProcessor(containerProcessor containerProcessor)
        {

            return View(containerProcessor);
        }
    }
}
