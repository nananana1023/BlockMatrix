using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockMatrix
{
    public class Matrix
    {
        #region Exceptions
        public class DimensionMismatchException : Exception { }; //adding matrices with different sizes 
        public class InvalidSizeException : Exception { }; //b1 or b2 invalid
        public class InvalidIndexException : Exception { }; //accesing index out of the whole matrix 
        public class SettingZeroException : Exception { }; //setting zero in the block
        public class OutOfBlocks : Exception { };

        #endregion

        #region Attribute
        private int b1, b2;
        private List<int> v; //storing the nonzero entries in a sequence
        private int n; //size of matrix
        #endregion

        #region Constructors
        public Matrix(int f, int s) //Initialize matrix with zeros
        {
            if (0 < f && s < (f + s) && 0<s && f < (f + s) )
            {
                n = f + s;
                b1 = f;
                b2 = s;
                v = new List<int>();
                for (int i = 0; i < VectorSize(); i++)
                {
                    v.Add(0);
                }
            }
            else
                throw new InvalidSizeException();
            
        }
        #endregion

        #region Helper functions
        public int GetSize()
        {
            return n;
        }

        public int VectorSize()
        {
            return b1 * b1 + b2 * b2;
        }
        public int ind(int i, int j) //turn matrix to vector - indexing from zero; matrix also starts from (0,0)
        {
            if (i < 0 || j < 0 || i >= n || j >= n) throw new InvalidIndexException();
            if (i < b1 && j < b1)
                return j * b1 + i;
            else if (i >= b1 && j >= b1 && i < GetSize() && j < GetSize())
                return b1 * b1 + ((j - b1) * b2 + (i - b1));
            else
                throw new OutOfBlocks();
        }
        public bool inBlock(int i, int j)
        {
            if (i < 0 || j < 0 || i >= n || j >= n) throw new InvalidIndexException();
            if (i < b1 && j < b1) //in the first diag
                return true;
            else if (i >= b1 && j >= b1) //in the second diag
                return true;
            else
                return false;
        }
        public int getElem(int i, int j)
        {
            if(i < 0 || j < 0 || i>=n || j>=n) throw new InvalidIndexException();
            if (inBlock(i, j))
            {
                return v[ind(i, j)];
            }
            else
            {
                return 0;
            }
        }
        public void setElem(int i, int j, int e)
        {
            if (i < 0 || j < 0 || i >= n || j >= n) throw new InvalidIndexException();
            if(inBlock(i,j))
                v[ind(i, j)] = e;
        }
        public override String ToString()
        {
            String str = "";
            str += n.ToString() + "x" + n.ToString() + "\n";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    str += getElem(i, j).ToString() + "\t";
                }
                str += "\n";
            }
            return str;
        }
        #endregion

        #region Main functions

        public int GetEntry(int i, int j) // getting the entry located at index (i, j) - return num if in any block, else 0
        {
            if(inBlock(i-1, j-1))
                return v[ind(j-1,i-1)]; 
            else
                return 0;
            //subtracting 1 bcs our matrix starts from 0. entry at (1,1) would be Matrix [0,0] = v[0]
        }
        public static Matrix add(Matrix a, Matrix b)
        {
            if (a.GetSize() == b.GetSize())
            {
                Matrix sum = new Matrix(a.b1, a.b2); //zero matrix, same size 
                for (int i = 0; i < a.v.Count; i++)
                {
                    sum.v[i] = a.v[i] + b.v[i];
                }
                return sum;
            }
            else
            {
                throw new DimensionMismatchException();
            }
        }

        public static Matrix mul(Matrix a, Matrix b)
        {
            if (a.GetSize() == b.GetSize())
            {
                Matrix c = new Matrix(a.b1, a.b2);
                for (int i = 0; i < a.GetSize(); i++)
                {
                    for (int j = 0; j < a.GetSize(); j++)
                    {
                        if (a.inBlock(i, j))
                        {
                            int sum = 0;
                            for (int k = 0; k < a.GetSize(); k++)
                            {
                                sum += a.getElem(i, k) * b.getElem(k, j);
                            }
                            c.setElem(i, j, sum);
                        }
                    }
                }
                return c;
            }
            else
            {
                throw new DimensionMismatchException();
            }

        }
        #endregion
    }
}
