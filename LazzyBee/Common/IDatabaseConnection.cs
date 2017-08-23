using System;
using SQLite;

namespace LazzyBee
{
	public interface IDatabaseConnection
	{
		SQLite.SQLiteConnection DbConnection();
	}
}
