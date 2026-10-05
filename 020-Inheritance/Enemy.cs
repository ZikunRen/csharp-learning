namespace CSharpLearning.Inheritance
{
    internal class Enemy : Character
    {
        public int AttackPower { get; }

        public Enemy(string name, int maxHp, int attackPower) : base(name, maxHp)
        {
            if (attackPower < 0) throw new ArgumentOutOfRangeException(nameof(attackPower), "攻击力不能为负数");
            AttackPower = attackPower;
        }

        public bool Attack(Character target)
        {
            if (target is null) throw new ArgumentNullException(nameof(target), "攻击对象不能为空");
            if (IsDead) return false;
            return target.TakeDamage(AttackPower);
        }
    }
}
