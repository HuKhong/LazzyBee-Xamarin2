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
		private SQLiteConnection database;
		private static object collisionLock = new object();
		public SqliteHelper()
		{
			database = DependencyService.Get<IDatabaseConnection>().DbConnection();
		}

		private WordInfo convertWordDAOToWordInfo(WordDAO wordDAO)
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

		private List<WordInfo> convertListWordDAOToListWordInfo(List<WordDAO> listWordDAO)
		{
			List<WordInfo> listWordInfo = new List<WordInfo>();
			foreach (WordDAO wordDAO in listWordDAO)
			{
				listWordInfo.Add(convertWordDAOToWordInfo(wordDAO));
			}

			return listWordInfo;
		}

		public List<WordInfo> getAllWords()
		{
			lock (collisionLock)
			{
				string strQuery = "SELECT * FROM 'vocabulary'";
				List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);
				return convertListWordDAOToListWordInfo(wordDAOs);
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
					wd = convertWordDAOToWordInfo(wordDAOs.ElementAt(0));
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

				return convertListWordDAOToListWordInfo(wordDAOs);
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
				JObject valueJsonObj = JObject.Parse(value);
				var cards = valueJsonObj["cards"];
				string strWordIDList = cards.ToString();

				string major = CommonDefine.DEFAULT_SUBJECT;
				List<WordDAO> wordDAOs = new List<WordDAO>();

				while ((wordDAOs.Count() < cards.Count()))
				{
					//get word object  from vocabulary
					strQuery = string.Format("SELECT * from 'vocabulary'" +
													"WHERE id IN {0} ORDER BY priority desc, level", strWordIDList);
					wordDAOs.AddRange(database.Query<WordDAO>(strQuery));

					//if specilized word is not enough
					if (wordDAOs.Count() < cards.Count() && !major.Equals(CommonDefine.DEFAULT_SUBJECT))
					{
						major = CommonDefine.DEFAULT_SUBJECT;
					}
					else
					{
						break;
					}
				}

				return convertListWordDAOToListWordInfo(wordDAOs);
			}
		}

		public List<WordInfo> getNewWordsList()
		{
			List<WordInfo> wordInfos = fetchWordsFromVocabularyForKey(CommonDefine.PROGRESS_PICKEDWORD_KEY);

			return wordInfos;
		}

		public List<WordInfo> getIncomingList()
		{
			List<WordInfo> wordInfos = fetchWordsFromVocabularyForKey(CommonDefine.PROGRESS_BUFFER_KEY);

			return wordInfos;
		}

		public List<WordInfo> getStudyAgainListWithLimit(int limit)
		{
			string strQuery = string.Format("SELECT * FROM 'vocabulary' where queue = 1 ORDER BY level LIMIT {0}", limit);

			List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);

			return convertListWordDAOToListWordInfo(wordDAOs);
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
				createInreivewListForADay (res);
			}
		}

		private List<WordInfo> _getReviewListFromSystem()
		{
			string strQuery = string.Format("SELECT * from 'system' WHERE key = '{0}'", CommonDefine.PROGRESS_INREVIEW_KEY);
			List<SystemDAO> systemDAOs = database.Query<SystemDAO>(strQuery);
			string value = "";
			if (systemDAOs.Count > 0)
			{
				value = systemDAOs.ElementAt(0).value;
			}

			//parse the result to get word-id list
			JObject valueJsonObj = JObject.Parse(value);
			var cards = valueJsonObj["cards"];
			string strWordIDList = cards.ToString();

			int oldDate = int.Parse(valueJsonObj["date"].ToString());

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
				                         " where id IN {0}", strWordIDList);

				wordDAOs = database.Query<WordDAO>(strQuery);

				return convertListWordDAOToListWordInfo(wordDAOs);
			}

			return null;
		}

		private List<WordInfo> _getReviewListFromVocabulary()
		{
			string totalWordADayInSetting = Common.loadSettingValueByKey(CommonDefine.SETTINGS_TOTAL_CARD_KEY);
			string strQuery = string.Format("SELECT * FROM 'vocabulary'  " +
						                    "where queue = {0} AND due <= {1} ORDER BY level LIMIT {2}", 
						                    WordInfo.QUEUE_REVIEW, DateTimeHelper.getEndOfDayInSec(), totalWordADayInSetting);

			List<WordDAO> wordDAOs = database.Query<WordDAO>(strQuery);

			return convertListWordDAOToListWordInfo(wordDAOs);
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

			dict.Add("date", curDate.ToString());
			dict.Add("card", strListID);
			dict.Add("count", wordInfos.Count().ToString());

			string value = JsonConvert.SerializeObject(dict);
			string strQuery = string.Format("UPDATE 'system' SET value = '{0}' where key = 'inreview'", value);

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
	}
}
