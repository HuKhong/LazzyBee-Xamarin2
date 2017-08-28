using System;
using Xamarin.Forms;

namespace LazzyBee
{
	public class Common
	{
		public Common()
		{
		}

		/* save/load/remove settings */
		public static void saveSettingValue(string key, string value) {
			if (key != null && value != null &&
			    key.Length > 0 && value.Length > 0)
			{
				Application.Current.Properties[key] = value;
			}
		}

		public static string loadSettingValueByKey (string key) {
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

		public static int getDailyTarget()
		{
			int res = 0;

			if (Common.checkKey(CommonDefine.SETTINGS_NEW_CARD_KEY) == false)
			{
				res = int.Parse(loadSettingValueByKey(CommonDefine.SETTINGS_NEW_CARD_KEY));
			}

			return res;
		}

		public static int getTotalTarget()
		{
			int res = 0;

			if (Common.checkKey(CommonDefine.SETTINGS_TOTAL_CARD_KEY) == false)
			{
				res = int.Parse(loadSettingValueByKey(CommonDefine.SETTINGS_TOTAL_CARD_KEY));
			}

			return res;
		}
	}
}
