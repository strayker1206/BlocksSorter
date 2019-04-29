using System;
using System.Collections.Generic;
using System.Linq;
using TopologicalGraphSorting.Contracts;

namespace TopologicalGraphSorting.Realization.Graph
{
    class Graph
    {
        private IList<Node> nodes = new List<Node>();
        internal IEnumerable<Node> Nodes
        {
            get
            {
                foreach (var n in nodes) yield return n;
            }
        }

        internal Graph(IEnumerable<IBlock> blocks)
        {
            InithializeGraph(blocks);
        }

        /// <summary>
        /// Если немного подумать, то становится ясно, что список блоков - это список рёбер ориентированного графа.
        /// Таким образом, вся задача сводится к топологической сортировке ориентированного графа без циклов (контуров).
        /// Для начала преобразуем список блоков в список узлов и соединим эти узлы согласно информации из блоков.
        /// </summary>
        private void InithializeGraph(IEnumerable<IBlock> blocks)
        {
            ISet<string> stations = new HashSet<string>();
            foreach (Block block in blocks)
            {
                var node1 = CreateNode(stations, block.StartPoint);
                var node2 = CreateNode(stations, block.EndPoint);
                Node.Connect(block, node1, node2);
            }
        }

        private Node CreateNode(ISet<string> stations, string stationName)
        {
            Node node;
            if (stations.Add(stationName))
            {
                node = new Node(stationName);
                nodes.Add(node);
            }
            else
            {
                node = nodes.FirstOrDefault(n => n.NodeName == stationName);
            }

            return node;
        }

        internal void Delete(Block block)
        {
            Node.Disconnect(block);
        }

        /// <summary>
        /// Алгоритм Кана - один из классичеких алгоритмов топологической сортировки, использующий обход в глубину.
        /// В классической версии, алгоритм сортирует узлы, но ничего не мешает сортировать рёбра.
        /// Исходим из леммы: Если в орграфе нет узла с нулевой степенью захода, то он содержит циклы и не может быть топологически отсортирован.
        /// Доказывать не буду, всё равно доказательство будет не из головы, а из интернета :)
        /// Итак, найдём узел с нулевой степенью захода, исключим его из графа и удалим все рёбра, которые связаны с этим узлом.
        /// Повторим в цикле до тех пор, пока узлов в графе не останется.
        /// 
        /// Недостаток данного алгоритма заключается в том, что приходится постоянно модифицировать граф, удаляя из него узлы и рёбра.
        /// Сложность алгоритма O(V+E), где V - узлы, E - рёбра графа.
        /// </summary>
        internal IList<IBlock> KahnAlgorithm()
        {
            var sortedBlocks = new List<IBlock>();
            var nodes = Nodes.ToList();
            while (nodes.Count != 0)
            {
                var nodeToDelete = nodes.FirstOrDefault(node =>
                                   !node.IncidentBlocks.Any(block => block.To == node));

                if (nodeToDelete == null)
                    throw new ArgumentException("Blocks cannot be sorted! Blocks list has a cycle!");

                nodes.Remove(nodeToDelete);

                var incidentBlocks = nodeToDelete.IncidentBlocks.ToList();
                if (incidentBlocks.Count() > 1)
                    throw new ArgumentException("Blocks list contain duplicates!");

                foreach (var block in incidentBlocks)
                {
                    sortedBlocks.Add(block);
                    Delete(block);
                }
            }
            return sortedBlocks;
        }

        #region Алгоритм Тарьяна
        /// <summary>
        /// Алгоритм Тарьяна так же основан на поиске в глубину. Однако ему не нужно искать узел с нулевой степенью захода.
        /// Изначально все узлы помечены белым цветом. Начинаем поиск в глубину из первого узла, помечаем его серым цветом и ищем дальше. 
        /// Если рёбер, выходящих из узла нет - помечаем его черным цветом и записываем его в список (алгоритм адаптирован под сортировку рёбер, поэтому записываем ребро). Фактически, это то же самое, что и удалить ребро из графа.
        /// 
        /// Рекурсивно продолжаем это до тех пор, пока "белых" узлов не останется.
        /// Если во время поиска натыкаемся на чёрную вершину (база рекурсии), то заканчиваем поиск. 
        /// Если натыкаемся на серую вершину - имеем цикл.
        /// В конце разворачиваем список блоков и возвращаем его.
        /// 
        /// Сложность алгоритма тоже линейная - O(V+E), где V - узлы, E - рёбра графа.
        /// 
        /// Кстати, алгоритм устойчив к дубликатам и просто схлопывает дубликаты, в отличие от алгоритма Кана, который продублирует рёбра в отсортированном списке.
        /// Но поскольку дубликатов обещали на вход не подавать, то нечестным клиентам библиотеки кидаю исключение :)
        /// </summary>
        internal IList<IBlock> TarjanAlgorithm()
        {
            var sortedBlocks = new List<IBlock>();
            var states = Nodes.ToDictionary(node => node, node => State.White);
            while (true)
            {
                var nodeToSearch = states.FirstOrDefault(z => z.Value == State.White).Key;
                if (nodeToSearch == null) break;

                if (!TarjanDepthSearch(nodeToSearch, states, sortedBlocks))
                    throw new ArgumentException("Blocks cannot be sorted! Blocks list has a cycle!");
            }
            sortedBlocks.Reverse();
            return sortedBlocks;
        }

        private bool TarjanDepthSearch(Node node, Dictionary<Node, State> states, IList<IBlock> sortedBlocks)
        {
            if (states[node] == State.Gray) return false;
            if (states[node] == State.Black) return true;
            states[node] = State.Gray;

            var incidentBlocks = node.IncidentBlocks.Where(block => block.From == node);
            if (incidentBlocks.Count() > 1)
                throw new ArgumentException("Blocks list contain duplicates!");

            var incidentBlock = incidentBlocks.FirstOrDefault();
            if (incidentBlock != null)
            {
                if (!TarjanDepthSearch(incidentBlock.To, states, sortedBlocks))
                    return false;

                states[node] = State.Black;
                sortedBlocks.Add(incidentBlock);
            }
            return true;
        }

        internal enum State
        {
            White,
            Gray,
            Black
        }
        #endregion
    }
}
