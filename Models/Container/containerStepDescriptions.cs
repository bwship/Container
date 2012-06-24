using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Container.Models.Container
{
    internal static class containerStepDescriptions
    {
        //Error or Warning Messages
        internal const string ERROR_GALLONS_TO_FIND_MUST_BE_LESS_THAN_CONTAINER_1_PLUS_CONTAINER_2 = "Gallons to find must be less than first container plus second container";
        internal const string ERROR_CONTAINER_1_AND_CONTAINER_2_MUST_BE_DIFFERENT = "First container and second container must be different";
        internal const string ERROR_CONTAINER_1_AND_CONTAINER_2_SHARE_PRIMES = "First container and second container can not share prime numbers";

        //Initial Message
        internal const string INITIAL_STEP = "Both containers are empty";

        //Fill, Dump, Transfer Messages
        internal const string CONTAINER_1_FILL = "Filled first container";
        internal const string CONTAINER_1_DUMP = "Dumped first container";
        internal const string CONTAINER_1_TRANSFER_TO_CONTAINER_2 = "Transferred first container to second container";

        internal const string CONTAINER_2_FILL = "Filled second container";
        internal const string CONTAINER_2_DUMP = "Dumped second container";
        internal const string CONTAINER_2_TRANSFER_TO_CONTAINER_1 = "Transferred second container to first container";

        //Found Messages
        internal const string CONTAINER_NOT_FOUND = "Cannot find the number of gallons.  Sorry.";
        internal const string CONTAINER_1_FOUND = "First container has the correct number of gallons.";
        internal const string CONTAINER_2_FOUND = "Second container has the correct number of gallons.";
        internal const string CONTAINER_1_PLUS_2_FOUND = "First container plus second container has the correct number of gallons.";
    }
}
