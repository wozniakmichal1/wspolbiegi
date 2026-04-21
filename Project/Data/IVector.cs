using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IVector
    {
        double X { get; }
        double Y { get; }

        double VelocityX { get; }
        double VelocityY { get; }
    }
}
