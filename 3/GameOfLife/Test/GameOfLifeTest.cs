using System;
using System.IO;
using FluentAssertions;
using Xunit;

using GameOfLife;

namespace Test
{
	public class GameOfLifeTest
	{
		[Fact]
		public void AcceptenceTest_Block()
		{
			File.WriteAllText("block.txt", "....\n.XX.\n.XX.\n....");
			var callbackCalled = 0;

			GameOfLife.World.Evolve(1, "block.txt", () => { callbackCalled++; });
			callbackCalled.Should().Be(1);
			File.Exists("block-1.txt").Should().Be(true);
			File.ReadAllText("block-1.txt").Should().Be("....\n.XX.\n.XX.\n....");
		}

		[Fact]
		public void AcceptenceTest_Blinker2()
		{
			File.WriteAllText("blinker.txt", ".....\n.....\n.XXX.\n.....\n.....");
			var callbackCalled = 0;

			GameOfLife.World.Evolve(2, "blinker.txt", () => { callbackCalled++; });
			callbackCalled.Should().Be(2);
			File.Exists("blinker-1.txt").Should().Be(true);
			File.Exists("blinker-2.txt").Should().Be(true);
			File.ReadAllText("blinker-1.txt").Should().Be(".....\n..X..\n..X..\n..X..\n.....");
			File.ReadAllText("blinker-2.txt").Should().Be(".....\n.....\n.XXX.\n.....\n.....");
		}

		[Fact]
		public void AcceptenceTest_Blinker1()
		{
			File.WriteAllText("blinker.txt", ".....\n.....\n.XXX.\n.....\n.....");
			File.WriteAllText("blinker-2.txt", ".....\n.....\n.XXX.\n.....\n.....");
			var callbackCalled = 0;

			GameOfLife.World.Evolve(1, "blinker.txt", () => { callbackCalled++; });
			callbackCalled.Should().Be(1);
			File.Exists("blinker-1.txt").Should().Be(true);
			File.ReadAllText("blinker-1.txt").Should().Be(".....\n..X..\n..X..\n..X..\n.....");
			File.Exists("blinker-2.txt").Should().Be(false);
		}

		[Fact]
		public void FromString_ShouldReturnEmptyWorldForEmptyString()
		{
			var world = World.FromString("");
			world.Width.Should().Be(0);
			world.Height.Should().Be(0);
		}

		[Fact]
		public void FromString_SingleLivingCell()
		{
			var world = World.FromString("X");
			world.Width.Should().Be(1);
			world.Height.Should().Be(1);
			world[0, 0].Should().Be(World.State.Alive);
		}

		[Fact]
		public void FromString_SingleDeadCell()
		{
			var world = World.FromString(".");
			world.Width.Should().Be(1);
			world.Height.Should().Be(1);
			world[0, 0].Should().Be(World.State.Dead);
		}

		[Fact]
		public void FromString_ComplexWorld()
		{
			var world = World.FromString(".X.\nX.X");
			world.Width.Should().Be(3);
			world.Height.Should().Be(2);
			world[0, 0] = World.State.Dead;
			world[1, 0] = World.State.Alive;
			world[2, 0] = World.State.Dead;
			world[0, 1] = World.State.Alive;
			world[1, 1] = World.State.Dead;
			world[2, 1] = World.State.Alive;
		}

		[Theory]
		[InlineData("...\n.X.\n...", "...\n...\n...")]
		[InlineData(".X.\n.X.\n.X.", "...\nXXX\n...")]
		public void CalculateNextGeneration(string before, string expectedAfter)
		{
			var world = World.FromString(before);
			var nextWorld = world.CalculateNextGeneration();
			nextWorld.ToString().Should().Be(expectedAfter);
		}
	}
}
