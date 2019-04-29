using BlocksSorter.Realization;
using System;
using System.Collections.Generic;
using TopologicalGraphSorting.Contracts;
using TopologicalGraphSorting.Realization.Graph;

namespace SorterClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<IBlock> blocks = new List<IBlock>();

            blocks.Add(new Block("Нижегородский", "Автозаводская"));
            blocks.Add(new Block("Южнопортовый", "Печатники"));
            blocks.Add(new Block("Текстильщики", "Нижегородский"));
            blocks.Add(new Block("Печатники", "Текстильщики"));

            //Дубликат
            //blocks.Add(new Block("Текстильщики", "Нижегородский"));
            //blocks.Add(new Block("Печатники", "Текстильщики"));

            //Цикл
            //blocks.Add(new Block("Автозаводская", "Южнопортовый"));

            IBlocksSorter blockSorter = new BlockSorter();
            var sortedBlocks = blockSorter.Sort(blocks);
            foreach (IBlock block in sortedBlocks)
            {
                Console.WriteLine(block.ToString());
            }

            Console.ReadKey();
        }
    }
}
