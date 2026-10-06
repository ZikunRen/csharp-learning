using CSharpLearning.Override;
using static CSharpLearning.TestUtils;

// ===== 非法输入与边界 =====

// T01（你来写，用 CheckThrows）：攻击力为 -1 创建角色 → 抛出 ArgumentOutOfRangeException
CheckThrows<ArgumentOutOfRangeException>("T01", () => new Character("Dummy", 50, -1 ));

// T02 攻击力为 0 → 不抛异常，AttackPower 为 0
var dummy = new Enemy("Dummy", 50, 0);
Check("T02", dummy.AttackPower == 0);

// T03 攻击 null → 抛出 ArgumentNullException
// null! 告诉编译器"我是故意传 null 的"，消除可空警告，这才是 null! 的正确用法
CheckThrows<ArgumentNullException>("T03", () => new Player("P3").Attack(null!));

// T04 已死亡的角色发起攻击 → 目标 Hp 不变
var ghost = new Enemy("Ghost", 10, 15);
ghost.TakeDamage(10);
var p4 = new Player("P4");
ghost.Attack(p4);
Check("T04", ghost.IsDead && p4.Hp == 100);

// ===== 基类 Attack =====

// T05 Enemy（攻击力 15）攻击满血 Player → Hp 为 85
var goblin = new Enemy("Goblin", 50, 15);
var p5 = new Player("P5");
goblin.Attack(p5);
Check("T05", p5.Hp == 85);

// ===== Player 击杀奖励 =====

var hero = new Player("Hero");
var orc = new Enemy("Orc", 50, 5);

// T06（你来写，用 Check，包括攻击那一行）：hero 攻击 orc 一次 → orc 的 Hp 为 30，hero 的 Gold 为 0
hero.Attack(orc);
Check("T06", orc.Hp == 30 && hero.Gold == 0);

// T07 再攻击两次 → orc 死亡、Hp 为 0，Gold 为 10
hero.Attack(orc);
hero.Attack(orc);
Check("T07", orc.IsDead && orc.Hp == 0 && hero.Gold == 10);

// T08 再攻击已死亡的 orc → Gold 仍为 10
hero.Attack(orc);
Check("T08", hero.Gold == 10);

// T09 一击刚好打死 → Hp 为 0、死亡、Gold 为 10
var p9 = new Player("P9");
var weakling = new Enemy("Weakling", 20, 5);
p9.Attack(weakling);
Check("T09", weakling.IsDead && weakling.Hp == 0 && p9.Gold == 10);

// T10 通过 Character 类型的变量击杀 → 依然获得 Gold
var p10 = new Player("P10");
Character p10AsCharacter = p10;          // 同一个对象，两个不同类型的变量
var e10 = new Enemy("E10", 20, 5);
p10AsCharacter.Attack(e10);              // override：看对象，执行 Player 的版本
Check("T10", e10.IsDead && p10.Gold == 10);

// ===== Slime =====

// T11 创建 Slime → MaxHp 30、Hp 30、AttackPower 10
var slime = new Slime("Slime");
Check("T11", slime.MaxHp == 30 && slime.Hp == 30 && slime.AttackPower == 10);

// T12 Slime 类型的变量攻击满血 Player → Hp 为 80
var p12 = new Player("P12");
slime.Attack(p12);                       // 变量是 Slime → new 的版本，双倍
Check("T12", p12.Hp == 80);

// T13 同一个 Slime，用 Character 类型的变量攻击满血 Player → Hp 为 90
Character slimeAsCharacter = slime;
var p13 = new Player("P13");
slimeAsCharacter.Attack(p13);            // new：看变量，执行基类的版本，一倍
Check("T13", p13.Hp == 90);

PrintSummary();

// ① override 和 new 有什么区别？用基类引用调用时，结果有什么不同？
// override是将基类的虚方法重写，会根据对象的实际类型选择执行哪个版本，用基类引用调用时执行的是派生类的版本。
// new是隐藏，派生类定义了一个与基类无关的同名方法，会根据变量的声明类型选择方法执行，用基类引用调用时执行的是基类的版本。
// 另外，override要求基类方法是virtual、abstract 或 override，new没有这个要求。
//
// ② 什么是多态？变量的声明类型和对象的实际类型分别决定了什么？
// 多态是同一方法调用，作用在不同类型的对象上，表现出不同的行为。变量的声明类型决定编译时能调用哪些成员，对象的实际类型决定运行时执行虚方法的哪个版本。

// ③ 在 Player 的 Attack 里，写 base.Attack(target) 和直接写 Attack(target) 有什么区别？
// base.Attack(target)调用的是基类的实现，用于在重写方法里复用基类的逻辑。直接写 Attack(target)调用的是Player自己的Attack方法，方法会无限重复调用，最终导致栈溢出，程序崩溃。
