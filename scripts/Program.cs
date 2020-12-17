using DmcSocial;
using Microsoft.VisualBasic;
using System;
using System.Linq;

namespace scripts
{
  class Program
  {
    static void Main(string[] args)
    {
      var method = args[0];
      var methodArgs = args.Skip(1).ToArray();
      switch (method)
      {
        case "calc-words":
          new WordFrequencyCalculator().Run(methodArgs);
          return;
        case "add-posts":
          new AddMediumPost().Run(methodArgs);
          return;
        case "calc-tag-correlation":
          new CalculateCorellcationCoefficient(methodArgs).Run();
          return;
        case "calc-tag-popularity":
          new CalculateTagPopularity(methodArgs[0]).Run();
          return;
      }
    }
  }
}
