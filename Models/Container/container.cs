using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Container.Models.Container
{
    public class container
    {
        #region Constuctors

        public container()
        {
            this.capacity = 0;
            this.gallons = 0;
        }

        public container(int capacity)
        {
            this.capacity = capacity;
            this.gallons = 0;
        }

        #endregion

        #region Properties

        //max number of gallons the container can hold
        [Display(Name = "Capacity")]
        [Range(2, 10000, ErrorMessage = "Must be a number between 2 and 10,000")]
        public int capacity { get; set; }

        //number of gallons in the current container
        [Display(Name = "Gallons")]
        public int gallons { get; set; }

        #endregion

        #region Methods

        public int fill()
        {
            this.gallons = this.capacity;
            return this.gallons;
        }

        public int dump()
        {
            this.gallons = 0;
            return this.gallons;
        }

        public int transfer(container containerTransferInto)
        {            
            if (containerTransferInto.gallons + this.gallons > containerTransferInto.capacity)
            {
                //if this container holds more gallons than the other, then only transfer the correct amount, and hte remainder stays in the current container
                this.gallons = this.gallons - (containerTransferInto.capacity - containerTransferInto.gallons);
                containerTransferInto.gallons = containerTransferInto.capacity;
            }                
            else
            {
                //otherwise pour all the contents into the new container, and the current container becomes empty
                containerTransferInto.gallons = containerTransferInto.gallons + this.gallons;
                this.gallons = 0;
            }
                       
            return this.gallons;
        }

        public bool isFull()
        {
            return (this.gallons == this.capacity) ? true : false;
        }

        public bool isEmpty()
        {
            return (this.gallons == 0) ? true : false;
        }

        #endregion
    }
}
