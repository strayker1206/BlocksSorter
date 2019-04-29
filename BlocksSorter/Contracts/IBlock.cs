namespace BlocksSorter.Contracts
{
    public interface IBlock
    {
        /// <summary>
        /// <see cref="Пункт отправления">StartPoint</see>.
        /// </summary>
        string StartPoint { get; }
        /// <summary>
        /// <see cref="Пункт назначения">StartPoint</see>.
        /// </summary>
        string EndPoint { get; }
    }
}
