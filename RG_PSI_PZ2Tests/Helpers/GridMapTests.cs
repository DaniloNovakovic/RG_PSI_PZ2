using NUnit.Framework;

namespace RG_PSI_PZ2.Helpers.Tests
{
    [TestFixture()]
    public class GridMapTests
    {
        [TestCase(10, 2)]
        public void ConstructorParamsValid_PropertiesAreValid(int numRows, int numCols)
        {
            var map = new GridMap(numRows, numCols);
            Assert.That(map.NumRows, Is.EqualTo(numRows));
            Assert.That(map.NumCols, Is.EqualTo(numCols));
        }

        [Test]
        public void CellAdded_GetReturnsAddedValue()
        {
            var cell1 = new GridMapCell();
            var cell2 = new GridMapCell();
            var map = new GridMap(4, 5);

            map.Add(3, 4, cell1);
            map.Add(0, 0, cell2);

            Assert.That(cell1, Is.EqualTo(map.Get(3, 4)));
            Assert.That(cell2, Is.EqualTo(map.Get(0, 0)));
        }

        [Test]
        public void CellDeleted_GetReturnsNull()
        {
            var cell1 = new GridMapCell();
            var map = new GridMap(4, 5);

            map.Add(3, 4, cell1);
            map.Delete(3, 4);

            Assert.That(map.Get(3, 4), Is.Null);
        }
    }
}