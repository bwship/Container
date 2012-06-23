using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Container.Models
{
    public class containerProcessor
    {
        public container container1;

        public container container2;

        [Display(Name = "Gallons")]
        public int gallonsToFind;

        public List<containerStep> containerSteps;
    }
}