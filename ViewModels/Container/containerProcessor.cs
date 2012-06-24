using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Container.Models.Container;

namespace Container.ViewModels.Container
{
    public class containerProcessor
    {
        public containerProcessor()
        {
            container1 = new container();
            container2 = new container();
            containerSteps = new List<containerStep>();
            containerSummary = new containerSummary();
        }

        public container container1 { get; set; }

        public container container2 { get; set; }
        
        [Display(Name = "Gallons To Find")]        
        public int gallonsToFind { get; set; }

        public List<containerStep> containerSteps { get; set; }

        public containerSummary containerSummary { get; set; }
    }
}
