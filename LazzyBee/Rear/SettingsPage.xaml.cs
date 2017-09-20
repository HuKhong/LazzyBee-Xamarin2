using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class SettingsPage : ContentPage
	{
		private const int TOTAL_OPTION_VERY_EASY = 20;
		private const int TOTAL_OPTION_EASY = 30;
		private const int TOTAL_OPTION_NORMAL = 40;
		private const int TOTAL_OPTION_HARD = 60;
		private const int TOTAL_OPTION_IMPOSSIBLE = 80;

		public SettingsPage()
		{
			InitializeComponent();

			loadSettingsContent();
		}

		void loadSettingsContent()
		{
			//daily target
			int dailyTargetNum = Common.loadDailyTarget();
			pickerNewWordTarget.SelectedIndex = dailyTargetNum / 5;

			//total target
			int dailyTotalNum = Common.loadTotalTarget();
			pickerTotalTarget.SelectedIndex = whatRowInTotalTargetTable(dailyTotalNum);

			//Level
			string lv = Common.loadSelectedLevel();
			if (lv == null)
			{
				lv = CommonDefine.DEFAULT_LEVEL;
			}
			int intLv = int.Parse(lv);
			if (intLv < 1)
			{
				intLv = int.Parse(CommonDefine.DEFAULT_LEVEL);
			}
			pickerLevel.SelectedIndex = intLv - 1; //count from 0

			//speed
			double speed = Common.loadSpeakingSpeed();
			slSpeed.Value = speed;

			//autoplay
			bool auto = Common.loadAutoplayFlag();
			swAutoplay.On = auto;

			//display meaning
			bool display = Common.loadDisplayMeaningFlag();
			swDisplayMeaning.On = display;

			//waiting time
			int waitingTime = Common.loadWaitingTimeToShowAnswer();
			pickerWaitingTime.SelectedIndex = waitingTime;

			//remind on/off
			bool remind = Common.loadRemindOnOffFlag();
			swReminder.On = remind;

			//remind time
			string time = Common.loadRemindTime();
			if (time != null)
			{
				DateTime dt;
				if (!DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture,
															  DateTimeStyles.None, out dt))
				{
					// handle validation error
				}
				pickerRemindTime.Time = dt.TimeOfDay;
			}

			//current db ver
			string dbVer = Common.loadCurrentDBVersion();
			tcUpdateDB.Detail = string.Format("Current version: {0}", dbVer);

			//backup code
			string backupCode = Common.loadBackupCode();
			tcBackupDB.Detail = backupCode;
		}

		private int whatRowInTotalTargetTable(int value)
		{
			int row = 0;

			if (value == TOTAL_OPTION_VERY_EASY)
			{
				row = 0;

			}
			else if (value == TOTAL_OPTION_EASY)
			{
				row = 1;

			}
			else if (value == TOTAL_OPTION_NORMAL)
			{
				row = 2;
			}
			else if (value == TOTAL_OPTION_HARD)
			{
				row = 3;

			}
			else if (value == TOTAL_OPTION_IMPOSSIBLE)
			{
				row = 4;
			}

			return row;
		}

		//Target
		void HandleDailyTargetSelectedIndexChanged(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleDailyTargetSelectedIndexChanged");
			//throw new NotImplementedException();
			string target = pickerNewWordTarget.SelectedItem.ToString();
			Common.saveDailyTarget(int.Parse(target.Substring(0, 2)));
			TableView tableSettings = new TableView();
		}

		void HandleTappedOnDailyTarget(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnDailyTarget");
			//throw new NotImplementedException();
			pickerNewWordTarget.Focus();
		}

		//total target
		void HandleTotalTargetSelectedIndexChanged(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTotalTargetSelectedIndexChanged");
			//throw new NotImplementedException()
			string target = pickerTotalTarget.SelectedItem.ToString();
			Common.saveTotalTarget(int.Parse(target.Substring(0, 2)));	//remove "words"

		}

		void HandleTappedOnDailyMaxTotal(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnDailyMaxTotal");
			//throw new NotImplementedException();
			pickerTotalTarget.Focus();
		}

		//level
		void HandleLevelSelectedIndexChanged(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleLevelSelectedIndexChanged");
			//throw new NotImplementedExceptio
			string lv = pickerLevel.SelectedItem.ToString();
			Common.saveSelectedLevel(lv);
		}

		void HandleTappedOnLevel(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnLevel");
			//throw new NotImplementedException();
			pickerLevel.Focus();
		}

		//Configuration
		//speed
		void HandleSpeedSliderValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			Debug.WriteLine("HandleSpeedSliderValueChanged");
			//throw new NotImplementedException();
			double speed = slSpeed.Value;
			Common.saveSpeakingSpeed(speed);
		}

		//autoplay
		void HandleSWAutoPlayOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			Debug.WriteLine("HandleSWAutoPlayOnChanged");
			//throw new NotImplementedException();
			bool flag = swAutoplay.On;
			Common.saveAutoplayFlag(flag);
		}

		//display meaning
		void HandleSWDisplayMeaningOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			Debug.WriteLine("HandleSWDisplayMeaningOnChanged");
			//throw new NotImplementedException();
			bool flag = swDisplayMeaning.On;
			Common.saveDisplayMeaningFlag(flag);
		}

		//waiting time
		void HandleTappedOnWaitingTime(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnWaitingTime");
			//throw new NotImplementedException();
			pickerWaitingTime.Focus();
		}

		void HandleWaitingTimeSelectedIndexChanged(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleWaitingTimeSelectedIndexChanged");
			//throw new NotImplementedException(
			string sec = pickerWaitingTime.SelectedItem.ToString();
			Common.saveWaitingTimeToShowAnswer(int.Parse(sec.Substring(0, 1)));
		}

		//Reminder
		void HandleSWReminderOnOffOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			Debug.WriteLine("HandleSWReminderOnOffOnChanged");
			//throw new NotImplementedException();
			bool flag = swReminder.On;
			Common.saveRemindOnOffFlag(flag);
		}

		void HandleReminderTimeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Debug.WriteLine("HandleReminderTimeChanged");
			//throw new NotImplementedException();
			if (e.PropertyName.Equals("Time")) {
				//save
				string time = pickerRemindTime.Time.ToString();
			}

		}

		void HandleTappedOnRemindTime(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnWaitingTime");
			//throw new NotImplementedException();
			pickerRemindTime.Focus();
		}

		//Database
		void HandleTappedOnUpdateDatabase(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnUpdateDatabase");
			//throw new NotImplementedException();
		}

		void HandleTappedOnBackupDatabase(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnBackupDatabase");
			//throw new NotImplementedException();
		}

		void HandleTappedOnRestoreDatabase(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnRestoreDatabase");
			//throw new NotImplementedException();
		}
	}
}
