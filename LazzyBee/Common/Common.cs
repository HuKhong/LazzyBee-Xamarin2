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
	}
}
