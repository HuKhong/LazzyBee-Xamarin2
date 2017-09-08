using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

namespace LazzyBee
{
	[Table("system")]
	public class SystemDAO : INotifyPropertyChanged
	{
		private string _key;
		[PrimaryKey]
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

		private string _value;
		public string value
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
		private static SqliteHelper instance;

		public static SqliteHelper Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new SqliteHelper();
				}
				return instance;
			}
		}

		private const string DB_SYSTEM_KEY_CARDS = "cards";
		private const string DB_SYSTEM_KEY_DATE = "date";
		private const string DB_SYSTEM_KEY_COUNT = "count";

		public const string PROGRESS_INREVIEW_KEY = "inreview";
		public const string PROGRESS_BUFFER_KEY = "buffer";
		public const string PROGRESS_PICKEDWORD_KEY = "pickedword";

		public const string DATABASENAME = "english.db";
		public const string DATABASENAME_NEW = "new_english.db";

		private SQLiteConnection database;
		private static object collisionLock = new object();
		private SqliteHelper()
		{
			database = DependencyService.Get<IDatabaseConnection>().DbConnection();
		}

		public List<WordInfo> getAllWords()
		{
			lock (collisionLock)
			{
				string strQuery = "SELECT * FROM 'vocabulary'";
				List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);
				return _convertListWordDAOToListWordInfo(wordDAOs);
			}
		}

		public WordInfo getWordInformation(string word)
		{
			lock (collisionLock)
			{
				string strQuery = string.Format("SELECT * FROM 'vocabulary' " +
												"WHERE question = '{0}'", word);
				List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);
				WordInfo wd = null;

				if (wordDAOs.Count > 0)
				{
					wd = _convertWordDAOToWordInfo(wordDAOs.ElementAt(0));
				}

				return wd;
			}
		}

		public List<WordInfo> getStudiedList()
		{
			lock (collisionLock)
			{
				string strQuery = string.Format("SELECT * FROM 'vocabulary'" +
												" where queue = {0} OR queue = {1} ORDER BY level", WordInfo.QUEUE_REVIEW, WordInfo.QUEUE_DONE);

				List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);

				return _convertListWordDAOToListWordInfo(wordDAOs);
			}
		}

		//fetch word objects from vocabulary by word-id that contained in pickedword or buffer or inreview
		public List<WordInfo> fetchWordsFromVocabularyForKey(string key)
		{
			lock (collisionLock)
			{
				string strQuery = string.Format("SELECT * from 'system' WHERE key = '{0}'", key);
				List<SystemDAO> systemDAOs = database.Query<SystemDAO>(strQuery);
				string value = "";
				if (systemDAOs.Count > 0)
				{
					value = systemDAOs.ElementAt(0).value;
				}

				//parse the result to get word-id list
				if (value != null)
				{
					var valueJsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
					string strWordIDList = valueJsonObj[DB_SYSTEM_KEY_CARDS];
					List<string> listCards = _convertStringToList(strWordIDList);

					string major = CommonDefine.DEFAULT_SUBJECT;
					List<WordDAO> wordDAOs = new List<WordDAO>();

					while ((wordDAOs.Count() < listCards.Count()))
					{
						//get word object  from vocabulary
						strQuery = string.Format("SELECT * from 'vocabulary'" +
												 "WHERE id IN ({0}) ORDER BY priority desc, level", strWordIDList);
						wordDAOs.AddRange(database.Query<WordDAO>(strQuery));

						//if specilized word is not enough
						if (wordDAOs.Count() < listCards.Count() && !major.Equals(CommonDefine.DEFAULT_SUBJECT))
						{
							major = CommonDefine.DEFAULT_SUBJECT;
						}
						else
						{
							break;
						}
					}

					return _convertListWordDAOToListWordInfo(wordDAOs);
				}

				return new List<WordInfo>();
			}
		}

		public List<WordInfo> getNewWordsList()
		{
			List<WordInfo> wordInfos = fetchWordsFromVocabularyForKey(PROGRESS_PICKEDWORD_KEY);

			return wordInfos;
		}

		public List<WordInfo> getIncomingList()
		{
			List<WordInfo> wordInfos = fetchWordsFromVocabularyForKey(PROGRESS_BUFFER_KEY);

			return wordInfos;
		}

		public List<WordInfo> getStudyAgainListWithLimit(int limit)
		{
			string strQuery = string.Format("SELECT * FROM 'vocabulary' where queue = 1 ORDER BY level LIMIT {0}", limit);

			List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);

			return _convertListWordDAOToListWordInfo(wordDAOs);
		}

		public List<WordInfo> getReviewList()
		{
			List<WordInfo> res = _getReviewListFromSystem();
			//if it is yesterday, get new review list from vocabulary
			//res could be empty after completed daily target (so can learn after have completed daily target
			if (res == null || res.Count == 0)
			{
				res = _getReviewListFromVocabulary();

				//save list to db (only word-id)
				_createInreivewListForADay(res);
			}

			return res;
		}

		public List<WordInfo> getSearchHintList(string searchText)
		{
			string strQuery = string.Format("SELECT * FROM 'vocabulary'" +
											" WHERE question like '{0}%%' OR question like '%% {1}%%'" +
											" ORDER BY level LIMIT 20", searchText, searchText);

			List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);

			return _convertListWordDAOToListWordInfo(wordDAOs);
		}

		public List<WordInfo> getSearchResultList(string searchText)
		{
			string strQuery = string.Format("SELECT * FROM 'vocabulary'" +
											" WHERE question like '{0}%%' OR question like '%% {1}%%'" +
											" ORDER BY level", searchText, searchText);

			List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);

			return _convertListWordDAOToListWordInfo(wordDAOs);
		}

		public void updateWord(WordInfo word)
		{
			string formattedAnswer = word.answers.Replace("\'", "\'\'");
			string formattedVN = word.langVN.Replace("\'", "\'\'");
			string formattedEN = word.langEN.Replace("\'", "\'\'");

			string strQuery = string.Format("UPDATE 'vocabulary' SET" +
											" queue = {0}, due = {1}, rev_count = {2}, last_ivl = {3}, e_factor = {4}, answers = '{5}'," +
											" l_vn = '{6}', l_en = '{7}'" +
											" where question = '{8}'",
											int.Parse(word.queue), int.Parse(word.due), int.Parse(word.revCount), int.Parse(word.lastInterval),
											int.Parse(word.eFactor), formattedAnswer, formattedVN, formattedEN, word.question);
			database.Execute(strQuery);
		}

		public void saveNoteForWord(WordInfo word, string note)
		{
			string formattedNote = note.Replace("\'", "\'\'");
			string strQuery = string.Format("UPDATE 'vocabulary' SET user_note = '{0}' where question = '{1}'", formattedNote, word.question);

			database.Execute(strQuery);
		}

		public void insertWordToDatabase(WordInfo word)
		{
			string strQuery = string.Format("SELECT * FROM 'vocabulary' WHERE gid = {0}", word.gid);
			List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);
			int count = wordDAOs.Count;

			string formattedAnswer = word.answers.Replace("\'", "\'\'");
			string formattedVN = word.langVN.Replace("\'", "\'\'");
			string formattedEN = word.langEN.Replace("\'", "\'\'");

			if (count == 0)
			{
				strQuery = string.Format("INSERT INTO 'vocabulary' " +
										 "(question, answers, subcats, status, package, level, queue, due, rev_count," +
										 " last_ivl, e_factor, l_vn, l_en, gid, priority)" +
										 " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', {14})",
										 word.question, formattedAnswer, word.subcats, word.status, word.package,
										 word.level, word.queue, word.due, word.revCount, word.lastInterval, word.eFactor,
										 formattedVN, formattedEN, word.gid, word.priority);

				database.Execute(strQuery);
			}
			else
			{
				strQuery = string.Format("UPDATE 'vocabulary' SET" +
											" queue = {0}, due = {1}, rev_count = {2}, last_ivl = {3}, e_factor = {4}, answers = '{5}'," +
											" l_vn = '{6}', l_en = '{7}'" +
											" where question = '{8}'",
											int.Parse(word.queue), int.Parse(word.due), int.Parse(word.revCount), int.Parse(word.lastInterval),
											int.Parse(word.eFactor), formattedAnswer, formattedVN, formattedEN, word.question);
				database.Execute(strQuery);
			}
		}

		public int getCountOfStudyAgain()
		{
			string strQuery = string.Format("SELECT id FROM 'vocabulary' where queue = {0}", WordInfo.QUEUE_AGAIN);

			List<WordDAO> res = database.Query<WordDAO>(strQuery);

			return res.Count;
		}

		public int getCountOfStudiedWord()
		{
			string strQuery = string.Format("SELECT id FROM 'vocabulary'" +
											" WHERE queue = {0} OR queue = {1}", WordInfo.QUEUE_REVIEW, WordInfo.QUEUE_DONE);

			List<WordDAO> res = database.Query<WordDAO>(strQuery);

			return res.Count;
		}

		/* pick up "amount" news word-ids from vocabulary, then add to buffer 
		   this list is shown in incoming list
		*/
		public void prepareWordsToStudyingQueue(int amount, string package)
		{
			//pick up "amount" news word-ids from vocabulary that not included the old words
			List<WordDAO> resList = new List<WordDAO>();

			string lowestLevel = Common.loadSettingValueByKey(CommonDefine.SETTINGS_MY_LEVEL_KEY);
			string igniredLevel = "7";

			if (!package.Equals(CommonDefine.DEFAULT_SUBJECT))
			{
				igniredLevel = "0";
			}

			if (lowestLevel == null || lowestLevel.Length == 0)
			{
				lowestLevel = CommonDefine.DEFAULT_LEVEL;
			}

			string strQuery = string.Format("SELECT * from 'vocabulary' " +
											"WHERE package LIKE '%%,{0},%%' " +
											"AND queue = {1} AND level >= {2} AND level <> {3} " +
											"ORDER BY priority desc, level LIMIT {4}",
											package, WordInfo.QUEUE_UNKNOWN, lowestLevel, igniredLevel, amount);
			resList.AddRange(database.Query<WordDAO>(strQuery));

			//if the selected package is not enough, get more from common
			if (resList.Count() < amount)
			{
				strQuery = string.Format("SELECT * from 'vocabulary' " +
										 "WHERE package LIKE '%%,{0},%%' AND package NOT LIKE '%%,{1},%%' " +
										 "AND queue = {2} AND level >= {3} AND level <> {4} " +
										 "ORDER BY priority desc, level LIMIT %ld",
										 CommonDefine.DEFAULT_SUBJECT, package, WordInfo.QUEUE_UNKNOWN, lowestLevel, igniredLevel, (amount - resList.Count()));

				resList.AddRange(database.Query<WordDAO>(strQuery));
			}

			//if there is no word with queue = QUEUE_UNKNOWN, reset all words with queue = NEW_WORD to UNKNOWN
			if (resList.Count() < amount)
			{
				strQuery = string.Format("UPDATE 'vocabulary' SET queue = {0} where queue = {1}", WordInfo.QUEUE_UNKNOWN, WordInfo.QUEUE_NEW_WORD);
				database.Execute(strQuery);
			}

			List<string> listID = new List<string>();
			foreach (WordDAO wordDAO in resList)
			{
				listID.Add(wordDAO.id.ToString());
			}

			string strListID = _convertListStringToAString(listID);
			Dictionary<string, string> dict = new Dictionary<string, string>();

			dict.Add(DB_SYSTEM_KEY_CARDS, strListID);
			dict.Add(DB_SYSTEM_KEY_COUNT, resList.Count().ToString());

			string value = JsonConvert.SerializeObject(dict);

			strQuery = string.Format("UPDATE 'system' SET value = '{0}' where key = '{1}'", value, PROGRESS_BUFFER_KEY);

			database.Execute(strQuery);
		}

		/* pick up "amount" word-ids from buffer, then add to pickedword (this list is to study)
		forceFlag: YES: dont need to check date
		*/
		public void pickUpRandom10WordsToStudyingQueue(int amount, bool force)
		{
			string strQuery = string.Format("SELECT * from 'system' WHERE key = '{0}'", PROGRESS_PICKEDWORD_KEY);
			List<SystemDAO> systemDAOs = database.Query<SystemDAO>(strQuery);
			string value = "";

			if (systemDAOs.Count > 0)
			{
				value = systemDAOs.ElementAt(0).value;
			}

			//parse the result to get old date
			Dictionary<string, string> valueJsonObj;
			int oldDate = 0;

			if (value != null)
			{
				valueJsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
				oldDate = int.Parse(valueJsonObj[DB_SYSTEM_KEY_DATE].ToString());
			}
			//compare current date
			int curDate = DateTimeHelper.getBeginOfDayInSec();   //just get time at the begin of day
			int offset = 0;

			if (curDate >= oldDate)
			{
				offset = curDate - oldDate;
			}
			else
			{
				offset = oldDate - curDate;
			}

			if (force == true ||
				(oldDate == 0 || offset > CommonDefine.SECONDS_OF_HALFDAY))
			{

				//reset flag if it's new day
				if (oldDate == 0 ||
					offset > CommonDefine.SECONDS_OF_HALFDAY)
				{
					Common.saveSettingValue(CommonDefine.COMPLETED_FLAG_KEY, "0");
        		}

				strQuery = string.Format("SELECT * from 'system' WHERE key = '{0}'", PROGRESS_BUFFER_KEY);
				systemDAOs = database.Query<SystemDAO>(strQuery);

				if (systemDAOs.Count > 0)
				{
					value = systemDAOs.ElementAt(0).value;
				}

				//parse the result to get word-id list
				string strWordIDList = "";
				List<string> pickedIDList = new List<string>();
				List<string> buffer = new List<string>();

				if (value != null)
				{
					valueJsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
					strWordIDList = valueJsonObj[DB_SYSTEM_KEY_CARDS];
					List<string> listCards = _convertStringToList(strWordIDList);

					int count = 0;

					foreach (string card in listCards)
					{
						buffer.Add(card);
					}

					while (pickedIDList.Count() < amount && count < listCards.Count)
					{

						pickedIDList.Add((string)listCards.ElementAt(count));
						buffer.Remove(listCards.ElementAt(count));
						count++;
					}
				}

				//update "pickedword" field in system
				string strListID = _convertListStringToAString(pickedIDList);
				Dictionary<string, string> dict = new Dictionary<string, string>();

				dict.Add(DB_SYSTEM_KEY_CARDS, strListID);
				dict.Add(DB_SYSTEM_KEY_COUNT, pickedIDList.Count().ToString());
				dict.Add(DB_SYSTEM_KEY_DATE, curDate.ToString());

				value = JsonConvert.SerializeObject(dict);
				strQuery = string.Format("UPDATE 'system' SET value = '{0}' " +
				                         "where key = '{1}'", value, PROGRESS_PICKEDWORD_KEY);

				database.Execute(strQuery);

				//remove these words from buffer
				strListID = _convertListStringToAString(buffer);
				Dictionary<string, string> dictBuffer = new Dictionary<string, string>();

				dictBuffer.Add(DB_SYSTEM_KEY_CARDS, strListID);
				dictBuffer.Add(DB_SYSTEM_KEY_COUNT, buffer.Count().ToString());

				value = JsonConvert.SerializeObject(dictBuffer);
				strQuery = string.Format("UPDATE 'system' SET value = '{0}' " +
										 "where key = '{1}'", value, PROGRESS_BUFFER_KEY);

				database.Execute(strQuery);

				//update queue to NEW_WORD for picked words
				if (pickedIDList.Count() > 0)
				{
					strListID = _convertListStringToAString(pickedIDList);
					strQuery = string.Format("UPDATE 'vocabulary'" +
					                         " SET queue = {0} where id IN ({1})", WordInfo.QUEUE_NEW_WORD, strListID);
					database.Execute(strQuery);
				}
			}
		}

		public int getCountOfPickedWord()
		{
			//get word id from pickedword
			return _getCountOfWordByKey(PROGRESS_PICKEDWORD_KEY);
		}

		public int getCountOfBuffer()
		{
			//get word id from buffer
			return _getCountOfWordByKey(PROGRESS_BUFFER_KEY);
		}

		public int getCountOfInreview()
		{
			//get word id from Inreview
			return _getCountOfWordByKey(PROGRESS_INREVIEW_KEY);
		}

		//update pickedword by wordArr
		public void updatePickedWordList(List<WordInfo> wordInfoList)
		{
			_updateSystemTableForKey(wordInfoList, PROGRESS_PICKEDWORD_KEY);
		}

		//update inreview by wordArr
		public void updateInreviewWordList(List<WordInfo> wordInfoList)
		{
			_updateSystemTableForKey(wordInfoList, PROGRESS_INREVIEW_KEY);
		}

		/******************** PRIVATE FUNCTIONS AREA ********************/
		/* count of words in buffer, pickedword, inreview */
		private int _getCountOfWordByKey(string key)
		{
			string strQuery = string.Format("SELECT * from 'system' WHERE key = '{0}'", key);
			List<SystemDAO> systemDAOs = database.Query<SystemDAO>(strQuery);
			string value = "";
			if (systemDAOs.Count > 0)
			{
				value = systemDAOs.ElementAt(0).value;
			}

			if (value != null)
			{
				//parse the result to get word-id list
				//JObject valueJsonObj = JObject.Parse(value);
				var valueJsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
				var cards = valueJsonObj[DB_SYSTEM_KEY_CARDS];
				int count = int.Parse((string)valueJsonObj[DB_SYSTEM_KEY_COUNT]);
				List<string> listCards = _convertStringToList(cards);

				if (!key.Equals(PROGRESS_INREVIEW_KEY))
				{
					count = listCards.Count();
				}
			}

			return 0;
		}
		/* get list of words from vocabulary by list of ids from system with key "inreview" */
		private List<WordInfo> _getReviewListFromSystem()
		{
			string strQuery = string.Format("SELECT * from 'system' WHERE key = '{0}'", PROGRESS_INREVIEW_KEY);
			List<SystemDAO> systemDAOs = database.Query<SystemDAO>(strQuery);
			string value = "";
			if (systemDAOs.Count > 0)
			{
				value = systemDAOs.ElementAt(0).value;
			}

			//parse the result to get word-id list
			if (value != null)
			{
				var valueJsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
				string strWordIDList = valueJsonObj[DB_SYSTEM_KEY_CARDS];
				if (strWordIDList.Length > 0) 
				{
					int oldDate = int.Parse(valueJsonObj[DB_SYSTEM_KEY_DATE].ToString());

					//compare current date
					int curDate = DateTimeHelper.getBeginOfDayInSec();   //just get time at the begin of day
					int offset = 0;

					if (curDate >= oldDate)
					{
						offset = curDate - oldDate;

					}
					else
					{
						offset = oldDate - curDate;
					}

					List<WordDAO> wordDAOs = null;
					if (offset < DateTimeHelper.SECONDS_OF_DAY)
					{
						//get if it is new. If review list is old, get review list from vocabulary table
						strQuery = string.Format("SELECT * FROM 'vocabulary'" +
						                         " where id IN ({0})", strWordIDList);

						wordDAOs = database.Query<WordDAO>(strQuery);

						return _convertListWordDAOToListWordInfo(wordDAOs);
					}
				}
			}

			return null;
		}

		/* if list id in "inreview" is obsoleted, get new list from vocabulary */
		private List<WordInfo> _getReviewListFromVocabulary()
		{
			string totalWordADayInSetting = Common.loadSettingValueByKey(CommonDefine.SETTINGS_TOTAL_CARD_KEY);
			string strQuery = string.Format("SELECT * FROM 'vocabulary'  " +
						                    "where queue = {0} AND due <= {1} ORDER BY level LIMIT {2}", 
						                    WordInfo.QUEUE_REVIEW, DateTimeHelper.getEndOfDayInSec(), totalWordADayInSetting);

			List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);

			return _convertListWordDAOToListWordInfo(wordDAOs);
		}

		/* update "inreview" key in system table */
		private WordInfo _convertWordDAOToWordInfo(WordDAO wordDAO)
		{
			WordInfo wordInfo = new WordInfo();
			wordInfo.answers = wordDAO.answers;
			wordInfo.due = wordDAO.due.ToString();
			wordInfo.eFactor = wordDAO.e_factor.ToString();
			wordInfo.gid = wordDAO.gid.ToString();
			wordInfo.langEN = wordDAO.l_en;
			wordInfo.langVN = wordDAO.l_vn;
			wordInfo.lastInterval = wordDAO.last_ivl.ToString();
			wordInfo.level = wordDAO.level.ToString();
			wordInfo.package = wordDAO.package;
			wordInfo.priority = wordDAO.priority;
			wordInfo.question = wordDAO.question;
			wordInfo.queue = wordDAO.queue.ToString();
			wordInfo.revCount = wordDAO.rev_count.ToString();
			wordInfo.status = wordDAO.status.ToString();
			wordInfo.subcats = wordDAO.subcats;
			wordInfo.userNote = wordDAO.user_note;
			wordInfo.wordid = wordDAO.id.ToString();

			return wordInfo;
		}

		private List<WordInfo> _convertListWordDAOToListWordInfo(List<WordDAO> listWordDAO)
		{
			List<WordInfo> listWordInfo = new List<WordInfo>();

			if (listWordDAO != null)
			{
				foreach (WordDAO wordDAO in listWordDAO)
				{
					listWordInfo.Add(_convertWordDAOToWordInfo(wordDAO));
				}
			}

			return listWordInfo;
		}
		private void _createInreivewListForADay(List<WordInfo> wordInfos)
		{
			List<string> listID = new List<string>();
			foreach (WordInfo wordInfo in wordInfos)
			{
				listID.Add(wordInfo.wordid);
			}

			string strListID = _convertListStringToAString(listID);
			int curDate = DateTimeHelper.getBeginOfDayInSec();
			Dictionary<string, string> dict = new Dictionary<string, string>();

			dict.Add(DB_SYSTEM_KEY_DATE, curDate.ToString());
			dict.Add(DB_SYSTEM_KEY_CARDS, strListID);
			dict.Add(DB_SYSTEM_KEY_COUNT, wordInfos.Count().ToString());

			string value = JsonConvert.SerializeObject(dict);
			string strQuery = string.Format("UPDATE 'system' SET value = '{0}' where key = '{1}'", value, PROGRESS_INREVIEW_KEY);

			database.Execute(strQuery);

		}

		private string _convertListStringToAString(List<string>listString)
		{
			string joined = string.Join(",", listString);

			return joined;
		}

		private List<string> _convertStringToList(string text)
		{
			List<string> list = text.Split(',').ToList();

			return list;
		}

		private void _updateSystemTableForKey(List<WordInfo> wordInfoList, string key)
		{
			List<string> listID = new List<string>();
			foreach (WordInfo wordInfo in wordInfoList)
			{
				listID.Add(wordInfo.wordid);
			}
			string strListID = _convertListStringToAString(listID);

			string strQuery = string.Format("SELECT * from 'system' WHERE key = '{0}'", key);
			List<SystemDAO> systemDAOs = database.Query<SystemDAO>(strQuery);
			string value = "";
			if (systemDAOs.Count > 0)
			{
				value = systemDAOs.ElementAt(0).value;
			}

			//parse the result to get word-id list
			var valueJsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
			string count = "0";
			int oldDate = int.Parse(valueJsonObj[DB_SYSTEM_KEY_DATE].ToString());

			Dictionary<string, string> dict = new Dictionary<string, string>();

			dict.Add(DB_SYSTEM_KEY_CARDS, strListID);
			dict.Add(DB_SYSTEM_KEY_DATE, oldDate.ToString());

			if (key.Equals(PROGRESS_INREVIEW_KEY))
			{
				count = (string)valueJsonObj[DB_SYSTEM_KEY_COUNT];
				dict.Add(DB_SYSTEM_KEY_COUNT, count.ToString());   //keep this value even if removing a word from "cards"
			}

			value = JsonConvert.SerializeObject(dict);
			strQuery = string.Format("UPDATE 'system' SET value = '{0}' " +
									 "where key = '{1}'", value, key);

			database.Execute(strQuery);
		}
	}
}
