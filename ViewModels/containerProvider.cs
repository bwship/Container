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
        #region Calculator Methods

        /// <summary>
        /// Calculate the gallons wanted based on the two gallons passed in
        /// </summary>
        public bool calculateContainer(containerProcessor cp)
        {
            cp.containerSteps.Add(addStep(cp, containerStepDescriptions.INITIAL_STEP, containerStepType.error));

            //Step 1 - Decide which container to fill first
            bool fillFirstContainer = initialFillContainer1(cp);

            //check if the gallons to find equals either of the two containers immediately
            if (isValueFound(cp))
            {
                return true;
            } 
                        
            //Step 2 - loop through and continue to fill, dump, and transfer until the value is found 
            if (fillFirstContainer)
            {
                //these attempts are ordered in most likely to be correct to last likely for container 1 being filled first, which helps to speed up the operation
                while (true)
                {
                    bool stepDone = false;
                    
                    //try operations on container 1 first
                    stepDone = Transfer1To2(cp);
                    stepDone = (!stepDone) ? DumpContainer2(cp) : stepDone;
                    stepDone = (!stepDone) ? Transfer2To1(cp) : stepDone;
                    stepDone = (!stepDone) ? DumpContainer1(cp) : stepDone;
                    stepDone = (!stepDone) ? FillContainer1(cp) : stepDone;
                    stepDone = (!stepDone) ? FillContainer2(cp) : stepDone;

                    //Step 3 - check if the value is found
                    if (isValueFound(cp))
                        return true;

                    //check if all possible steps have been attempted
                    if (isTestExhausted(cp))
                        return false;
                }           
            }
            else
            {
                //these attempts are ordered in most likely to be correct to last likely for container 2 being filled first, which helps to speed up the operation
                while (true)
                {
                    bool stepDone = false;

                    //try operations on container 2 first
                    stepDone = Transfer2To1(cp);
                    stepDone = (!stepDone) ? DumpContainer1(cp) : stepDone;
                    stepDone = (!stepDone) ? Transfer1To2(cp) : stepDone;
                    stepDone = (!stepDone) ? DumpContainer2(cp) : stepDone;
                    stepDone = (!stepDone) ? FillContainer2(cp) : stepDone;
                    stepDone = (!stepDone) ? FillContainer1(cp) : stepDone;

                    //Step 3 - check if the value is found
                    if (isValueFound(cp))
                        return true;

                    //check if all possible steps have been attempted
                    if (isTestExhausted(cp))
                        return false;
                }           
            }            
        }

        /// <summary>
        /// Return summary information about the process run
        /// </summary>
        public bool summaryInformation(containerProcessor cp, bool result)
        {
            //set the success info
            cp.containerSummary.success = result;

            //set how many steps it took
            cp.containerSummary.steps = cp.containerSteps.Count(m => m.containerStepType == containerStepType.step);

            //set the last step information
            containerStep lastStep = cp.containerSteps.Last();
            cp.containerSummary.lastMessage = String.Format("{0} <br /> Container 1: {1:###,##0} <br /> Container 2: {2:###,##0}", lastStep.stepDescription, lastStep.container1Count, lastStep.container2Count);

            return true;
        }

        /// <summary>
        /// Verifies if the input data is valid
        /// </summary>
        public bool isValid(containerProcessor cp)
        {
            bool isValid = true;

            //check that gallons to find is less than or equal to container 1 plus container 2's capacity
            if (cp.gallonsToFind >= cp.container1.capacity + cp.container2.capacity)
            {
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.ERROR_GALLONS_TO_FIND_MUST_BE_LESS_THAN_CONTAINER_1_PLUS_CONTAINER_2, containerStepType.error));
                isValid = false;
            }

            if (cp.container1.capacity == cp.container2.capacity)
            {
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.ERROR_CONTAINER_1_AND_CONTAINER_2_MUST_BE_DIFFERENT, containerStepType.error));
                isValid = false;
            }

            if (!Coprime(cp.container1.capacity, cp.container2.capacity))
            {
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.ERROR_CONTAINER_1_AND_CONTAINER_2_SHARE_PRIMES, containerStepType.error));
                isValid = false;
            }

            return isValid;
        }

        #endregion

        #region Processing Helpers

        /// <summary>
        /// Decides which jug to fill up first, and returns true if it was jug one
        /// </summary>
        private bool initialFillContainer1(containerProcessor cp)
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
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_FILL));
            }
            else
            {
                //fill the second container
                cp.container2.fill();
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_FILL));
            }

            return fillFirstContainer;
        }
                
        /// <summary>
        /// Checks if all steps have already been tried
        /// </summary>
        private bool isTestExhausted(containerProcessor cp)
        {
            //check if every option is exhausted, and if so, exit with failure
            if (cp.containerSteps.Count > (cp.container1.capacity * cp.container2.capacity) + 3)
            {
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_NOT_FOUND, containerStepType.message));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the value to find is in one or both of the containers
        /// </summary>
        private bool isValueFound(containerProcessor cp)
        {
            if (cp.gallonsToFind <= cp.container1.capacity || cp.gallonsToFind <= cp.container2.capacity)
            {
                if (cp.container1.gallons == cp.gallonsToFind)
                {
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_FOUND, containerStepType.message));
                    return true;
                }
                else if (cp.container2.gallons == cp.gallonsToFind)
                {
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_FOUND, containerStepType.message));
                    return true;
                }
            }
            else
            {
                if (cp.container1.gallons + cp.container2.gallons == cp.gallonsToFind)
                {
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_PLUS_2_FOUND, containerStepType.message));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add the step to the step log
        /// </summary>
        private containerStep addStep(containerProcessor cp, string message, containerStepType containerStepType = containerStepType.step)
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

        #endregion

        #region Test Attempts

        /// <summary>
        /// Attempt to transfer container 1 into container 2
        /// </summary>
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
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_TRANSFER_TO_CONTAINER_2));
                    stepDone = true;
                }
            }

            return stepDone;
        }

        /// <summary>
        /// Attempt to transfer container 2 into container 1
        /// </summary>
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
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_TRANSFER_TO_CONTAINER_1));
                    stepDone = true;
                }
            }

            return stepDone;
        }

        /// <summary>
        /// Attempt to dump container 1
        /// </summary>
        private bool DumpContainer1(containerProcessor cp)
        {
            bool stepDone = false;

            //try to dump container 1
            if (!cp.container1.isEmpty())
            {
                if (IsValidStep(0, cp.container2.gallons, cp))
                {
                    cp.container1.dump();
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_DUMP));
                    stepDone = true;
                }
            }

            return stepDone;
        }

        /// <summary>
        /// Attempt to dump container 2
        /// </summary>
        private bool DumpContainer2(containerProcessor cp)
        {
            bool stepDone = false;

            //try to dump container 2
            if (!cp.container2.isEmpty())
            {
                if (IsValidStep(cp.container1.gallons, 0, cp))
                {
                    cp.container2.dump();
                    cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_DUMP));
                    stepDone = true;
                }
            }

            return stepDone;
        }

        /// <summary>
        /// Attempt to fill container 1
        /// </summary>
        private bool FillContainer1(containerProcessor cp)
        {
            bool stepDone = false;

            //try to fill container 1
            if (IsValidStep(cp.container1.capacity, cp.container2.gallons, cp))
            {
                cp.container1.fill();
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_1_FILL));
                stepDone = true;
            }

            return stepDone;
        }

        /// <summary>
        /// Attempt to fill container 2
        /// </summary>
        private bool FillContainer2(containerProcessor cp)
        {
            bool stepDone = false;

            //try to fill container 2
            if (IsValidStep(cp.container1.gallons, cp.container2.capacity, cp))
            {
                cp.container2.fill();
                cp.containerSteps.Add(addStep(cp, containerStepDescriptions.CONTAINER_2_FILL));
                stepDone = true;
            }

            return stepDone;
        }

        /// <summary>
        /// check if the attemp is valid
        /// </summary>
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
