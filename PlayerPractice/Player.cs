namespace _019_Player
{
    internal class Player
    {
        private int _hp;

        public string Name { get; }
        public int MaxHP { get; }
        public int HP
        {
            get
            {
                return _hp;
            }

            private set
            {
                if (value > MaxHP) _hp = MaxHP;
                else if (value < 0) _hp = 0;
                else _hp = value;
            }
        }
        public bool IsDead => HP <= 0;
        public static int Count { get; private set; }

        public Player(string name, int maxHP)
        {
            if (maxHP <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHP), maxHP, "最大生命值必须大于 0");
            Name = name.Trim();
            MaxHP = maxHP;
            HP = maxHP;
            Count++;
        }

        public Player(string name) :this(name, 100)
        {

        }

        public bool TakeDamage(int amount)
        {
            if (amount < 0 || IsDead) return false;

            HP -= amount;
            return true;
        }

        public bool Heal(int amount)
        {
            if (amount < 0 || IsDead) return false;

            HP += amount;
            return true;
        }

        public void PrintStatus()
        {
            Console.WriteLine($"角色{Name}的血量为：{HP}/{MaxHP}，{(IsDead ? "已死亡" : "正常存活")}");
        }

        public static void PrintCount()
        {
            Console.WriteLine($"当前角色总数：{Count}");
        }


    }
}
