namespace CSharpLearning.Inheritance
{
    internal class Program
    {
        // ===== Character规则 =====
        // TakeDamage(amount):
        // amount < 0       -> Hp不变，返回false
        // 已死亡      -> Hp不变，返回false
        // amount == 0      -> Hp 不变，返回 true
        // amount正常      -> Hp -= amount。Hp等于0时，IsDead为true / Hp小于0时,Hp设置为0，IsDead为true。返回true
        // Heal(amount):
        // amount < 0       -> Hp不变，返回false
        // 已死亡      -> Hp不变，返回false
        // amount == 0      -> Hp不变，返回true
        // amount正常     -> Hp += amount，Hp大于MaxHp时设置为MaxHp，返回true

        // --- Character 构造 ---（规则：name 为 null/空 → ArgumentException；maxHp ≤ 0 → ArgumentOutOfRangeException）
        // T01 正常：new Character("Hero", 50) → Name=="Hero", MaxHp==50, Hp==50, IsDead==false
        // T02 边界：new Character("Hero", 1) → MaxHp==1, Hp==1（最小的合法 maxHp）
        // T03 边界：new Character("A", 50) → Name=="A"（最短的合法名字）
        // T04 边界：new Character("Hero", 0) → 抛出 ArgumentOutOfRangeException（最大的非法 maxHp）
        // T05 非法：new Character("Hero", -10) → 抛出 ArgumentOutOfRangeException
        // T06 非法：new Character(null!, 50) → 抛出 ArgumentException
        // T07 非法：new Character("", 50) → 抛出 ArgumentException
        // T08 待定：new Character("   ", 50) → 抛出 ArgumentException

        // --- TakeDamage ---（初始：new Character("Hero", 100)）
        // T09 正常：TakeDamage(30) → Hp==70, IsDead==false, 返回 true
        // T10 边界：TakeDamage(0) → Hp==100, IsDead==false, 返回 true
        // T11 边界：TakeDamage(99) → Hp==1, IsDead==false（差 1 点致死）
        // T12 边界：TakeDamage(100) → Hp==0, IsDead==true（刚好致死）
        // T13 超出：TakeDamage(150) → Hp==0, IsDead==true（不会变成负数）
        // T14 非法：TakeDamage(-10) → Hp==100, 返回 false
        // T15 已死亡：先 TakeDamage(100)，再 TakeDamage(10) → Hp==0, IsDead==true, 返回 false

        // --- Heal ---（初始：new Character("Hero", 100)，先 TakeDamage(50)，此时 Hp==50）
        // T16 正常：Heal(20) → Hp==70, 返回 true
        // T17 边界：Heal(0) → Hp==50, 返回 true
        // T18 边界：Heal(50) → Hp==100（刚好回满）
        // T19 超出：Heal(80) → Hp==100（不超过 MaxHp）
        // T20 非法：Heal(-10) → Hp==50, 返回 false
        // T21 已死亡：先打到 0，再 Heal(10) → Hp==0, IsDead==true, 返回 false
        // T22 极大值：Heal(int.MaxValue) → 期望 Hp==100（先预测实际结果，跑的时候再看）

        // --- Player ---
        // T23 正常：new Player("Hero") → Name=="Hero", MaxHp==100, Hp==100, Gold==0, IsDead==false
        // T24 非法：new Player(null!) → 抛出 ArgumentException（验证 : base(...) 把参数传给了基类检查）
        // T25 正常：GainGold(10) → Gold==10
        // T26 累加：GainGold(10) 两次 → Gold==20
        // T27 边界：GainGold(0) → Gold==0
        // T28 非法：GainGold(-5) → Gold==0
        // T29 继承：new Player("Hero").TakeDamage(30) → Hp==70（继承来的方法能正常工作）

