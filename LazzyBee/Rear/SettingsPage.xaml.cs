using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			pickerLevel.SelectedIndex = int.Parse(lv);

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
			//TimeSpan timeSpand = new TimeSpan(
			//pickerRemindTime.Time

			//current db ver
			string dbVer = Common.loadCurrentDBVersion();
			tcUpdateDB.Detail = dbVer;

			//backup code
			string backupCode = Common.loadCurrentDBVersion();
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
		}

		void HandleTappedOnDailyTarget(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnDailyTarget");
			//throw new NotImplementedException();
		}

		void HandleTotalTargetSelectedIndexChanged(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTotalTargetSelectedIndexChanged");
			//throw new NotImplementedException(
		}

		void HandleTappedOnDailyMaxTotal(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnDailyMaxTotal");
			//throw new NotImplementedException();
		}

		void HandleLevelSelectedIndexChanged(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleLevelSelectedIndexChanged");
			//throw new NotImplementedExceptio
		}

		void HandleTappedOnLevel(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnLevel");
			//throw new NotImplementedException();
		}

		//Configuration
		void HandleSpeedSliderValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			Debug.WriteLine("HandleSpeedSliderValueChanged");
			//throw new NotImplementedException();
		}

		void HandleSWAutoPlayOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			Debug.WriteLine("HandleSWAutoPlayOnChanged");
			//throw new NotImplementedException();
		}

		void HandleSWDisplayMeaningOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			Debug.WriteLine("HandleSWDisplayMeaningOnChanged");
			//throw new NotImplementedException();
		}

		void HandleTappedOnWaitingTime(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnWaitingTime");
			//throw new NotImplementedException();
		}

		void HandleWaitingTimeSelectedIndexChanged(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleWaitingTimeSelectedIndexChanged");
			//throw new NotImplementedException(
		}

		//Reminder
		void HandleSWReminderOnOffOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			Debug.WriteLine("HandleSWReminderOnOffOnChanged");
			//throw new NotImplementedException();
		}

		void HandleReminderTimeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Debug.WriteLine("HandleReminderTimeChanged");
			//throw new NotImplementedException();

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
