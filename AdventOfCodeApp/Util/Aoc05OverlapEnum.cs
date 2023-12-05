using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util
{
    internal enum Aoc05OverlapEnum
    {
        None, // null value eq
        Below, // Seed range is fully below range
        Above, // Seed range is fully above range
        Inner, // Seedrange is fully within Range
        Outer, // Seedrange is fully encompassing Range
        StartOut, // Seedrange starts outside, finish inside
        EndOut, // Seedrange starts inside finish outside
    }
}
