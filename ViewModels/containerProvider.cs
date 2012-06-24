using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Container.Models.Container;
using Container.ViewModels.Container;

namespace Container.ViewModels
{
    public class containerProvider
    {
        /// <summary>
        /// Calculate the gallons wanted based on the two gallons passed in
        /// </summary>
        public bool calculateContainer(containerProcessor cp)
        {
            cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.INITIAL_STEP, containerStepType.error));

            //Step 1 - Decide which container to fill first
            bool fillFirstContainer = InitialFillContainer1(cp);

            //check if the gallons to find equals either of the two containers immediately
            if (cp.container1.gallons == cp.gallonsToFind)
            {
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_1_FOUND, containerStepType.message));
                return true;
            }
            else if (cp.container2.gallons == cp.gallonsToFind)
            {
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_2_FOUND, containerStepType.message));
                return true;
            }

            //Step 2 - loop through and continue to fill, dump, and transfer until the value is found 
            while (true)
            {
                bool stepDone = false;

                //these attempts are ordered in most likely to be correct to last likely, which helps to speed up the operation
                if (fillFirstContainer)
                {
                    //try operations on container 1 first
                    stepDone = Transfer1To2(cp);
                    stepDone = (!stepDone) ? DumpContainer2(cp) : stepDone;

                    stepDone = (!stepDone) ? Transfer2To1(cp) : stepDone;
                    stepDone = (!stepDone) ? DumpContainer1(cp) : stepDone;
                    stepDone = (!stepDone) ? FillContainer1(cp) : stepDone;
                    stepDone = (!stepDone) ? FillContainer2(cp) : stepDone;
                }
                else
                {
                    //try operations on container 2 first
                    stepDone = Transfer2To1(cp);
                    stepDone = (!stepDone) ? DumpContainer1(cp) : stepDone;
                    stepDone = (!stepDone) ? Transfer1To2(cp) : stepDone;
                    stepDone = (!stepDone) ? DumpContainer2(cp) : stepDone;
                    stepDone = (!stepDone) ? FillContainer2(cp) : stepDone;
                    stepDone = (!stepDone) ? FillContainer1(cp) : stepDone;
                }
                               
                //Step 3 - check if the value is found
                if (cp.gallonsToFind <= cp.container1.capacity || cp.gallonsToFind <= cp.container2.capacity)
                {
                    if (cp.container1.gallons == cp.gallonsToFind)
                    {
                        cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_1_FOUND, containerStepType.message));
                        return true;
                    }
                    else if (cp.container2.gallons == cp.gallonsToFind)
                    {
                        cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_2_FOUND, containerStepType.message));
                        return true;
                    }
                }
                else
                {
                    if (cp.container1.gallons + cp.container2.gallons == cp.gallonsToFind)
                    {
                        cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_1_PLUS_2_FOUND, containerStepType.message));
                        return true;
                    }
                }

                if (cp.containerSteps.Count > (cp.container1.capacity * cp.container2.capacity) + 2)
                {
                    cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_NOT_FOUND, containerStepType.message));
                    return false;
                }
            }           
        }

        /// <summary>
        /// Decides which jug to fill up first, and returns true if it was jug one
        /// </summary>
        private bool InitialFillContainer1(containerProcessor cp)
        {
            bool fillFirstContainer = false;

            if (cp.container1.capacity <= cp.gallonsToFind)
            {
                fillFirstContainer = (cp.gallonsToFind % cp.container1.capacity == 0) ? true : false;
            }
            else
            {
                fillFirstContainer = (cp.container1.capacity % cp.gallonsToFind == 0) ? true : false;
            }

            if (fillFirstContainer)
            {
                //fill the first container
                cp.container1.fill();
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_1_FILL));
            }
            else
            {
                //fill the second container
                cp.container2.fill();
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_2_FILL));
            }

            return fillFirstContainer;
        }

        /// <summary>
        /// Verifies if the input data is valid
        /// </summary>
        public bool IsValid(containerProcessor cp)
        {
            bool isValid = true;

            //check that gallons to find is less than or equal to container 1 plus container 2's capacity
            if (cp.gallonsToFind > cp.container1.capacity + cp.container2.capacity)
            {
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.ERROR_GALLONS_TO_FIND_MUST_BE_LESS_THAN_OR_EQUAL_TO_CONTAINER_1_PLUS_CONTAINER_2, containerStepType.error));
                isValid = false;
            }

            if (cp.container1.capacity == cp.container2.capacity)
            {
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.ERROR_CONTAINER_1_AND_CONTAINER_2_MUST_BE_DIFFERENT, containerStepType.error));
                isValid = false;
            }

            if (!Coprime(cp.container1.capacity, cp.container2.capacity))
            {
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.ERROR_CONTAINER_1_AND_CONTAINER_2_SHARE_PRIMES, containerStepType.error));
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Add the step to the step log
        /// </summary>
        private containerStep AddStep(containerProcessor cp, string message, containerStepType containerStepType = containerStepType.step)
        {
            return new containerStep
            {
                step = (cp.containerSteps == null || cp.containerSteps.Count == 0) ? 0 : cp.containerSteps.Max(m => m.step) + 1,
                stepDescription = message,
                container1Count = cp.container1.gallons,
                container2Count = cp.container2.gallons,
                containerStepType = containerStepType
            };
        }

        #region Test Attempts

        private bool Transfer1To2(containerProcessor cp)
        {
            bool stepDone = false;
            int tempContainer1 = 0;
            int tempContainer2 = 0;

            if (!cp.container1.isEmpty())
            {
                if (!cp.container1.isEmpty() && cp.container2.gallons + cp.container1.gallons > cp.container2.capacity)
                {
                    tempContainer1 = cp.container1.gallons - (cp.container2.capacity - cp.container2.gallons);
                    tempContainer2 = cp.container2.capacity;
                }
                else
                {
                    //otherwise pour all the contents into the new container, and the current container becomes empty
                    tempContainer1 = 0;
                    tempContainer2 = cp.container2.gallons + cp.container1.gallons;
                }

                if (IsValidStep(tempContainer1, tempContainer2, cp))
                {
                    cp.container1.transfer(cp.container2);
                    cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_1_TRANSFER_TO_CONTAINER_2));
                    stepDone = true;
                }
            }

            return stepDone;
        }

        private bool Transfer2To1(containerProcessor cp)
        {
            bool stepDone = false;
            int tempContainer1 = 0;
            int tempContainer2 = 0;

            if (!cp.container2.isEmpty())
            {
                //try to transfer container 2 to container 1
                if (cp.container1.gallons + cp.container2.gallons > cp.container1.capacity)
                {
                    tempContainer2 = cp.container2.gallons - (cp.container1.capacity - cp.container1.gallons);
                    tempContainer1 = cp.container1.capacity;
                }
                else
                {
                    //otherwise pour all the contents into the new container, and the current container becomes empty
                    tempContainer2 = 0;
                    tempContainer1 = cp.container1.gallons + cp.container2.gallons;
                }

                if (IsValidStep(tempContainer1, tempContainer2, cp))
                {
                    cp.container2.transfer(cp.container1);
                    cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_2_TRANSFER_TO_CONTAINER_1));
                    stepDone = true;
                }
            }

            return stepDone;
        }

        private bool DumpContainer1(containerProcessor cp)
        {
            bool stepDone = false;

            //try to dump container 1
            if (!cp.container1.isEmpty())
            {
                if (IsValidStep(0, cp.container2.gallons, cp))
                {
                    cp.container1.dump();
                    cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_1_DUMP));
                    stepDone = true;
                }
            }

            return stepDone;
        }

        private bool DumpContainer2(containerProcessor cp)
        {
            bool stepDone = false;

            //try to dump container 2
            if (!cp.container2.isEmpty())
            {
                if (IsValidStep(cp.container1.gallons, 0, cp))
                {
                    cp.container2.dump();
                    cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_2_DUMP));
                    stepDone = true;
                }
            }

            return stepDone;
        }

        private bool FillContainer1(containerProcessor cp)
        {
            bool stepDone = false;

            //try to fill container 1
            if (IsValidStep(cp.container1.capacity, cp.container2.gallons, cp))
            {
                cp.container1.fill();
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_1_FILL));
                stepDone = true;
            }

            return stepDone;
        }

        private bool FillContainer2(containerProcessor cp)
        {
            bool stepDone = false;

            //try to fill container 2
            if (IsValidStep(cp.container1.gallons, cp.container2.capacity, cp))
            {
                cp.container2.fill();
                cp.containerSteps.Add(AddStep(cp, containerStepDescriptions.CONTAINER_2_FILL));
                stepDone = true;
            }

            return stepDone;
        }

        private bool IsValidStep(int container1, int container2, containerProcessor cp)
        {
            return (!cp.containerSteps.Any(m => m.container1Count == container1 & m.container2Count == container2));
        }

        #endregion
                
        #region Coprime checks

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

        #endregion

    }
}
