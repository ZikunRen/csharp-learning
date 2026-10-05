

namespace CSharpLearning.Inheritance
{
    internal class Player : Character

    {
        private const int DefaultMaxHp = 100;
        public int Gold { get; private set; } = 0;
        public Player(string name) : base(name, DefaultMaxHp)
        {
        }

        public bool GainGold(int amount)
        {
            if (amount < 0) return false;
            int missing = int.MaxValue - Gold;
            if (amount < missing) Gold += amount;
            else Gold = int.MaxValue;
            return true;
        }

    }
}
