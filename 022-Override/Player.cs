namespace CSharpLearning.Override
{
    internal class Player : Character
    {
        private const int DefaultMaxHp = 100;
        private const int DefaultAttackPower = 20;
        private const int KillReward = 10;

        public int Gold { get; private set; }

        public Player(string name) : base(name, DefaultMaxHp, DefaultAttackPower)
        {
        }

        // 报错实验 1：Character.Attack 去掉 virtual 后，Player.Attack编译报错CS0506：继承成员“Character.Attack(Character)”未标记为 virtual、abstract 或 override，无法进行重写
        public override bool Attack(Character target)
        {
            // base.Attack 返回 true 说明攻击前目标还活着，此时目标死亡即为本次击杀
            if (!base.Attack(target)) return false;
            if (target.IsDead) GainGold(KillReward);
            return true;
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
