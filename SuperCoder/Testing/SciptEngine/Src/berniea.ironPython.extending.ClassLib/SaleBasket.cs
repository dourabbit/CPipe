using System.Collections.Generic;
using System.Linq;
using berniea.ironPython.extending.ClassLib;

namespace berniea.ironPython.extending.ClassLib
{
  public class SaleBasket
  {
    public IList<Line> Lines { get; set; }
    public double Total
    {
      get
      {
        double result = 0;
        if (Lines.Count > 0)
          result = Lines.Sum(line => line.Amount);
        return result;
      }
    }
  }
}