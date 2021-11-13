namespace TradeSystem
{
    /// <summary>
    /// Manages finance operations inside the game.
    /// </summary>
    public static class TradeSystem
    {
        /// <summary>
        /// The global sum of money.
        /// </summary>
        public static MoneySum GlobalSum { get; } =
            new MoneySum("global.money.sum", true);
        /// <summary>
        /// The sum that can be used to manage money inside individual levels.
        /// Must be created before usage.
        /// </summary>
        public static MoneySum Sum { get; set; }
    }
}