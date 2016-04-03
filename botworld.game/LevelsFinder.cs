using System.IO;

namespace botworld.game
{
	internal class LevelsFinder
	{
		public string FindLevelsDir(string startDirPath)
		{
			do
			{
				var levelsDirPath = startDirPath + "\\levels";
				if (Directory.Exists(levelsDirPath))
					return levelsDirPath;
				startDirPath = Path.GetDirectoryName(startDirPath);
			} while (startDirPath != null);
			return null;
		}
	}
}