using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gateway.Common
{
    public class Utilities
    {
        public static dynamic? ObjectNullCheck(dynamic data)
        {
            // Implement cleanup logic here
            return (data != null && data?.Count > 0) ? data?[0] : null;
        } 
    }
} 
