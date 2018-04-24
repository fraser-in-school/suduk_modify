using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace suduku_find_answer
{
    public class File
    {
        public static StreamWriter sw = new StreamWriter("suduku.txt", false, Encoding.Default);
    }
    public class suduku_find_answer
    {
        public static void Prt(int[,] arr)
        {
            for(int i=0;i<9;i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write("{0} ",arr[i,j]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static int Check(int[,] numbers)/*检查该矩阵是否满足数独规则，0-满足规则*/
        {                   /*1--9-该0行出错；10--18-该数字-9列出错；19--27-该数字-18个宫格出错*/
            for (int i = 0; i < 9; i++)/*检查行的重复*/
            {
                bool[] line = new bool[10];
                for (int j = 0; j < 9; j++)
                {
                    if (line[numbers[i, j]] && numbers[i, j] != 0) return i+1;/*出错行为第i行*/
                    else line[numbers[i, j]] = true;
                }
            }
            for (int i = 0; i < 9; i++)/*检查列的重复*/
            {
                bool[] row = new bool[10];
                for (int j = 0; j < 9; j++)
                {
                    if (row[numbers[j, i]] && numbers[j, i] != 0) return i + 10;/*出错在第i列*/
                    else row[numbers[j, i]] = true;
                }
            }
            int p = 3, q = 3;
            while (p <= 9 && q <= 9)
            {
                bool[] pane = new bool[10];
                for (int i = p-3; i < p; i++)
                {
                    for (int j = q-3; j < q; j++)
                    {
                        if (pane[numbers[i, j]] && numbers[i, j] != 0) return i + 19;/*出错行为第i个宫格*/
                        else pane[numbers[i, j]] = true;
                    }
                }
                q += 3;
                if (q == 9) 
                {
                    p += 3;
                    q = 3;
                }
            }

            return 0;/*未发现错误*/
        }
        
        public static void Write_txt(int[,] ans)
        {
            //File.sw.WriteLine(Count.count.ToString());
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
        public static int[] Count_zero(int[,] arr)
        {
            int[] num_zero = new int[27];
            for(int i=0;i<9;i++)
                for(int j=0;j<9;j++)
                    if(arr[i,j]==0)
                    {
                        num_zero[i]++;
                        num_zero[9 + j]++;
                        num_zero[(i / 3) * 3 + j / 3 + 18]++;
                    }
            return num_zero;
        }
        public static void Place(int[,] arr,int x,int[] num_zero)
        {
            if (x < 9)          //按行来填写空格
            {
                bool[] statu = new bool[10];    //记录出现的数字
                int[] space = new int[10];  //记录空格
                int[] remain = new int[10]; //记录剩下的数字
                int p = 0, q = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (arr[x, i] != 0)
                        statu[arr[x, i]] = true;
                    else space[p++] = i;
                }
                for (int i = 1; i < 10; i++)
                    if (!statu[i]) remain[q++] = i;
                while (--q >= 0)
                {
                    arr[x, space[0]] = remain[q];
                    num_zero[x]--;
                    num_zero[space[0] + 9]--;
                    num_zero[(x / 3) * 3 + space[0] / 3 + 18]--;
                    suduku_find_answer.Dfs(arr, num_zero);
                }
            }
            else if (x >= 9 && x < 18)
            {
                bool[] statu = new bool[10];
                int[] space = new int[10];
                int[] remain = new int[10];
                int p = 0, q = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (arr[i, x] != 0)
                        statu[arr[i, x]] = true;
                    else space[p++] = i;
                }
                for (int i = 1; i < 10; i++)
                    if (!statu[i]) remain[q++] = i;
                while (--q >= 0)
                {
                    arr[space[0], x] = remain[q];
                    num_zero[x]--;
                    num_zero[space[0] + 9]--;
                    num_zero[(x / 3) * 3 + space[0] / 3 + 18]--;
                    suduku_find_answer.Dfs(arr, num_zero);
                }
            }
            else if (x >= 18) 
            {
                bool[] statu = new bool[10];
                int[] space = new int[10];
                int[] remain = new int[10];
                int p = 0, q = 0;
                for (int i = (x - 18) / 3 * 3; i < (x - 18) / 3 + 3; i++) 
                    for (int j = (x - 18) % 3; j < (x - 18) % 3 + 3; j++)
                    {
                        if (arr[i, j] != 0)
                            statu[arr[i, j]] = true;
                        else
                        {
                            space[0] = i;
                            space[1] = j;
                        }
                    }
                for (int i = 1; i < 10; i++)
                    if (!statu[i]) remain[q++] = i;
                while (--q >= 0)
                {
                    arr[space[0], space[1]] = remain[q];
                    num_zero[x]--;
                    num_zero[space[0] + 9]--;
                    num_zero[(x / 3) * 3 + space[0] / 3 + 18]--;
                    suduku_find_answer.Dfs(arr, num_zero);
                }
            }
        }
        public static void Dfs(int[,] arr,int[] num_zero)
        {
            if (suduku_find_answer.Check(arr) != 0) return;
            //suduku_find_answer.Prt(arr);
            num_zero = suduku_find_answer.Count_zero(arr);
            int min = 9, max = 0;
            for (int i=0;i<27;i++)
            {
                if (num_zero[i] == 1) 
                {
                    suduku_find_answer.Place(arr, i, num_zero);
                    break;
                }
                else if(num_zero[i]>1)
                {
                    if (num_zero[i] < min) min = num_zero[i];
                }
                if(num_zero[i]>max)
                {
                    max = num_zero[i];
                }
            }
            if (max == 0) 
            {
                suduku_find_answer.Prt(arr);
                return;
            }
        }
        public static int Get_zero_number(int[,]arr)
        {
            int num_zero = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (arr[i, j] == 0) num_zero++;
            return num_zero;

        }
        public static void Get_answer(int[,] arr)
        {
            if (suduku_find_answer.Check(arr) != 0) return;
            if (suduku_find_answer.Get_zero_number(arr) == 0) 
            {
                suduku_find_answer.Write_txt(arr);
                return;
            }
            
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (arr[i, j] == 0)
                        for (int k = 1; k <= 9; k++) 
                        {
                            arr[i, j] = k;
                            Get_answer(arr);
                        }
                }
        }
        public static void Get_read(string path)
        {
            
            int[,] arr = new int[9, 9];
            int k = 0;
            StreamReader sr = new StreamReader(path, Encoding.Default);
            while (true)
            {
                int flag = 0;
                for (int i = 0; i < 9; i++)
                {
                    string str = sr.ReadLine();
                    if (str==null)
                    {
                        flag = 1;
                        break;
                    }
                    string[] temp = str.Split(" ".ToCharArray());
                    for (int j = 0; j < 9; j++)
                        arr[i, j] = int.Parse(temp[j]);
                    
                }
                if (flag == 1) break;
                //suduku_find_answer.Prt(arr);
                string space=sr.ReadLine();
                int[] num_zero = suduku_find_answer.Count_zero(arr);
                suduku_find_answer.Get_answer(arr);
                //suduku_find_answer.Dfs(arr, num_zero);
                //k++;
                //Console.WriteLine("{0}",k);
            }

        }
    }
}
