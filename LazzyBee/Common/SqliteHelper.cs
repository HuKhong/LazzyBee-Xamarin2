using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LazzyBee
{
	[Table("system")]
	public class SystemDAO : INotifyPropertyChanged
	{
		private string _key;
		public string key
		{
			get
			{
				return _key;
			}
			set
			{
				this._key = value;
				OnPropertyChanged(nameof(key));
			}
		}

		private int _value;
		[PrimaryKey, AutoIncrement]
		public int value
		{
			get
			{
				return _value;
			}
			set
			{
				this._value = value;
				OnPropertyChanged(nameof(value));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class SqliteHelper
	{
		private SQLiteConnection database;
		private static object collisionLock = new object();
		public SqliteHelper()
		{
			database = DependencyService.Get<IDatabaseConnection>().DbConnection();
		}

		public List<WordDAO> getAllWords()
		{
			lock (collisionLock)
			{
				string strQuery = "SELECT * FROM 'vocabulary'";
				return database.Query<WordDAO>(strQuery);
			}
		}

		public WordInfo getWordInformation(string word)
		{
			lock (collisionLock)
			{
				string strQuery = string.Format("SELECT * FROM 'vocabulary' " +
				                                "WHERE question = '{0}'", word);
				List<WordInfo> words = database.Query<WordInfo>(strQuery);
				WordInfo wd = null;

				if (words.Count > 0)
				{
					wd = words.ElementAt(0);
				}

				return wd;
			}
		}

		public List<WordInfo> getStudiedList()
		{
			lock (collisionLock)
			{
				string strQuery = string.Format("SELECT id, question, answers, subcats, status, package, level, queue, due, " +
				                                "rev_count, last_ivl, e_factor, l_vn, l_en, gid, user_note, priority FROM 'vocabulary'" +
				                                " where queue = {0} OR queue = {1} ORDER BY level", WordInfo.QUEUE_REVIEW, WordInfo.QUEUE_DONE);
				return database.Query<WordInfo>(strQuery);
			}
		}

		//fetch word objects from vocabulary by word-id that contained in pickedword or buffer or inreview
		//public List<SystemDAO> fetchWordsFromVocabularyForKey(string key)
		//{
		//	lock (collisionLock)
		//	{
		//		string strQuery = string.Format("SELECT value from 'system' WHERE key = '{0}'", key);
		//		List<SystemDAO> value = database.Query<T>(strQuery);
		//	}
		//}
	}
}
