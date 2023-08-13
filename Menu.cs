using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockMatrix
{
    public class Menu
    {
        private List<Matrix> vec = new List<Matrix>();
        private int index = -1;

        public Menu() { }

        public void Run()
        {
            int n;
            do
            {
                n = GetMenuPoint();
                switch (n)
                {
                    case 1:
                        SetMatrix();
                        break;
                    case 2:
                        GetElement();
                        break;
                    case 3:
                        Add();
                        break;
                    case 4:
                        Mul();
                        break;
                }

            } while (n != 0);

        }

        #region Menu operations

        private static int GetMenuPoint()
        {
            int n;
            do
            {
                Console.WriteLine("\n\n 0. - Quit");
                Console.WriteLine(" 1. - Set a matrix");
                Console.WriteLine(" 2. - Get an element");
                Console.WriteLine(" 3. - Add matrices");
                Console.WriteLine(" 4. - Multiply matrices");
                Console.Write(" Choose: ");
                try
                {
                    n = int.Parse(Console.ReadLine());
                }
                catch (System.FormatException) { n = -1; }
            } while (n < 0 || n > 5); //keep asking if n is not 0-4
            return n;
        }

        private void SetMatrix()
        {
            bool ok = false;
            int b1 = -1, b2 = -1;
            do
            {
                try
                {
                    Console.Write("b1? ");
                    b1 = int.Parse(Console.ReadLine());
                    Console.Write("b2? ");
                    b2 = int.Parse(Console.ReadLine());
                    ok = true;
                    vec.Add(new Matrix(b1, b2)); //creating new matrix
                }
                catch (Matrix.InvalidSizeException)
                {
                    Console.WriteLine("b1 and b2 should be more than 1 and less than n-1 ! ");
                    ok = false;
                }
            } while (!ok);
            index++;
            int e = 0;
            for (int i = 0; i < b1 + b2; i++)
            {
                for (int j = 0; j < b1 + b2; j++)
                {
                    if (vec[index].inBlock(i, j))
                    {
                            Console.WriteLine("Element: ");
                            try
                            {
                                e = int.Parse(Console.ReadLine());
                                if (e == 0)
                                    throw new Matrix.SettingZeroException();
                                vec[index].setElem(i, j, e);
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("Number is expected!");
                                ok = false;
                                break;

                            }
                            catch (Matrix.SettingZeroException)
                            {
                                Console.WriteLine("You can't set 0 to the blocks");
                                j--;
                            }
                    }
                }
            }
            Console.WriteLine((index + 1) + "th matrix: " + vec[index].ToString());
        }

        private void GetElement()
        {
            if (vec.Count == 0)
            {
                Console.WriteLine("Set a matrix first!");
                return;
            }

            int entryMatrix = -1;
            do
            {
                Console.WriteLine("Which Matrix? Please write the number of the matrix");
                Console.WriteLine($"Matrix number must be between 1 and {vec.Count}");
                entryMatrix = int.Parse(Console.ReadLine());
            } while (entryMatrix < 1 || entryMatrix > vec.Count);

            try
            {
                Console.WriteLine("Give the index of the row: ");
                int i = int.Parse(Console.ReadLine());
                Console.WriteLine("Give the index of the column: ");
                int j = int.Parse(Console.ReadLine());
                Console.WriteLine($"[{i},{j}]={vec[entryMatrix - 1].GetEntry(i, j)}");
            }
            catch (Matrix.InvalidIndexException)
            {
                Console.WriteLine($"Index of a matrix must be between 1 and {vec[entryMatrix - 1].GetSize()}");
            }
        }

        private void Add()
        {
            try
            {
                if (vec.Count == 0)
                {
                    Console.WriteLine("Set a matrix first!");
                    return;
                }
                int m1 = -1, m2 = -1;

                do
                {
                    Console.WriteLine($"Matrix number must be between 1 and {vec.Count}");
                    Console.WriteLine("The number of the first matrix to add: ");
                    m1 = int.Parse(Console.ReadLine());
                } while (m1 < 1 || m1 > vec.Count);
                do
                {
                    Console.WriteLine("The number of the second matrix to add: ");
                    m2 = int.Parse(Console.ReadLine());
                } while (m2 < 1 || m2 > vec.Count);

                Console.WriteLine($"sum:\n{Matrix.add(vec[m1 - 1], vec[m2 - 1]).ToString()}\n\n");
            }
            catch (Matrix.DimensionMismatchException)
            {
                Console.WriteLine("Matrix sizes should be the same!");
            }

        }
        private void Mul()
        {
            try
            {
                if (vec.Count == 0)
                {
                    Console.WriteLine("Set a matrix first!");
                    return;
                }
                int m1 = -1, m2 = -1;

                do
                {
                    Console.WriteLine($"Matrix number must be between 1 and {vec.Count}");
                    Console.WriteLine("The number of the first matrix to multiply: ");
                    m1 = int.Parse(Console.ReadLine());
                } while (m1 < 1 || m1 > vec.Count);
                do
                {
                    Console.WriteLine("The number of the second matrix to multiply: ");
                    m2 = int.Parse(Console.ReadLine());
                } while (m2 < 1 || m2 > vec.Count);
                Console.WriteLine($"product:\n{Matrix.mul(vec[m1 - 1], vec[m2 - 1]).ToString()}\n\n");
            }

            catch (Matrix.DimensionMismatchException)
            {
                Console.WriteLine("Matrix sizes should be the same!");
            }
        }
        #endregion
    }
}
