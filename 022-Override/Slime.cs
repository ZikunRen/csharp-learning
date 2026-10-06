namespace CSharpLearning.Override
{
    internal class Slime : Enemy
    {
        private const int DefaultMaxHp = 30;
        private const int DefaultAttackPower = 10;
        private const int DamageMultiplier = 2;

        public Slime(string name) : base(name, DefaultMaxHp, DefaultAttackPower)
        {
        }

        // 报错实验 2：Slime.Attack 去掉 new 后，编译未出现错误，仅存在警告CS0114：“Slime.Attack(Character)”隐藏继承的成员“Character.Attack(Character)”。若要使当前成员重写该实现，请添加关键字 override。否则，添加关键字 new。
        public new bool Attack(Character target)
        {
            if (target is null) throw new ArgumentNullException(nameof(target), "攻击对象不能为空");
            if (IsDead) return false;
            return target.TakeDamage(AttackPower * DamageMultiplier);
        }
    }
}
