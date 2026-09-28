using System;

namespace NumberGuessingGame
{
    internal class NumberGuessingGame
    {
        const int minNumber = 1;
        const int maxNumber = 50;
        const int maxGuessCount = 5;


        static int GenerateRandomNumber() => Random.Shared.Next(minNumber, maxNumber + 1);

        static int ReadValidGuess(int guessCount)
        {
            while (true)
            {
                Console.Write($"请输入第{guessCount}次猜测结果：");
                string? input = Console.ReadLine();

                bool isReadValid = int.TryParse(input, out int guessedNumber);

                if (isReadValid && guessedNumber >= minNumber && guessedNumber <= maxNumber)
                {
                    return guessedNumber;
                }

                Console.WriteLine($"输入格式有误，请输入{minNumber}~{maxNumber}的整数");
            }
        }

        static bool AskToPlayAgain()
        {
            while(true)
            {
                Console.Write("是否再次重新开始游戏（y/n）：");
                string? continueInput = Console.ReadLine();

                if (continueInput == "y") return true;
                if (continueInput == "n") return false;

                Console.WriteLine("输入格式有误，请重新输入。");
            }

        }

        static void Main()
        {

            bool isGameContinue = true;

            while (isGameContinue)
            {
                int randomNumber = GenerateRandomNumber();
                Console.WriteLine("已随机生成1~50的整数");
                bool isGuessedRight = false;

                for (int guesscount = 0; guesscount < maxGuessCount; guesscount++)
                {
                    int guessedNumber = ReadValidGuess(guesscount + 1);


                    if (guessedNumber == randomNumber)
                    {
                        Console.WriteLine("猜对了！！！");
                        isGuessedRight = true;
                        break;
                    }
                    else if (guessedNumber > randomNumber)
                    {
                        Console.WriteLine("太大了！！！");
                    }
                    else
                    {
                        Console.WriteLine("太小了！！！");
                    }

                }

                if (isGuessedRight) Console.WriteLine("恭喜猜对，本轮游戏结束。");
                else Console.WriteLine($"猜测次数已用完，正确答案是：{randomNumber}，本轮游戏结束。");

                isGameContinue = AskToPlayAgain();
            }
        }
    }
}
