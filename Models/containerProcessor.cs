using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Container.Models
{
    public class containerProcessor
    {
        public containerProcessor()
        {
            this.container1 = new container();
            this.container2 = new container();
            this.containerSteps = new List<containerStep>();
        }

        public container container1;

        public container container2;

        [Display(Name = "Gallons")]
        public int gallonsToFind;

        public List<containerStep> containerSteps;
    }
}