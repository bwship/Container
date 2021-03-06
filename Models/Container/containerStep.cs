using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Container.ViewModels.Container
{
    public enum containerStepType
    {
        error,
        step,
        message
    }

    public class containerStep
    {
        #region Properties

        public int step { get; set; }

        public string stepDescription { get; set; }

        public int container1Count { get; set; }

        public int container2Count { get; set; }

        public containerStepType containerStepType { get; set; }

        #endregion
    }
}
