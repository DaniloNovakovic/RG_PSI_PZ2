using NUnit.Framework;
using RG_PSI_PZ2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RG_PSI_PZ2.Helpers.Tests
{
    [TestFixture()]
    public class GridMapTests
    {
        [TestCase(10, 2)]
        public void GridMap_ConstructorParamsValid_PropertiesValid(int numRows, int numCols)
        {
            var map = new GridMap(numRows, numCols);
            Assert.That(map.NumRows, Is.EqualTo(numRows));
            Assert.That(map.NumCols, Is.EqualTo(numCols));
        }
    }
}