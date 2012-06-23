using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Container.Models
{
    public enum moveType
    {
        fill,
        dump,
        transfer
    }

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
        public int capacity;

        //number of gallons in the current container
        [Display(Name = "Gallons")]
        public int gallons;

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

        public int transfer(ref container containerTransferInto)
        {
            //transfer the contents of the 
            containerTransferInto.gallons = (containerTransferInto.gallons + this.gallons > containerTransferInto.capacity) ? containerTransferInto.capacity : containerTransferInto.gallons + this.gallons;
            
            //remove the gallons from this container
            this.gallons = 0;

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