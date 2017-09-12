using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();

			//daily target
			int dailyTargetNum = Common.loadDailyTarget();
			if (dailyTargetNum == 1)
			{
				tcDailyNewWords.Detail = string.Format("{0} word per day", dailyTargetNum);
			}
			else
			{
				tcDailyNewWords.Detail = string.Format("{0} words per day", dailyTargetNum);
			}

			//total target
			int dailyTotalNum = Common.loadTotalTarget();
			tcDailyTotal.Detail = string.Format("{0} words per day", dailyTotalNum);

			//Level
			string lv = Common.loadSelectedLevel();
			tcLevel.Text = string.Format("Level: {0}", lv);

			//speed
			double speed = Common.loadSpeakingSpeed();
			slSpeed.Value = speed;

			//autoplay
			bool auto = Common.loadAutoplayFlag();
			swAutoplay.On = auto;

			//display meaning
			bool display = Common.loadDisplayMeaningFlag();
			swDisplayMeaning.On = display;

			//remind on/off
			bool remind = Common.loadRemindOnOffFlag();
			swReminder.On = remind;

			//remind time
			string time = Common.loadRemindTime();
			tcTimeToReminder.Detail = time;

			//current db ver
			string dbVer = Common.loadCurrentDBVersion();
			tcUpdateDB.Detail = dbVer;

			//backup code
			string backupCode = Common.loadCurrentDBVersion();
			tcBackupDB.Detail = backupCode;
		}

		//Target
		void HandleTappedOnDailyTarget(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnDailyTarget");
			//throw new NotImplementedException();
		}

		void HandleTappedOnDailyMaxTotal(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnDailyMaxTotal");
			//throw new NotImplementedException();
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

		//Reminder
		void HandleSWReminderOnOffOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			Debug.WriteLine("HandleSWReminderOnOffOnChanged");
			//throw new NotImplementedException();
		}

		void HandleTappedOnReminderTime(object sender, System.EventArgs e)
		{
			Debug.WriteLine("HandleTappedOnReminderTime");
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
