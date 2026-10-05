namespace CSharpLearning.Inheritance
{
    internal class Character
    {
        private int _hp;
        public string Name { get; }
        public int MaxHp { get; }
        public int Hp
        {
            get
            {
                return _hp;
            }

            private set
            {
                if (value > MaxHp) _hp = MaxHp;
                else if (value < 0) _hp = 0;
                else _hp = value;
            }
        }
        public bool IsDead => Hp <= 0;

        public Character(string name, int maxHp)
        {
            if (maxHp <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHp), maxHp, "最大生命值必须大于 0");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("名字不能为空或空格", nameof(name));

            Name = name;
            MaxHp = maxHp;
            Hp = maxHp;
        }

        public bool TakeDamage(int amount)
        {
            if (amount < 0 || IsDead) return false;

            Hp -= amount;
            return true;
        }

        public bool Heal(int amount)
        {
            if (amount < 0 || IsDead) return false;

            int missing = MaxHp - Hp;
            if (missing > amount) Hp += amount;
            else Hp = MaxHp; 
            return true;
        }

        public void PrintStatus()
        {
            Console.WriteLine($"角色{Name}的血量为：{Hp}/{MaxHp}，{(IsDead ? "已死亡" : "正常存活")}");
        }

    }
}
