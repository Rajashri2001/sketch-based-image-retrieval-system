using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sketch_Based
{
    class Image
    {
        string ID;
        string imageUrl;
        string visible;
        string page;

        public string ID1 { get => ID; set => ID = value; }
        public string ImageUrl { get => imageUrl; set => imageUrl = value; }
        public string Visible { get => visible; set => visible = value; }
        public string Page { get => page; set => page = value; }
    }
}
