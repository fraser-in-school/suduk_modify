using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
namespace ConsoleApp1
{
    public class Suduku
    {
        public static void Write_txt(int[,] ans)
        {
            File.sw.WriteLine(Count.count.ToString());
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    if (j != 8)
                        File.sw.Write(ans[i, j].ToString() + " ");
                    else
                        File.sw.WriteLine(ans[i, j].ToString());
                }
            }
            File.sw.WriteLine();
        }
        public static void Exchange(int[,] ans, int before, int after, bool Is_line)
        {
            if (Is_line)
                for (int i = 0; i < 9; i++)
                {
                    int temp = ans[before, i];
                    ans[before, i] = ans[after, i];
                    ans[after, i] = temp;
                }
            else
                for (int i = 0; i < 9; i++)
                {
                    int temp = ans[i, before];
                    ans[i, before] = ans[i, after];
                    ans[i, after] = temp;
                }
        }
        public static void Dfs(int n, int[] sample)
        {
            if (n >= 9)
            {
                int[,] ans = new int[9, 9];
                int[] shift = { 0, 3, 6, 7, 4, 1, 2, 5, 8 };
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        ans[i, j] = sample[(j + shift[i]) % 9];
                        //Console.Write("{0} ",ans[i,j] );
                    }
                    //Console.WriteLine();
                }
                int[,] line = { { 3, 4, 5, 6, 7, 8 }, { 5, 3, 4, 8, 6, 7 } };
                int[,] row = { { 0, 3, 4, 5, 6, 7, 8 }, { 0, 5, 3, 4, 8, 6, 7 } };
                for (int i = 0; i < 6; i++)
                {

                    Suduku.Exchange(ans, line[0, i], line[1, i], true);
                    for (int j = 0; j < 6; j++)
                    {
                        Suduku.Exchange(ans, row[0, j], row[1, j], false);
                        Suduku.Write_txt(ans);
                        Count.count++;
                        if (Count.count >= Count.maxn)
                        {
                            // File.sw.Flush();
                            File.sw.Close();
                            return;
                        }
                    }
                }
            }
            if (Count.count >= Count.maxn) return;
            for (int i = 1; i <= 9; i++)
            {
                int j = 0, flag = 0;
                while (j < n)
                {
                    if (sample[j] == i)
                    {
                        flag = 1;
                        break;
                    }
                    j++;
                }
                if (flag == 0)
                {
                    sample[n] = i;
                    //Console.WriteLine("{0} {1}",Count.count,Count.maxn);
                    Dfs(n + 1, sample);
                }
            }
        }
        public static void Swap(int[] sample, int i, int j)
        {
            int temp = sample[i];
            sample[i] = sample[j];
            sample[j] = temp;
        }
    }
    public class File
    {
        public static StreamWriter sw = new StreamWriter("out.txt", false, Encoding.Default);
    }
    public class Count
    {
        public static int count = 0;
        public static int maxn = 100;
    }
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch runtime = new System.Diagnostics.Stopwatch();
            runtime.Start();
            int[] Line = new int[9];
            Line[0] = 4;
            char[] str = new char[5];
            Count.maxn = Convert.ToInt32(Console.ReadLine());
            Suduku.Dfs(1, Line);
            runtime.Stop();
            Console.WriteLine("{0}", runtime.Elapsed.TotalMilliseconds);
            Console.ReadKey();
        }
    }
}