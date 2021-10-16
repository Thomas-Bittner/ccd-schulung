using System.IO;

namespace GameOfLife
{
	public class FileSystem : IFileSystem
	{
		public string ReadAllText(string filePath)
			=> File.ReadAllText(filePath);

		public void WriteAllText(string filePath, string content)
			=> File.WriteAllText(filePath, content);

		public void DeleteFilesInDirectory(string directoryPath, string pattern)
		{
			var directory = new DirectoryInfo(directoryPath);
			foreach (var file in directory.EnumerateFiles(pattern))
				file.Delete();
		}
	}
}
