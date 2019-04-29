using System.Collections.Generic;

namespace TopologicalGraphSorting.Realization.Graph
{
    class Node
    {
        private readonly List<Block> incidentBlocks = new List<Block>();
        internal readonly string NodeName;

        internal Node(string name)
        {
            NodeName = name;
        }

        internal IEnumerable<Block> IncidentBlocks
        {
            get
            {
                foreach (var block in incidentBlocks)
                    yield return block;
            }
        }

        /// <summary>
        /// Соединяем узлы.
        /// 
        /// P.S. Небольшое нарушение инкапсуляции в рамках библиотеки: internal сеттеры у From и To, но закроем на это глаза :)
        /// Cоздавать отдельный класс Edge, а потом преобразовывать рёбра обратно в IBlock очень не хотелось, поэтому работаю с IBlock как с ребром графа.
        /// </summary>
        internal static void Connect(Block block, Node node1, Node node2)
        {
            block.From = node1;
            block.To = node2;
            node1.incidentBlocks.Add(block);
            node2.incidentBlocks.Add(block);
        }

        /// <summary>
        /// Удаление ребра происходит путём рассоединения узлов
        /// </summary>
        internal static void Disconnect(Block block)
        {
            block.From.incidentBlocks.Remove(block);
            block.To.incidentBlocks.Remove(block);
        }

        public override string ToString() => NodeName;
    }
}
