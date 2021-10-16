namespace GameOfLife
{
	public interface IFileSystem
	{
		string ReadAllText(string filePath);
		void WriteAllText(string filePath, string text);
		void DeleteFilesInDirectory(string directory, string pattern);
	}
}
