using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.HelperClass
{
    public class DirectoryHelper
    {
        public static string GetNUpperDirectory(string basePath,int count)
        {
            string finalPath = "";
            int i = 0;
            while(i<count)
            {
                basePath = System.IO.Directory.GetParent(basePath).FullName;
                i++;
            }
            finalPath = basePath;
            return finalPath;
        }
    }
}
