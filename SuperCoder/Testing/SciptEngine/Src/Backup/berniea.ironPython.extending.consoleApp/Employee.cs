using System;

namespace berniea.ironPython.extending.consoleApp
{
  public class Employee
  {
    public string Name { get; set; }
    public int Id { get; set; }
      
    private int _salary;

    public int Salary
    {
      get { return _salary; }
      set { _salary = value; }
    }

    public int GetSalary()
    {
      Console.WriteLine("GetSalary");
      return _salary;
    }
  }
}