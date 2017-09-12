using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LazzyBee
{
	public class Common
	{
		public Common()
		{
		}

		/* save/load/remove settings */
		private static void saveSettingValue(string key, string value) {
			if (key != null && value != null &&
			    key.Length > 0 && value.Length > 0)
			{
				Application.Current.Properties[key] = value;
			}
		}

		private static string loadSettingValueByKey (string key) {
    		if (Application.Current.Properties.ContainsKey(key))
			{
			    string value = Application.Current.Properties[key] as string;
			    
				return value;
			}

			return null;
		}

		public static void clearSettingValueByKey(string key)
		{
			if (Application.Current.Properties.ContainsKey(key))
			{
				Application.Current.Properties.Remove(key);
			}
		}

		public static bool checkKey(string key)
		{
			if (Application.Current.Properties.ContainsKey(key))
			{
				return true;
			}

			return false;
		}

		//save/load daily target
		public static int loadDailyTarget()
		{
			int res = 0;

			if (Common.checkKey(CommonDefine.SETTINGS_NEW_CARD_KEY) == true)
			{
				res = int.Parse(loadSettingValueByKey(CommonDefine.SETTINGS_NEW_CARD_KEY));
			}

			return res;
		}

		public static void saveDailyTarget(int target)
		{
			if (target >= 0 && target <= 30)
			{
				saveSettingValue(CommonDefine.SETTINGS_NEW_CARD_KEY, target.ToString());
			}
		}

		//save/laod max total
		public static int loadTotalTarget()
		{
			int res = 0;

			if (Common.checkKey(CommonDefine.SETTINGS_TOTAL_CARD_KEY) == true)
			{
				res = int.Parse(loadSettingValueByKey(CommonDefine.SETTINGS_TOTAL_CARD_KEY));
			}

			return res;
		}

		public static void saveTotalTarget(int target)
		{
			if (target >= 0 && target <= 100)
			{
				saveSettingValue(CommonDefine.SETTINGS_TOTAL_CARD_KEY, target.ToString());
			}
		}

		//save/load major
		public static void saveMajorToProperties(MajorObject major)
		{
			string strMajor = JsonConvert.SerializeObject(major);

			if (strMajor != null)
			{
				saveSettingValue(CommonDefine.SELECTED_MAJOR_KEY, strMajor);
			}
		}

		public static MajorObject loadMajorFromProperties()
		{
			if (Common.checkKey(CommonDefine.SELECTED_MAJOR_KEY) == true)
			{
				string strMajor = loadSettingValueByKey(CommonDefine.SELECTED_MAJOR_KEY);
				MajorObject major = JsonConvert.DeserializeObject<MajorObject>(strMajor);

				return major;
			}

			return null;
		}

		//save/load selected level
		public static string loadSelectedLevel()
		{
			if (Common.checkKey(CommonDefine.SETTINGS_MY_LEVEL_KEY) == true)
			{
				string strLevel = loadSettingValueByKey(CommonDefine.SETTINGS_MY_LEVEL_KEY);

				return strLevel;
			}

			return "";
		}

		public static void saveSelectedLevel(string lv)
		{
			if (lv != null)
			{
                saveSettingValue(CommonDefine.SETTINGS_MY_LEVEL_KEY, lv);
			}
		}

		//save/load speaking speed
		public static double loadSpeakingSpeed()
		{
			if (Common.checkKey(CommonDefine.SETTINGS_TTS_SPEED_KEY) == true)
			{
				string strSpeed = loadSettingValueByKey(CommonDefine.SETTINGS_TTS_SPEED_KEY);

				return double.Parse(strSpeed);
			}

			return double.Parse(CommonDefine.DEFAULT_TTS_SPEED);
		}

		public static void saveSpeakingSpeed(double speed)
		{
			if (speed > 0 && speed < 3)
			{
				saveSettingValue(CommonDefine.SETTINGS_TTS_SPEED_KEY, speed.ToString());
			}
		}

		//autoplay sound
		public static bool loadAutoplayFlag()
		{
			if (Common.checkKey(CommonDefine.SETTINGS_AUTOPLAY_KEY) == true)
			{
				string strFlag = loadSettingValueByKey(CommonDefine.SETTINGS_AUTOPLAY_KEY);

				return bool.Parse(strFlag);
			}

			return bool.Parse(CommonDefine.DEFAULT_AUTO_PLAY_SOUND);
		}

		public static void saveAutoplayFlag(bool flag)
		{
			saveSettingValue(CommonDefine.SETTINGS_AUTOPLAY_KEY, flag.ToString());
		}

		//display meaning
		public static bool loadDisplayMeaningFlag()
		{
			if (Common.checkKey(CommonDefine.SETTINGS_DISPLAY_MEANING_KEY) == true)
			{
				string strFlag = loadSettingValueByKey(CommonDefine.SETTINGS_DISPLAY_MEANING_KEY);

				return bool.Parse(strFlag);
			}

			return bool.Parse(CommonDefine.DEFAULT_DISPLAY_MEANING);
		}

		public static void saveDisplayMeaningFlag(bool flag)
		{
			saveSettingValue(CommonDefine.SETTINGS_DISPLAY_MEANING_KEY, flag.ToString());
		}

		//save/load time to show answer
		public static int loadWaitingTimeToShowAnswer()
		{
			int res = 0;

			if (Common.checkKey(CommonDefine.SETTINGS_TIME_SHOW_ANSWER_KEY) == true)
			{
				res = int.Parse(loadSettingValueByKey(CommonDefine.SETTINGS_TIME_SHOW_ANSWER_KEY));
			}

			return res;
		}

		public static void saveWaitingTimeToShowAnswer(int sec)
		{
			if (sec >= 0 && sec <= 20)
			{
				saveSettingValue(CommonDefine.SETTINGS_TIME_SHOW_ANSWER_KEY, sec.ToString());
			}
		}

		//save/load remind time
		public static string loadRemindTime()
		{
			if (Common.checkKey(CommonDefine.SETTINGS_NOTIFICATION_TIME_KEY) == true)
			{
				string strTime = loadSettingValueByKey(CommonDefine.SETTINGS_NOTIFICATION_TIME_KEY);

				return strTime;
			}

			return CommonDefine.DEFAULT_TIME_SHOW_NOTIFICATION;
		}

		public static void saveRemindTime(string time)
		{
			//need to check time format, do this later
			if (time != null)
			{
				saveSettingValue(CommonDefine.SETTINGS_NOTIFICATION_TIME_KEY, time);
			}
		}

		//save/load remind flag
		public static bool loadRemindOnOffFlag()
		{
			if (Common.checkKey(CommonDefine.SETTINGS_NOTIFICATION_ONOFF_KEY) == true)
			{
				string strFlag = loadSettingValueByKey(CommonDefine.SETTINGS_NOTIFICATION_ONOFF_KEY);

				return bool.Parse(strFlag);
			}

			return bool.Parse(CommonDefine.DEFAULT_NOTIFICATION);
		}

		public static void saveRemindOnOffFlag(bool flag)
		{
			saveSettingValue(CommonDefine.SETTINGS_NOTIFICATION_ONOFF_KEY, flag.ToString());
		}

		//save/load current db version
		public static string loadCurrentDBVersion()
		{
			if (Common.checkKey(CommonDefine.DB_VERSION_KEY) == true)
			{
				string strVer = loadSettingValueByKey(CommonDefine.DB_VERSION_KEY);

				return strVer;
			}

			return "8";
		}

		public static void saveCurrentDBVersion(string ver)
		{
			if (ver != null)
			{
				saveSettingValue(CommonDefine.DB_VERSION_KEY, ver);
			}
		}

		//save/load backup-code
		public static string loadBackupCode()
		{
			if (Common.checkKey(CommonDefine.BACKUP_CODE_KEY) == true)
			{
				string strCode = loadSettingValueByKey(CommonDefine.BACKUP_CODE_KEY);

				return strCode;
			}

			return "";
		}

		public static void saveBackupCode(string code)
		{
			if (code != null)
			{
				saveSettingValue(CommonDefine.BACKUP_CODE_KEY, code);
			}
		}

		//save/load IS_FIRST_RUN_KEY
		public static bool loadIsFirstRunFlag()
		{
			if (Common.checkKey(CommonDefine.IS_FIRST_RUN_KEY) == true)
			{
				string strFlag = loadSettingValueByKey(CommonDefine.IS_FIRST_RUN_KEY);

				return bool.Parse(strFlag);
			}

			return true;
		}

		public static void saveIsFirstRunFlag(bool flag)
		{
			saveSettingValue(CommonDefine.IS_FIRST_RUN_KEY, flag.ToString());
		}

		//save/load SHOW_GUIDE_KEY
		public static bool loadShowGuideFlag()
		{
			if (Common.checkKey(CommonDefine.SHOW_GUIDE_KEY) == true)
			{
				string strFlag = loadSettingValueByKey(CommonDefine.SHOW_GUIDE_KEY);

				return bool.Parse(strFlag);
			}

			return true;
		}

		public static void saveShowGuideFlag(bool flag)
		{
			saveSettingValue(CommonDefine.SHOW_GUIDE_KEY, flag.ToString());
		}

		//save/load REVERSE_ENABLE_KEY
		public static bool loadReverseEnableFlag()
		{
			if (Common.checkKey(CommonDefine.REVERSE_ENABLE_KEY) == true)
			{
				string strFlag = loadSettingValueByKey(CommonDefine.REVERSE_ENABLE_KEY);

				return bool.Parse(strFlag);
			}

			return false;
		}

		public static void saveReverseEnableFlag(bool flag)
		{
			saveSettingValue(CommonDefine.REVERSE_ENABLE_KEY, flag.ToString());
		}

		//save/load COMPLETED_FLAG_KEY
		public static bool loadCompletedFlag()
		{
			if (Common.checkKey(CommonDefine.COMPLETED_FLAG_KEY) == true)
			{
				string strFlag = loadSettingValueByKey(CommonDefine.COMPLETED_FLAG_KEY);

				return bool.Parse(strFlag);
			}

			return false;
		}

		public static void saveCompletedFlag(bool flag)
		{
			saveSettingValue(CommonDefine.COMPLETED_FLAG_KEY, flag.ToString());
		}
	}
}
