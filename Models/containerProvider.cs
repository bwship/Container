using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Container.Models
{
    public class containerProvider
    {
        public bool calculateContainer(ref containerProcessor cp)
        {
            cp.containerSteps = new List<containerStep>();

            //if gallons are 0, then there are no steps required
            if (cp.gallonsToFind == 0)
            {
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.GALLONS_TO_FIND_EQUALS_ZERO));
                return false;
            }

            //check that the container and gallons is valid
            if (IsValid(ref cp))
            {
                //fill the first container
                cp.container1.fill();
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_FILL));

                while (true)
                {
                    if (cp.container1.isFull() && cp.container2.isEmpty())
                    {
                        //container 1 is full and container 2 is empty then transfer
                        cp.container1.transfer(ref cp.container2);
                        cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_TRANSFER_TO_CONTAINER_2));
                    }
                    else if (cp.container2.isFull() && cp.container1.isEmpty())
                    {
                        //if the container 2 is full and container 1 is empty, then transfer
                        cp.container2.transfer(ref cp.container1);
                        cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_TRANSFER_TO_CONTAINER_1));
                    }
                    else
                    {
                        if (cp.container1.isFull() && !cp.container2.isEmpty())
                        {
                            //container 1 is full and container 2 is not empty, so dump container 1
                            cp.container1.dump();
                            cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_DUMP));
                        }
                        else if (cp.container2.isFull() && !cp.container1.isEmpty())
                        {
                            //container 2 is full and contianer 1 is not empty, so dump container 2
                            cp.container2.dump();
                            cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_DUMP));
                        }
                    }

                    if (cp.container1.gallons == cp.gallonsToFind)
                    {
                        cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_FOUND));
                        return true;
                    }
                    else if (cp.container2.gallons == cp.gallonsToFind)
                    {
                        cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_FOUND));
                        return true;
                    }
                }
            }
        }

        private bool IsValid(ref containerProcessor cp)
        {
            bool isValid = true;

                        //check that gallons to find is less than or equal to container 1 plus container 2's capacity
            if (cp.gallonsToFind > cp.container1.capacity + cp.container2.capacity)
            {
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.ERROR_GALLONS_TO_FIND_MUST_BE_LESS_THAN_OR_EQUAL_TO_CONTAINER_1_PLUS_CONTAINER_2));
                isValid = false;
            }

            if (cp.container1.capacity == cp.container2.capacity)
            {
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.ERROR_CONTAINER_1_AND_CONTAINER_2_MUST_BE_DIFFERENT));
                isValid = false;
            }

            if (!Coprime(cp.container1.capacity, cp.container2.capacity))
            {
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.ERROR_CONTAINER_1_AND_CONTAINER_2_SHARE_PRIMES));
                isValid = false;
            }

            return isValid;
        }

        private containerStep addStep(containerProcessor cp, string message)
        {
            return new containerStep
                        {
                            step = (cp.containerSteps == null) ? 0 : cp.containerSteps.Max(m => m.step) + 1,
                            stepDescription = message
                        };
        }

        /// <summary>
        /// Get the Greatest Common Denominator of two numbers
        /// </summary>
        private static int GetGCDByModulus(int value1, int value2)
        {
            while (value1 != 0 && value2 != 0)
            {
                if (value1 > value2)
                    value1 %= value2;
                else
                    value2 %= value1;
            }
            return Math.Max(value1, value2);
        }

        private static bool Coprime(int value1, int value2)
        {
            return GetGCDByModulus(value1, value2) == 1;
        }
    }
}