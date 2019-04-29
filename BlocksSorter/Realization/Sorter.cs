using BlocksSorter.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksSorter.Realization
{
    internal static class Sorter
    {
        /// <summary>
        /// Для начала необходимо найти такой блок, в котором отправная точка не будет являться конечной ни в одном из других блоков.
        /// Добавим его в список, затем на каждом шаге будем искать такой блок, у которого отправная точка соответствует конечной точке найденного блока.
        /// Сделаем это n-1 раз, поскольку первый блок уже был найден.
        /// Решение "в лоб", сложность алгоритма O(n^2) - нахождение первого блока методом GetStartBlock() самое тяжелое.
        /// </summary>
        internal static IList<IBlock> Sort(this IList<IBlock> blocks)
        {
            var sortingBlocks = new List<IBlock>();

            var firstBlock = blocks.GetStartBlock();
            sortingBlocks.Add(firstBlock);

            IBlock nextBlock = firstBlock;
            for (int i = 0; i < blocks.Count - 1; i++)
            {
                nextBlock = blocks.FirstOrDefault(b => nextBlock.EndPoint == b.StartPoint);
                if (nextBlock != null)
                {
                    sortingBlocks.Add(nextBlock);
                }
                else
                {
                    throw new ArgumentException("Blocks list contain duplicates!");
                }
            }

            return sortingBlocks;
        }

        /// <summary>
        /// Метод нахождения стартового блока. Операция выполняется за O(n^2).
        /// Если нашелся такой блок, в начальную точку которого не ведёт ни один блок, то возвращаем его, иначе - имеем цикл(контур).
        /// </summary>
        private static IBlock GetStartBlock(this IList<IBlock> blocks)
        {
            var endPoints = blocks.Select(b => b.EndPoint);
            var startBlock = blocks.FirstOrDefault(b => !endPoints.Contains(b.StartPoint));

            if (startBlock == null)
                throw new ArgumentException("Blocks cannot be sorted! Blocks list has a cycle!");

            return startBlock;
        }
    }
}
