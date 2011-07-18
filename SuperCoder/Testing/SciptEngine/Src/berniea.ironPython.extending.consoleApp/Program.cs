using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using berniea.ironPython.extending.ClassLib;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace berniea.ironPython.extending.consoleApp
{
  class Program
  {
    static void Main(string[] args)
    {
      ExecuteExpression();
      ExecuteExpressoinWithEntity();
      ExecuteBusinessRules();
    }

    static void ExecuteExpression()
    {
      string code = @"100 * 2 + 4 / 3";

      ScriptEngine engine = Python.CreateEngine();
      ScriptSource source =
        engine.CreateScriptSourceFromString(code, SourceCodeKind.Expression);

      int res = source.Execute<int>();
      Console.WriteLine(res);
    }

    static void ExecuteExpressoinWithEntity()
    {
      ScriptEngine engine = Python.CreateEngine();
      ScriptScope scope = engine.CreateScope();

      string code = @"emp.Salary * 0.3";

      ScriptSource source =
        engine.CreateScriptSourceFromString(code, SourceCodeKind.Expression);

      var emp =
        new Employee { Id = 1000, Name = "Bernie", Salary = 1000 };

      scope.SetVariable("emp", emp);
      var res = (double)source.Execute(scope);
      Console.WriteLine(res);
    }

    static void ExecuteBusinessRules()
    {
      ScriptEngine engine = Python.CreateEngine();
      ScriptRuntime runtime = engine.Runtime;
      ScriptScope scope = engine.CreateScope();
      ScriptSource source;

      #region Setup

      var saleBasket = new SaleBasket
       {
         Lines = new List<Line>
                 {
                  new Line { ProductName = "Prod1", ProductPrice = 100, Quantity = 2, Amount = 100 * 2},
                  new Line { ProductName = "Prod2", ProductPrice = 20, Quantity = 1, Amount = 20 * 1},
                  new Line { ProductName = "Prod3", ProductPrice = 45.8, Quantity = 2, Amount = 45.8 * 2},
                  new Line { ProductName = "Prod4", ProductPrice = 3.9, Quantity = 10, Amount = 3.9 * 10},
                  new Line { ProductName = "Prod5", ProductPrice = 555.5, Quantity = 10, Amount = 555.5 * 10}
                 }
       };

      #endregion

      #region Find python files

      string rootDir = AppDomain.CurrentDomain.BaseDirectory;
      string rulesDir = Path.Combine(rootDir, "Rules");
      
      var files = new List<string>();
      
      foreach (string path in Directory.GetFiles(rulesDir))
        if (path.ToLower().EndsWith(".py"))
          files.Add(path);

      #endregion
      
      Console.WriteLine(saleBasket.Total);
      
      AddAssemblies(runtime);
      
      scope.SetVariable("saleBasket", saleBasket);
      
      foreach (var file in files)
      {
        source = engine.CreateScriptSourceFromFile(file);
        source.Execute(scope);
      }
      
      Console.WriteLine(saleBasket.Total);
      
      #region not in use

//      string code = @"def Rule(a=None):                          
//                          if a is not None:
//                            print a
//                          else:
//                            print 'aaaaa'
//                          #return True";

      //ScriptSource source =
      //  engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);

      //var emp =
      //  new Employee { Id = 1000, Name = "Bernie", Salary = 1000 };

      //scope.SetVariable("emp", emp);
      //object res = source.Execute(scope);

      //object res1 = scope.Execute(code);

      //CompiledCode _code = source.Compile();

      ////scope.SetVariable("a", new object());
      //object obj = _code.Execute(scope);
      //var rule1 = scope.GetVariable<Action>("Rule");
      //rule1();
      ////bool res = (bool)rule1();

      #endregion

      Console.WriteLine();
    }

    private static void AddAssemblies(ScriptRuntime runtime)
    {
      string path = Assembly.GetExecutingAssembly().Location;
      string dir = Directory.GetParent(path).FullName;
      string libPath = Path.Combine(dir, "berniea.ironPython.extending.ClassLib.dll");
      
      Assembly assembly = Assembly.LoadFile(libPath);
      runtime.LoadAssembly(assembly);
    }
  }
}
