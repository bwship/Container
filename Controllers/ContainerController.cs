﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Container.Models;

namespace Container.Controllers
{
    public class ContainerController : Controller
    {
        #region Variables
        
        containerProvider _containerProvider;

        #endregion

        #region Contructors

        public ContainerController()
        {
            _containerProvider = new containerProvider();
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContainerProcessor()
        {
            containerProcessor containerProcessor = new containerProcessor();
            
            return View();
        }

        [HttpPost]
        public ActionResult ContainerProcessor(containerProcessor containerProcessor)
        {
            containerProcessor.container1.capacity = 1;
            containerProcessor.container2.capacity = 10;
            containerProcessor.gallonsToFind = 4;

            if (_containerProvider.IsValid(ref containerProcessor))
            {
                _containerProvider.calculateContainer(ref containerProcessor);
            }

            return View(containerProcessor);
        }
    }
}
