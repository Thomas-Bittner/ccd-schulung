using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace GameOfLife
{
	public class World
	{
		public enum State
		{
			Dead,
			Alive
		}

		private readonly State[,] _map;

		public World(int width, int height)
		{
			Width = width;
			Height = height;
			_map = new State[width, height];
		}

		public int Width { get; }
		public int Height { get; }

		public State this[int x, int y]
		{
			get
			{
				if (x < 0 || x >= Width || y < 0 || y >= Height)
					return State.Dead;
				return _map[x, y];
			}
			set => _map[x, y] = value;
		}

		public static void Evolve(int numberOfGenerations, string seedWorldFilename, Action onGeneration)
		{
			var fileBaseName = Path.GetFileNameWithoutExtension(seedWorldFilename);
			DeleteOldFiles(fileBaseName);

			var currentWorld = FromFile(seedWorldFilename);
			for (var i = 1; i <= numberOfGenerations; i++)
			{
				currentWorld = currentWorld.CalculateNextGeneration();
				currentWorld.Save($"{fileBaseName}-{i}.txt");
				onGeneration();
			}
		}

		private static void DeleteOldFiles(string baseName)
		{
			var directory = new DirectoryInfo(".");
			foreach (var file in directory.EnumerateFiles($"{baseName}-*.txt"))
			{
				file.Delete();
			}
		}

		public static World FromFile(string filename)
		{
			var fileContent = File.ReadAllText(filename);
			return FromString(fileContent);
		}

		public static World FromString(string serializedWorld)
		{
			var lines = serializedWorld.Split('\n');
			var worldWidth =
				lines.Aggregate(0, (maximumLength, currentString) => Math.Max(maximumLength, currentString.Trim().Length));
			var worldHeight = lines.Count(line => !string.IsNullOrWhiteSpace(line));

			var world = new World(worldWidth, worldHeight);
			for (var y = 0; y < worldHeight; y++)
			for (var x = 0; x < worldWidth; x++)
				switch (lines[y][x])
				{
					case 'X':
						world[x, y] = State.Alive;
						break;
					case '.':
						world[x, y] = State.Dead;
						break;
				}

			return world;
		}

		public override string ToString()
		{
			var result = "";
			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
					switch (_map[x, y])
					{
						case State.Alive:
							result += "X";
							break;
						case State.Dead:
							result += ".";
							break;
					}

				result += '\n';
			}

			return result.Trim();
		}

		public void Save(string filename)
		{
			File.WriteAllText(filename, this.ToString());
		}

		public World CalculateNextGeneration()
		{
			var nextGeneration = new World(Width, Height);
			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
				{
					var numberOfNeighbors = GetNumberOfAliveNeighbors(x, y);
					switch (_map[x, y])
					{
						case State.Alive when numberOfNeighbors < 2:
						case State.Alive when numberOfNeighbors > 3:
							nextGeneration[x, y] = State.Dead;
							break;
						case State.Alive:
							nextGeneration[x, y] = State.Alive;
							break;
						case State.Dead when numberOfNeighbors == 3:
							nextGeneration[x,y] = State.Alive;
							break;
						case State.Dead:
							nextGeneration[x, y] = State.Dead;
							break;
					}
				}
			}

			return nextGeneration;
		}

		private int GetNumberOfAliveNeighbors(int x, int y)
		{
			var neighbors = new[]
			{
				this[x - 1, y - 1],
				this[x - 1, y],
				this[x - 1, y + 1],
				this[x, y - 1],
				this[x, y + 1],
				this[x + 1, y - 1],
				this[x + 1, y],
				this[x + 1, y + 1]
			};
			return neighbors.Count(state => state == State.Alive);
		}
	}
}