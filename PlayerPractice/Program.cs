//测试用例：
//打印总数 → 0
//A 受伤 30 → 70/100
//A受伤0 → 70/100
//A 再受伤 -10 → 仍为 70
//A治疗 -10 → 仍为 70
//A 再受伤 200 → 0/100，且 isDead 为 true
//A 再治疗 50 → 仍为 0
//B（MaxHp 150）受伤 20 后治疗 500 → 150/150
//B 治疗 10 → 仍为 150
//B 受伤 150 → 0/150，且isDead为true
//再创建 C，打印总数 → 3
//创建D失败（maxHP≤0/maxHP = -5） → 抛出异常ArgumentOutOfRangeException
//打印总数 → 3


using _019_Player;

namespace _019_Player
{
    internal class Program
    {
        static void AssertTrue(bool condition, string description) => Console.WriteLine($"{(condition ? "PASS" : "FAIL")} {description}");


        static void Main()
        {
            Player.PrintCount();
            AssertTrue(Player.Count == 0, "初始状态，角色数量应为0");

            Player a = new("A");

            a.TakeDamage(30);
            AssertTrue(a.HP == 70, "A受到30伤害后血量应为70");
            a.PrintStatus();

            a.TakeDamage(0);
            AssertTrue(a.HP == 70, "A受到0伤害后的血量应为70");
            a.PrintStatus();

            a.TakeDamage(-10);
            AssertTrue(a.HP == 70, "A受到-10伤害后的血量应为70");
            a.PrintStatus();

            a.Heal(-10);
            AssertTrue(a.HP == 70, "A受到-10治疗后的血量应为70");
            a.PrintStatus();

            a.TakeDamage(200);
            AssertTrue(a.HP == 0, "A受到200伤害后的血量应为0");
            AssertTrue(a.IsDead, "A受到200伤害后应为已死亡");
            a.PrintStatus();

            a.Heal(50);
            AssertTrue(a.HP == 0, "A受到50治疗后的血量应为0");
            AssertTrue(a.IsDead, "A受到50治疗后应为已死亡");
            a.PrintStatus();

            Player b = new("B", 150);

            b.TakeDamage(20);
            b.Heal(500);
            AssertTrue(b.HP == 150, "B受到20伤害500治疗后的血量应为150");
            b.PrintStatus();

            b.Heal(10);
            AssertTrue(b.HP == 150, "B受到10治疗后的血量应为150");
            b.PrintStatus();
            
            b.TakeDamage(150);
            AssertTrue(b.HP == 0, "B受到150伤害后的血量应为0");
            AssertTrue(b.IsDead, "B受到150伤害后应为已死亡");
            b.PrintStatus();

            Player c = new("C");

            Player.PrintCount();
            AssertTrue(Player.Count == 3, "创建A B C角色后，角色数量应为3");

            try
            {
                Player d = new("D", 0);
                AssertTrue(false, "创建MaxHP = 0的实例D，应抛出异常");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                AssertTrue(true, "创建MaxHP = 0的实例D，应抛出异常");
            }

            try
            {
                Player e = new("E", -5);
                AssertTrue(false, "创建MaxHP = -5 的实例E，应抛出异常");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                AssertTrue(true, "创建MaxHP = -5 的实例E，应抛出异常");
            }

            Player.PrintCount();
            AssertTrue(Player.Count == 3, "创建失败D E后，角色数量应为3");

        }
    }
}
//① 字段和属性有什么区别？为什么 Hp 适合用属性？
// 9.1：字段是直接存储数据的变量，一般设为私有。属性通过get和set访问器控制对字段的读写，本质是一对方法，用起来像字段。
// 用属性可以分别控制读写权限，并在赋值时做校验，如HP的赋值限制在 0 到 MaxHp 之间，这就是封装。

//② static 的 Count 属于谁？每个 Player 对象都各有一份吗？
// 9.2：属于类自身，不属于任何实例对象。不是，整个类只有一份，所有对象共享。

//③ 写了带参数的构造函数后，new Player() 还能编译通过吗？为什么？
// 9.3：不能。因为一旦声明了带参数的构造函数，默认的无参构造函数就不再隐式生成，new Player() 会编译报错。需要的话，要自己显式写一个无参构造函数。