        // --- Enemy 构造 ---（规则：attackPower < 0 → ArgumentOutOfRangeException）
        // T30 正常：new Enemy("Slime", 50, 15) → Name=="Slime", MaxHp==50, Hp==50, AttackPower==15
        // T31 边界：new Enemy("Slime", 50, 0) → AttackPower==0（0 是合法的）
        // T32 边界：new Enemy("Slime", 50, -1) → 抛出 ArgumentOutOfRangeException
        // T33 非法：new Enemy("Slime", 0, 15) → 抛出 ArgumentOutOfRangeException（基类检查依然生效）

        // --- Enemy.Attack ---（攻击方：new Enemy("Slime", 50, 15)，目标：new Player("Hero")）
        // T34 正常：Attack(player) → player.Hp==85, player.IsDead==false；enemy.Hp==50（攻击方不受影响）
        // T35 致死：AttackPower 为 100 的敌人 Attack(player) → player.Hp==0, player.IsDead==true
        // T36 已死亡：player 死后再被 Attack → player.Hp==0
        // T37 边界：AttackPower 为 0 的敌人 Attack(player) → player.Hp==100
        // T38 非法：Attack(null) → 抛出 ArgumentNullException
        // T39 已死亡的敌人：enemy 先被打死，再 Attack(player) → 返回 false，player.Hp==100

        // ===== 编译期实验（不放进测试代码运行）=====
        // E01 Character c = new Player("Hero"); c.GainGold(10);
        //     → 预期：编译错误。错误代码：CS1061  原因：写在概念题③
        // E02 在 Main 中访问 Character 的 private 成员 → 结果：编译失败，CS0122："Character._hp"不可访问，因为它具有一定的保护级别
        // E03 在 Main 中访问 Character 的 protected 成员 → 结果：编译失败，CS0122："Character.Level"不可访问，因为它具有一定的保护级别
        // E04 在 Player 内部访问 Character 的 private 成员 → 结果：编译失败，CS0122："Character._hp"不可访问，因为它具有一定的保护级别
        // E05 在 Player 内部访问 Character 的 protected 成员 → 结果：编译成功

        static int _passCount = 0;
        static int _failCount = 0;

        static void Check(string id, bool condition)
        {
            if (condition)
            {
                _passCount++;
                Console.WriteLine($"{id} PASS");
            }
            else
            {
                _failCount++;
                Console.WriteLine($"{id} FAIL  <<<<<");
            }
        }

        static void CheckThrows<T>(string id, Action action) where T : Exception
        {
            try
            {
                action();
                Check(id, false);   // 能执行到这一行，说明没有抛出异常 → 失败
            }
            catch (T)
            {
                Check(id, true);    // 抛出了期望类型的异常 → 通过
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    实际抛出的是 {ex.GetType().Name}");
                Check(id, false);   // 抛出了异常，但类型不对 → 失败
            }
        }

        static void Main(string[] args)
        {
            RunCharacterTests();
            RunPlayerTests();
            RunEnemyTests();
            Console.WriteLine($"\n通过 {_passCount}，失败 {_failCount}");
        }

        static void RunCharacterTests()
        {
            Console.WriteLine("--- Character 构造 ---");

            // T01 正常
            {
                Character c = new Character("Hero", 50);
                Check("T01", c.Name == "Hero" && c.MaxHp == 50 && c.Hp == 50 && !c.IsDead);
            }
            // T02 边界：最小的合法 maxHp
            {
                Character c = new Character("Hero", 1);
                Check("T02", c.MaxHp == 1 && c.Hp == 1 && !c.IsDead);
            }
            // T03 边界：最短的合法名字
            {
                Character c = new Character("A", 50);
                Check("T03", c.Name == "A");
            }
            CheckThrows<ArgumentOutOfRangeException>("T04", () => new Character("Hero", 0));
            CheckThrows<ArgumentOutOfRangeException>("T05", () => new Character("Hero", -10));
            CheckThrows<ArgumentException>("T06", () => new Character(null!, 50));
            CheckThrows<ArgumentException>("T07", () => new Character("", 50));
            CheckThrows<ArgumentException>("T08", () => new Character("   ", 50));

            Console.WriteLine("--- TakeDamage ---");

            // T09 正常
            {
                Character c = new Character("Hero", 100);
                bool result = c.TakeDamage(30);
                Check("T09", result && c.Hp == 70 && !c.IsDead);
            }
            // T10 边界：伤害为 0
            {
                Character c = new Character("Hero", 100);
                bool result = c.TakeDamage(0);
                Check("T10", result && c.Hp == 100 && !c.IsDead);
            }
            // T11 边界：差 1 点致死
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(99);
                Check("T11", c.Hp == 1 && !c.IsDead);
            }

