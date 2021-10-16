using System;
using System.IO;
using System.Linq;

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
		public int Generation { get; set; }
		public string Name { get; set; }

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

		public static void Evolve(int numberOfGenerations, string seedWorldFilename, Action onGeneration, IFileSystem fileSystem)
		{
			var currentWorld = FromFile(seedWorldFilename, fileSystem);
			currentWorld.DeleteOldFiles(fileSystem);

			for (var i = 1; i <= numberOfGenerations; i++)
			{
				currentWorld = currentWorld.CalculateNextGeneration();
				currentWorld.Save(fileSystem);
				onGeneration();
			}
		}

		private void DeleteOldFiles(IFileSystem fileSystem)
		{
			fileSystem.DeleteFilesInDirectory(".", $"{Name}-*.txt");
		}

		public static World FromFile(string filename, IFileSystem fileSystem)
		{
			var fileContent = fileSystem.ReadAllText(filename);
			var world = FromString(fileContent);
			world.Name = Path.GetFileNameWithoutExtension(filename);
			world.Generation = 0;
			return world;
		}

		public static World FromString(string serializedWorld)
		{
			var lines = serializedWorld.Split('\n');
			var worldWidth =
				lines.Aggregate(0, (maximumLength, currentString) => Math.Max(maximumLength, currentString.Trim().Length));
			var worldHeight = lines.Count(line => !string.IsNullOrWhiteSpace(line));

			var world = new World(worldWidth, worldHeight);
			for (var y = 0; y < worldHeight; y++)
			{
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

		public void Save(IFileSystem fileSystem)
		{
			var filename = $"{Name}-{Generation}.txt";
			fileSystem.WriteAllText(filename, this.ToString());
		}

		public World CalculateNextGeneration()
		{
			var nextGeneration = new World(Width, Height)
			{
				Name = Name,
				Generation = Generation + 1
			};

			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
				{
					var numberOfNeighbors = GetNumberOfAliveNeighbors(x, y);
					var currentState = this[x, y];
					nextGeneration[x, y] = CalculateNextState(currentState, numberOfNeighbors);
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

		private static State CalculateNextState(State oldState, int numberOfNeighbors)
		{
			switch (oldState)
			{
				case State.Alive when numberOfNeighbors < 2:
				case State.Alive when numberOfNeighbors > 3:
					return State.Dead;
				case State.Alive:
					return State.Alive;
				case State.Dead when numberOfNeighbors == 3:
					return State.Alive;
				case State.Dead:
				default:
					return State.Dead;
			}
		}
	}
}