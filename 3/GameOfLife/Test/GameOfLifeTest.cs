using System;
using FluentAssertions;
using Xunit;

using GameOfLife;

namespace Test
{
	public class GameOfLifeTest
	{
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
