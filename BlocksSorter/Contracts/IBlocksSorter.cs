using System.Collections.Generic;

namespace BlocksSorter.Contracts
{
    public interface IBlocksSorter
    {
        /// <summary>
        /// <see cref="Метод сортировки блоков">Sort</see>.
        /// </summary>
        /// <param name="blocks">Входной список блоков.</param>
        /// <returns>Список отсортированных блоков.</returns>
        IList<IBlock> Sort(IList<IBlock> blocks);
    }
}
