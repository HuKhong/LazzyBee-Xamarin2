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
			var dbName = "DB/english.db";
			string personalFolder =	System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string libraryFolder = Path.Combine(personalFolder, "..", "Library");
			var path = Path.Combine(libraryFolder, dbName);
			return new SQLiteConnection(dbName);
		}
	}
}
