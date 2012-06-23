using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Container.Models
{
    internal static class containerStepDescriptions
    {
        //Error or Warning Messages
        internal const string GALLONS_TO_FIND_EQUALS_ZERO = "Gallons to find is set to zero.  No steps required.";
        internal const string ERROR_GALLONS_TO_FIND_MUST_BE_LESS_THAN_OR_EQUAL_TO_CONTAINER_1_PLUS_CONTAINER_2 = "Gallons to find must be less than or equal to Container 1 plus Container 2";
        internal const string ERROR_CONTAINER_1_AND_CONTAINER_2_MUST_BE_DIFFERENT = "Container and Container 2 must be different";
        internal const string ERROR_CONTAINER_1_AND_CONTAINER_2_SHARE_PRIMES = "Container 1 and Container 2 can not share prime numbers";

        
        //Fill, Dump, Transfer Messages
        internal const string CONTAINER_1_FILL = "Filled Container 1";
        internal const string CONTAINER_1_DUMP = "Dumped Container 1";
        internal const string CONTAINER_1_TRANSFER_TO_CONTAINER_2 = "Transferred Container 1 to Container 2";

        internal const string CONTAINER_2_FILL = "Filled Container 2";
        internal const string CONTAINER_2_DUMP = "Dumped Container 2";
        internal const string CONTAINER_2_TRANSFER_TO_CONTAINER_1 = "Transferred Container 2 to Container 1";

        //Found Messages
        internal const string CONTAINER_1_FOUND = "Container 1 has the correct number of gallons.";
        internal const string CONTAINER_2_FOUND = "Container 2 has the correct number of gallons.";
    }
}