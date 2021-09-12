using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR_System_v2.Utils
{
    public class IoUtils
    {
        public static String GetMainDirectory()
        {
            String path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "/ECR";



            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
