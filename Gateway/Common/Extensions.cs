using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Common
{
    public class Extensions
    {
        public string AppendTimeStamp(string file, string format)
        {
            try
            {
                return string.Concat(
                    Path.GetFileNameWithoutExtension(file),
                    DateTime.Now.ToString(format),
                    Path.GetExtension(file));
            }
            catch (Exception e) { throw e; }
        }
    }
}
