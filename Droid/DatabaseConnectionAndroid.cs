using SQLite;
using LocalDataAccess.Droid;
using System.IO;
[assembly: Xamarin.Forms.Dependency(typeof(LazzyBee.Droid.DatabaseConnectionAndroid))]
namespace LazzyBee.Droid
{
	public class DatabaseConnectionAndroid : IDatabaseConnection
	{
		public SQLiteConnection DbConnection()
		{
			var dbName = "english.db";
			var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
			return new SQLiteConnection(path);
		}
	}
}
