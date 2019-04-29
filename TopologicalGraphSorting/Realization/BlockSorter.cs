using System;
using System.Collections.Generic;
using System.Linq;
using TopologicalGraphSorting.Contracts;
using TopologicalGraphSorting.Realization.Graph;

namespace BlocksSorter.Realization
{
    public class BlockSorter : IBlocksSorter
    {
        public IList<IBlock> Sort(IList<IBlock> blocks)
        {
            if (blocks == null)
                throw new ArgumentNullException("Blocks is null");

            if (blocks.Any())
            {
                var graph = new Graph(blocks);
                return graph.TarjanAlgorithm();

                //Использование алгоритма Кана
                //return graph.KahnAlgorithm();
            }
            return blocks;
        }
    }
}
