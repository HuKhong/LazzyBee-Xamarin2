using System;
using System.IO;

namespace LazzyBee
{
	public class FileHelper
	{
		public FileHelper()
		{
		}

		public static string libraryFolder
		{
			get
			{
				string personalPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string libraryPath = Path.Combine(personalPath, "..", "Library");

				if (!Directory.Exists(libraryPath))
				{
					Directory.CreateDirectory(libraryPath);
				}

				return libraryPath;
			}
		}

		public static string trashFolder
		{
			get
			{
				string trashPath = Path.Combine(libraryFolder, "Trash Folder");

				if (!Directory.Exists(trashPath))
				{
					Directory.CreateDirectory(trashPath);
				}

				return trashPath;
			}
		}

		public static string dataFolder
		{
			get
			{
				string dataPath = Path.Combine(libraryFolder, "Data");

				if (!Directory.Exists(dataPath))
				{
					Directory.CreateDirectory(dataPath);
				}

				return dataPath;
			}
		}


		public static string backupFolder
		{
			get
			{
				string backupPath = Path.Combine(libraryFolder, "Backup");

				if (!Directory.Exists(backupPath))
				{
					Directory.CreateDirectory(backupPath);
				}

				return backupPath;
			}

		}

		public static string restoreFolder
		{
			get
			{
				string restorePath = Path.Combine(libraryFolder, "Restore");

				if (!Directory.Exists(restorePath))
				{
					Directory.CreateDirectory(restorePath);
				}

				return restorePath;
			}

		}

		public static string tempFolder
		{
			get
			{
				string tempPath = Path.Combine(libraryFolder, "Temp Folder");

				if (!Directory.Exists(tempPath))
				{
					Directory.CreateDirectory(tempPath);
				}

				return tempPath;

			}
		}
	}
}
