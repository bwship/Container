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
            //the first step should be to fill the container that is closer to the number of gallons to find
            if (Math.Abs(cp.gallonsToFind - cp.container1.capacity) <= Math.Abs(cp.gallonsToFind - cp.container2.capacity))
            {
                //fill the first container
                cp.container1.fill();
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_FILL));
            }
            else
            {
                //fill the second container
                cp.container2.fill();
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_FILL));
            }

            while (true)
            {
                if (cp.container1.isFull())
                {
                    if (cp.container2.isEmpty())
                    {
                        cp.container1.transfer(ref cp.container2);
                        cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_TRANSFER_TO_CONTAINER_2));
                    }
                    else
                    {
                        //if container 2 would become filled, then dump it, otherwise transfer it
                        if (cp.container1.gallons + cp.container2.gallons >= cp.container2.capacity)
                        {
                            cp.container1.dump();
                            cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_DUMP));
                        }
                        else
                        {
                            cp.container1.transfer(ref cp.container2);
                            cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_TRANSFER_TO_CONTAINER_2));
                        }
                    }
                }
                else if (cp.container2.isFull())
                {
                    if (cp.container1.isEmpty())
                    {
                        cp.container2.transfer(ref cp.container1);
                        cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_TRANSFER_TO_CONTAINER_1));
                    }
                    else
                    {
                        //if container 1 would become filled, then dump it, otherwise transfer it
                        if (cp.container1.gallons + cp.container2.gallons >= cp.container1.capacity)
                        {
                            cp.container2.dump();
                            cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_DUMP));
                        }
                        else
                        {
                            cp.container2.transfer(ref cp.container1);
                            cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_TRANSFER_TO_CONTAINER_1));
                        }
                    }

                }
                else if (cp.container1.isEmpty())
                {
                    cp.container1.fill();
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_FILL));
                }
                else if (cp.container2.isEmpty())
                {
                    cp.container2.fill();
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_FILL));
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

        public bool IsValid(ref containerProcessor cp)
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
                            step = (cp.containerSteps == null || cp.containerSteps.Count == 0) ? 1 : cp.containerSteps.Max(m => m.step) + 1,
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