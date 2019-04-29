using BlocksSorter.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

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
                return blocks.Sort();
            }
            return blocks;
        }
    }
}
