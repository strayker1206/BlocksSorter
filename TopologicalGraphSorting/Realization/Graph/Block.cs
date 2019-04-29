using TopologicalGraphSorting.Contracts;
using System;

namespace TopologicalGraphSorting.Realization.Graph
{
    public class Block : IBlock
    {
        public string StartPoint { get; private set; }
        public string EndPoint { get; private set; }

        internal Node From { get; set; }
        internal Node To { get; set; }

        public Block(string startPoint, string endPoint)
        {
            ValidatePoints(startPoint, endPoint);
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        private void ValidatePoints(string startPoint, string endPoint)
        {
            if (string.IsNullOrEmpty(startPoint))
                throw new ArgumentNullException("Start Point is null or empty!");

            if (string.IsNullOrEmpty(endPoint))
                throw new ArgumentNullException("End Point is null or empty!");

            if (startPoint.ToLower() == endPoint.ToLower())
                throw new ArgumentException("Start Point and End Point is equal!");
        }

        public override string ToString() => $"{StartPoint} -> {EndPoint}";
    }
}
