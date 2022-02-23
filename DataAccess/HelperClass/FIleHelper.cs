using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.HelperClass
{
    public  class FIleHelper
    {
        public static void CopyIfExist(string srcFileDir, string dstFileDir)
        {
            if(File.Exists(srcFileDir))
            using (var sw = new StreamWriter(dstFileDir,false))
            {
                byte[] file = File.ReadAllBytes(srcFileDir);
                sw.BaseStream.Write(file, 0, file.Length);
            }
        }
      
        public static string CopyFileToDir(string initialDirectory, string targetDestCopyDirectory,string fileName,string containedInFolder)
        {
            string[] files = Directory.GetFiles(initialDirectory, $"*{Path.GetExtension(fileName)}", SearchOption.AllDirectories);
               //.FirstOrDefault(x => x.Contains(containedInFolder));
            string destDir = string.Concat(targetDestCopyDirectory,$"{Path.DirectorySeparatorChar}", fileName);
            string file = files.FirstOrDefault(x=>x.Contains(containedInFolder) && x.Contains(fileName));
            if (!string.IsNullOrEmpty(file))
            {
                CopyIfExist(file, destDir);
            }
            return !string.IsNullOrEmpty(destDir) ? (File.Exists(destDir) ? destDir:null):null;
        }
    }
}
