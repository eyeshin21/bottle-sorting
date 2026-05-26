using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static bool GetRandomAddition(int n, out string operation)
        {
            int n1 = GetRandomRange(1, n - 1);
            int n2 = n - n1;
            operation = $"{n1}+{n2}";
            return true;
        }

        public static bool GetRandomSubtraction(int n, out string operation)
        {
            if (n < 100)
            {
                int n1 = GetRandomRange(n + 1, Mathf.Min(n * 3, 99));
                int n2 = n1 - n;
                if (n2 > 0)
                {
                    operation = $"{n1}-{n2}";
                    return true;
                }
            }

            operation = default;
            return false;
        }

        static int[] _multiplications;
        static int[] Multiplications => _multiplications ??= new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
        public static bool GetRandomMultiplication(int n, out string operation)
        {
            var multiplications = Multiplications;
            multiplications.Swap();

            for (int i = multiplications.Length - 1; i >= 0; i--)
            {
                int n1 = multiplications[i];
                int n2 = n / n1;
                if (n1 * n2 == n)
                {
                    operation = GetRandomBool() ? $"{n1}x{n2}" : $"{n2}x{n1}";
                    return true;
                }
            }

            operation = default;
            return false;
        }

        public static bool GetRandomDivision(int n, out string operation)
        {
            if (n <= 50)
            {
                var multiplications = Multiplications;
                multiplications.Swap();

                for (int i = multiplications.Length - 1; i >= 0; i--)
                {
                    int n1 = multiplications[i];
                    int n2 = n * n1;
                    if (n2 < 100)
                    {
                        operation = $"{n2}\u00f7{n1}"; // Obelus
                        return true;
                    }
                }
            }

            operation = default;
            return false;
        }

        static int[] _operations;
        public static string GetRandomOperation(int n)
        {
            if (n < 1 || n > 100)
            {
                LegacyLog.NotSupported(n);
                return n.ToString();
            }

            if (_operations == null)
            {
                _operations = CreateIndices(4);
            }
            _operations.Swap();

            for (int i = 0; i < 4; i++)
            {
                int rnd = _operations[i];
                if (rnd == 0)
                {
                    if (GetRandomAddition(n, out string operation))
                    {
                        return operation;
                    }
                }
                else if (rnd == 1)
                {
                    if (GetRandomSubtraction(n, out string operation))
                    {
                        return operation;
                    }
                }
                else if (rnd == 2)
                {
                    if (GetRandomMultiplication(n, out string operation))
                    {
                        return operation;
                    }
                }
                else
                {
                    if (GetRandomDivision(n, out string operation))
                    {
                        return operation;
                    }
                }
            }

            return n.ToString();
        }
    }
}