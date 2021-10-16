namespace GameOfLife
{
	class Program
	{
		static void Main(string[] args)
		{
			var numberOfGenerations = int.Parse(args[0]);
			var fileName = args[1];

			var application = new Application(numberOfGenerations, fileName);
			application.Run();
		}
	}
}
