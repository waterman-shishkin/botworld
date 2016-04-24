using System;
using System.IO;
using System.Reflection;
using botworld.bl;
using botworld.ogmo;
using botworld.otter;

namespace botworld.game
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				if (args.Length != 1)
					throw new InvalidOperationException("В командной строке не передано имя тестового уровня");

				var levelFileName = args[0];
				var levelsFinder = new LevelsFinder();
				var applicationDirPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
				var levelsDirPath = levelsFinder.FindLevelsDir(applicationDirPath);
				if (levelsDirPath == null)
					throw new InvalidOperationException("Не найдена папка 'levels' с описаниями уровней");

				var projectFilePath = levelsDirPath + "\\botworld.oep";
				if (!File.Exists(projectFilePath))
					throw new InvalidOperationException("Не найден файл 'botworld.oep' с описанием проекта");

				var levelFilePath = levelsDirPath + "\\" + levelFileName + ".oel";
				if (!File.Exists(levelFilePath))
					throw new InvalidOperationException("Не найден файл '" + levelFileName + ".oel' с описанием уровня");

				var gameLoader = new GameLoader();
				var keysSequenceSource = new OtterKeysSequenceSource();
				var game = gameLoader.Load(projectFilePath, levelFilePath, keysSequenceSource);
				var levelSettings = gameLoader.LevelSettings;
				var gameView = new GameView("Botworld: " + levelSettings.Title, game, levelSettings.CellSize,
					levelSettings.BackgroundColor, levelSettings.EntitiesImageFilenames, levelsDirPath);
				gameView.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine("Ошибка: {0}", e);
				Console.ReadKey();
			}
		}
	}
}
