using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
	class Program
	{
		static void Main(string[] args)
		{
			var numberOfGenerations = int.Parse(args[0]);
			var fileName = args[1];
			World.Evolve(numberOfGenerations, fileName, () => { });
		}
	}
}
