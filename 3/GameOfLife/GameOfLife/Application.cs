using System;

namespace GameOfLife
{
	public class Application
	{
		private readonly int numberOfGenerations;
		private readonly string sourceWorldFilePath;

		public Action OnGeneration { get; set; } = () => {};
		public IFileSystem FileSystem { get; set; } = new FileSystem();

		public Application(int numberOfGenerations, string sourceWorldFilePath)
		{
			this.numberOfGenerations = numberOfGenerations;
			this.sourceWorldFilePath = sourceWorldFilePath;
		}

		public void Run()
		{
			World.Evolve(numberOfGenerations, sourceWorldFilePath, OnGeneration, FileSystem);
		}
	}
}
