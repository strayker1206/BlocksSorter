using BlocksSorter.Realization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TopologicalGraphSorting.Contracts;
using TopologicalGraphSorting.Realization.Graph;

namespace TopologicalGraphSorting.Tests
{
    [TestClass]
    public class SortingTests
    {
        private void TestEquation(IList<IBlock> sortedBlocks, IList<IBlock> returnedBlocks)
        {
            Assert.AreEqual(sortedBlocks.Count, returnedBlocks.Count);

            for (int i = 0; i < sortedBlocks.Count; i++)
                Assert.AreEqual(sortedBlocks[i].ToString(), returnedBlocks[i].ToString());
        }

        [TestMethod]
        public void BlocksListIsNullTest()
        {
            IList<IBlock> blocks = null;

            IBlocksSorter blockSorter = new BlockSorter();
            Assert.ThrowsException<ArgumentNullException>(() => blockSorter.Sort(blocks));
        }

        [TestMethod]
        public void EmptyBlocksListTest()
        {
            IList<IBlock> blocks = new List<IBlock>();

            IBlocksSorter blockSorter = new BlockSorter();
            Assert.AreEqual(0, blockSorter.Sort(blocks).Count);
        }

        [TestMethod]
        public void SimpleTest()
        {
            IList<IBlock> blocks = new List<IBlock>();

            blocks.Add(new Block("Нижегородский", "Автозаводская"));
            blocks.Add(new Block("Южнопортовый", "Печатники"));
            blocks.Add(new Block("Текстильщики", "Нижегородский"));
            blocks.Add(new Block("Печатники", "Текстильщики"));

            IList<IBlock> sortedBlocks = new List<IBlock>();
            sortedBlocks.Add(new Block("Южнопортовый", "Печатники"));
            sortedBlocks.Add(new Block("Печатники", "Текстильщики"));
            sortedBlocks.Add(new Block("Текстильщики", "Нижегородский"));
            sortedBlocks.Add(new Block("Нижегородский", "Автозаводская"));

            IBlocksSorter blockSorter = new BlockSorter();
            var returnedBlocks = blockSorter.Sort(blocks);

            TestEquation(sortedBlocks, returnedBlocks);
        }

        [TestMethod]
        public void BlocksListWithCycleTest()
        {
            IList<IBlock> blocks = new List<IBlock>();

            blocks.Add(new Block("Нижегородский", "Автозаводская"));
            blocks.Add(new Block("Южнопортовый", "Печатники"));
            blocks.Add(new Block("Текстильщики", "Нижегородский"));
            blocks.Add(new Block("Печатники", "Текстильщики"));

            // Цикл
            blocks.Add(new Block("Автозаводская", "Южнопортовый"));

            IBlocksSorter blockSorter = new BlockSorter();
            Assert.ThrowsException<ArgumentException>(() => blockSorter.Sort(blocks));
        }

        [TestMethod]
        public void BlocksListWithDuplicatesTest()
        {
            IList<IBlock> blocks = new List<IBlock>();

            blocks.Add(new Block("Нижегородский", "Автозаводская"));
            blocks.Add(new Block("Южнопортовый", "Печатники"));
            blocks.Add(new Block("Текстильщики", "Нижегородский"));
            blocks.Add(new Block("Печатники", "Текстильщики"));

            //Дубликат
            blocks.Add(new Block("Текстильщики", "Нижегородский"));
            blocks.Add(new Block("Печатники", "Текстильщики"));

            IBlocksSorter blockSorter = new BlockSorter();
            Assert.ThrowsException<ArgumentException>(() => blockSorter.Sort(blocks));
        }
    }
}
