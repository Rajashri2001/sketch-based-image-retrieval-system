using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlibDotNet;

namespace Sketch_Based
{
    class LoadFeature
    {
        string name;

        public string Name { get => name; set => name = value; }
        public Matrix<float> Facediscriptor { get => facediscriptor; set => facediscriptor = value; }

        DlibDotNet.Matrix<float> facediscriptor = new DlibDotNet.Matrix<float>(128, 1);
    }
}