            // T12 边界：刚好致死 → Hp==0, IsDead==true
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(100);
                Check("T12", c.Hp == 0 && c.IsDead);
            }

            // T13 超出：伤害大于当前 Hp
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(150);
                Check("T13", c.Hp == 0 && c.IsDead);
            }
            // T14 非法：负数伤害
            {
                Character c = new Character("Hero", 100);
                bool result = c.TakeDamage(-10);
                Check("T14", !result && c.Hp == 100);
            }
            // T15 已死亡再受伤
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(100);
                bool result = c.TakeDamage(10);
                Check("T15", !result && c.Hp == 0 && c.IsDead);
            }
            // T40 极大值伤害（证明 TakeDamage 不会溢出）
            {
                Character c = new Character("Hero", 100);
                bool result = c.TakeDamage(int.MaxValue);
                Check("T40", result && c.Hp == 0 && c.IsDead);
            }

            Console.WriteLine("--- Heal ---");

            // T16 正常
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(50);
                bool result = c.Heal(20);
                Check("T16", result && c.Hp == 70);
            }
            // T17 边界：回血为 0
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(50);
                bool result = c.Heal(0);
                Check("T17", result && c.Hp == 50);
            }
            // T18 边界：刚好回满
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(50);
                c.Heal(50);
                Check("T18", c.Hp == 100);
            }
            // T19 超出上限
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(50);
                c.Heal(80);
                Check("T19", c.Hp == 100);
            }
            // T20 非法：负数回血
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(50);
                bool result = c.Heal(-10);
                Check("T20", !result && c.Hp == 50);
            }
            // T21 已死亡不能回血
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(100);
                bool result = c.Heal(10);
                Check("T21", !result && c.Hp == 0 && c.IsDead);
            }

            // T22 极大值回血：Hp 50 时 Heal(int.MaxValue) → Hp==100
            {
                Character c = new Character("Hero", 100);
                c.TakeDamage(50);
                bool result = c.Heal(int.MaxValue);
                Check("T22", result && c.Hp == 100);
            }
        }

        static void RunPlayerTests()
        {
            Console.WriteLine("--- Player ---");

            // T23 正常创建
            {
                Player p = new Player("Hero");
                Check("T23", p.Name == "Hero" && p.MaxHp == 100 && p.Hp == 100 && p.Gold == 0 && !p.IsDead);
            }

            // T24 new Player(null!) 抛出异常
            {
                try
                {
                    Player p = new Player(null!);
                    Check("T24", false);
                }
                catch (ArgumentException)
                {
                    Check("T24", true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"    实际抛出的是 {ex.GetType().Name}");
                    Check("T24", false);
                }
            }

            // T25 正常获得金币
            {
                Player p = new Player("Hero");
                bool result = p.GainGold(10);
                Check("T25", result && p.Gold == 10);
            }
            // T26 累加
            {
                Player p = new Player("Hero");
                p.GainGold(10);
                p.GainGold(10);
                Check("T26", p.Gold == 20);
            }
            // T27 边界：0
            {
                Player p = new Player("Hero");
                bool result = p.GainGold(0);
                Check("T27", result && p.Gold == 0);
            }
            // T28 非法：负数
            {
                Player p = new Player("Hero");
                bool result = p.GainGold(-5);
                Check("T28", !result && p.Gold == 0);
            }
            // T29 继承来的方法能正常工作
            {
                Player p = new Player("Hero");
                p.TakeDamage(30);
                Check("T29", p.Hp == 70);
            }
            // T41 金币溢出：先加到上限，再加 → 保持 int.MaxValue
            {
                Player p = new Player("Hero");
                p.GainGold(int.MaxValue);
                p.GainGold(10);
                Check("T41", p.Gold == int.MaxValue);
            }
        }

        static void RunEnemyTests()
        {
            Console.WriteLine("--- Enemy ---");

            // T30 正常创建
            {
                Enemy e = new Enemy("Slime", 50, 15);
                Check("T30", e.Name == "Slime" && e.MaxHp == 50 && e.Hp == 50 && e.AttackPower == 15);
            }
            // T31 边界：攻击力 0 合法
            {
                Enemy e = new Enemy("Slime", 50, 0);
                Check("T31", e.AttackPower == 0);
            }
            CheckThrows<ArgumentOutOfRangeException>("T32", () => new Enemy("Slime", 50, -1));
            CheckThrows<ArgumentOutOfRangeException>("T33", () => new Enemy("Slime", 0, 15));

            // T34 正常攻击：目标掉血，攻击方不受影响
            {
                Enemy e = new Enemy("Slime", 50, 15);
                Player p = new Player("Hero");
                bool result = e.Attack(p);
                Check("T34", result && p.Hp == 85 && !p.IsDead && e.Hp == 50);
            }
            // T35 一击致死
            {
                Enemy e = new Enemy("Boss", 200, 100);
                Player p = new Player("Hero");
                e.Attack(p);
                Check("T35", p.Hp == 0 && p.IsDead);
            }

            // T36 Player 死亡后再被攻击 → Hp 仍为 0
            {
                Enemy e = new Enemy("Slime", 50, 15);
                Player p = new Player("Hero");
                p.TakeDamage(100);
                e.Attack(p);
                Check("T36", p.Hp == 0 && p.IsDead);
            }

            // T37 攻击力为 0
            {
                Enemy e = new Enemy("Slime", 50, 0);
                Player p = new Player("Hero");
                e.Attack(p);
                Check("T37", p.Hp == 100);
            }

            // T38 Attack(null) 抛出 ArgumentNullException
            {
                Enemy e = new Enemy("Slime", 50, 0);
                CheckThrows<ArgumentNullException>("T38", () => e.Attack(null!));
            }
            
            

            // T39 已死亡的敌人不能攻击
            {
                Enemy e = new Enemy("Slime", 50, 15);
                Player p = new Player("Hero");
                e.TakeDamage(50);
                bool result = e.Attack(p);
                Check("T39", !result && p.Hp == 100);
            }
        }
    }
}


// ① private、protected、internal、public 的访问范围分别是什么？
// private 只能在声明它的类内部访问，派生类虽然继承了它，也不能直接访问。
// protected 可以在本类和派生类的内部访问，但不能通过对象从外部访问。
// internal 可以在同一个程序集（同一个项目编译出的 dll 或 exe）内的任何地方访问。
// public 在任何地方都可以访问。
//
// ② 创建派生类对象时，基类和派生类的构造函数谁先执行？为什么需要 : base(...)？
// 基类构造函数先执行，派生类构造函数后执行，因为派生类可能依赖基类已经初始化好的成员，而且基类的 private 成员只能由基类自己初始化。
// 不写 : base(...) 时编译器会默认调用基类的无参构造函数；如果基类没有无参构造函数，就必须用 : base(...) 指定调用哪个构造函数并传入参数，否则编译报错。
//
// ③ 用 Character 类型的变量引用 Player 对象时，能调用 Player 独有的方法吗？为什么？（结合 E01 的 CS1061）
// 不能直接调用。编译器根据变量的声明类型检查能访问哪些成员。如果需要调用，可以用 is 模式匹配判断类型后再转换。