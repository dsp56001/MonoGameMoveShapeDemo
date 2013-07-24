using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShapeConsoleClient
{
    public class ShapeModel
    {
        // We declare Left and Top as lowercase with 
        // JsonProperty to sync the client and server models

        public double Left { get; set; }


        public double Top { get; set; }

    }
}
