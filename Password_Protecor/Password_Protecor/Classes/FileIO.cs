using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hide_Your_Files_Inside_a_Picture
{
    public class FileIO
    {
        public string name { get; set; }
        public string path { get; set; }
        public string zipPath { get; set; }
        public string extension { get; set; }

        public FileIO()
        {

        }

        public FileIO(string path)
        {
            this.path = path;
            this.extension = System.IO.Path.GetExtension(path);
            this.name = System.IO.Path.GetFileName(path);
        }

        public override string ToString()
        {
            return String.Format("{0}", name);
        }
    }
}
