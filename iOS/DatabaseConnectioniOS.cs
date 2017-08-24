using System;
using SQLite;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(LazzyBee.iOS.DatabaseConnectioniOS))]
namespace LazzyBee.iOS
{
	public class DatabaseConnectioniOS : IDatabaseConnection
	{
		public SQLiteConnection DbConnection()
		{
			copyDatabaseToDataFolder();
			string dataPath = Path.Combine(FileHelper.dataFolder, CommonDefine.DATABASENAME);
			return new SQLiteConnection(dataPath);
		}

		private void copyDatabaseToDataFolder()
		{
			string dbName = Path.Combine("DB", CommonDefine.DATABASENAME);
			string dataPath = Path.Combine(FileHelper.dataFolder, CommonDefine.DATABASENAME);

			if (!File.Exists(dataPath))
			{
				File.Copy(dbName, dataPath);
			}
		}
    
	}
}
