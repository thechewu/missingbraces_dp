using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracesProblem
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        /// <summary>
        /// Dynamic programming solution to missing braces problem.
        /// The idea is to have a n by n matrix of integers,
        /// in which A[i,j] is the number of symbols you need to add to the substring S[i]...S[j]
        /// in order to create a correct string from it.
        /// 
        /// Example:
        /// (())
        /// 
        /// A[0,0] = 1 All single braces need exactly 1 symbol to be proper
        /// A[0,1] = 2 Two open braces need two closed braces to close
        /// A[0,2] = 1 It just needs one closing brace to be a correct string
        /// A[0,3] = 0 It is a correct string
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            
            char[] open = new char[] { '(', '{', '[' };
            char[] closed = new char[] { ')', '}', ']' };
            //string s = GenerateRandomInput(10, open, closed);
            string s = Console.ReadLine();
            int n = s.Length;
            // Need extra row on the right for edge cases in the calculation
            int[,] A = new int[n + 1, n + 1];

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // Initialize everything with zeros except the diagonal is 1 (single braces)
            for (int i = 0; i <= n; i++)
                for (int j = 0; j <= n; j++)
                    A[i, j] = (i <= j) ? 1 : 0;

            // We start from the second to last row at the bottom and work our way up the matrix,
            // and we go from left to right on each row, based on the i-j difference
            // Example:
            // For n=4, we would go:
            // 2,2 2,3
            // 1,1 1,2 1,3
            // 0,0 0,1 0,2 0,3
            // Our solution will be the A[0,n-1], or A[0,3] in this case
            for (int i = n - 2; i >= 0; i--)
            {
                for (int j = i; j < n; j++)
                {
                    // Skip over single braces
                    if (i == j)
                    {
                        A[i, j] = 1;
                        continue;
                    }
                    // If the current character (S[i]) is a closed brace, that means it needs at least 1 to close,
                    // plus whatever its neighbour needs to complete
                    if (closed.Contains(s[i]))
                    {
                        A[i, j] = 1 + A[i + 1, j];
                    }
                    // And if it's not...
                    else
                    {
                        // First option is that there is a matching closing brace in the substring
                        int e_min = int.MaxValue;
                        char c = closed[Array.IndexOf(open, s[i])];
                        // We find the one that will give us the minimal number
                        for (int k = i; k <= j; k++)
                        {
                            if (s[k].Equals(c))
                            {
                                int temp = A[i + 1, k - 1] + A[k + 1, j];
                                if (temp < e_min) e_min = temp;
                            }
                        }

                        // In case there is none, we insert a matching close brace at a place where it would give the minimal number
                        int n_min = int.MaxValue;
                        for (int k = i; k < j; k++)
                        {
                            int temp = A[i, k] + A[k+1, j];
                            if (temp < n_min) n_min = temp;
                        }
                        // And the result is whichever is less
                        A[i, j] = Math.Min(e_min, n_min);

                    }

                }
            }
            stopwatch.Stop();
            PrintMatrix(A, n);
            Console.WriteLine("Solution: " + A[0, n - 1] + " found in " + stopwatch.ElapsedMilliseconds + "ms");
            
            Console.ReadKey();
        }

        static string GenerateRandomInput(int n,char[] open, char[] closed)
        {
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                sb.Append(rnd.Next() % 2 == 0 ? open[rnd.Next(3)] : closed[rnd.Next(3)]);                
            }
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
        static void PrintMatrix(int[,] A, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(A[i, j]);
                }
                Console.Write(System.Environment.NewLine);
            }
        }
    }
}
