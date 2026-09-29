namespace MethodParameters
{
    internal class Program
    {
        static void Swap(ref int a, ref int b)
        {
            (a, b) = (b, a);
        }

        static int CountDigit(string s)
        {
            int digitNumber = 0;
            foreach(char c in s)
            {
                if (c >= '0' && c <= '9') digitNumber++;
            }
            return digitNumber;
        }


        static bool MyTryParse(string s, out int result)
        {
            int start = 0;
            bool isNegative = false;
            result = 0;

            if(string.IsNullOrEmpty(s)) return false;

            if (s[0] == '-')
            {
                if (s.Length == 1) return false;
                isNegative = true;
                start = 1;
            }

            for (int i = start; i < s.Length; i++)
            {
                if (s[i] < '0' || s[i] > '9')

                {
                    result = 0;
                    return false;
                }

                result *= 10; 
                result += s[i] - '0';
            }

            if (isNegative) result = -result;

            return true;
        }

        static int Sum(params int[] numbers)
        {
            int sum = 0;
            foreach(int num in numbers)
            {
                sum += num;
            }
            return sum;
        }

        static void Main()
        {
            int x = 5;
            int y = 8;

            Swap(ref x, ref y);
            Console.WriteLine($"{x},{y}");

            int digitNumber = CountDigit("3598bsdk");
            Console.WriteLine(digitNumber);

            MyTryParse("-1255", out int number);
            Console.WriteLine(number);

            Console.WriteLine(Sum(1, 2, 3));        // 6
            Console.WriteLine(Sum());               // 0
            Console.WriteLine(Sum(new[] { 4, 5 })); // 9
            Console.WriteLine(Sum(4, 5));
        }
    }
}
