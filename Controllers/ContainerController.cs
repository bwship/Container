using System.Linq;
using System.Web.Mvc;
using Container.Models.Container;
using Container.ViewModels;
using Container.ViewModels.Container;

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
            return View();
        }

        [HttpPost]
        public ActionResult ContainerProcessor(containerProcessor containerProcessor)
        {
            if (ModelState.IsValid)
            {
                if (_containerProvider.IsValid(containerProcessor))
                {
                    bool result = _containerProvider.calculateContainer(containerProcessor);

                    containerProcessor.containerSummary.success = result;
                    containerProcessor.containerSummary.steps = containerProcessor.containerSteps.Count(m => m.containerStepType == containerStepType.step);
                }
                else
                {                    
                    foreach (containerStep containerStep in containerProcessor.containerSteps)
                    {                                
                        switch (containerStep.stepDescription)
                        {
                            case containerStepDescriptions.ERROR_GALLONS_TO_FIND_MUST_BE_LESS_THAN_OR_EQUAL_TO_CONTAINER_1_PLUS_CONTAINER_2:
                                this.ModelState.AddModelError("gallonsToFind", containerStep.stepDescription);
                                break;
                            default:
                                this.ModelState.AddModelError("container1.capacity", containerStep.stepDescription);
                                break;
                        }                        
                    }

                    containerProcessor.containerSteps = null;
                }
            }

            return View(containerProcessor);
        }
    }
}
