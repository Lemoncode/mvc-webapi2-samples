using Shapes.Contracts;
using System;

namespace Shapes.Services
{
    public class AreaResolver : IAreaResolver
    {
        public double CircleArea(double radius)
        {
            return Math.PI * Math.Pow(radius, 2);
        }
    }
}
