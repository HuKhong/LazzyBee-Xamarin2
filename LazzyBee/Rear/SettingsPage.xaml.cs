using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LazzyBee
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();

			//daily target
			int dailyTargetNum = Common.getDailyTarget();
			if (dailyTargetNum == 1)
			{
				tcDailyNewWords.Detail = string.Format("{0} word per day", dailyTargetNum);
			}
			else
			{
				tcDailyNewWords.Detail = string.Format("{0} words per day", dailyTargetNum);
			}

			//total target
			int dailyTotalNum = Common.getTotalTarget();
			tcDailyTotal.Detail = string.Format("{0} words per day", dailyTotalNum);

			//Level
		}

		//Target
		void HandleTappedOnDailyTarget(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}

		void HandleTappedOnDailyMaxTotal(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}

		void HandleTappedOnLevel(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}

		//Configuration
		void HandleSpeedSliderValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		void HandleSWAutoPlayOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			throw new NotImplementedException();
		}

		void HandleSWDisplayMeaningOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			throw new NotImplementedException();
		}

		void HandleTappedOnWaitingTime(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}

		//Reminder
		void HandleSWReminderOnOffOnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			throw new NotImplementedException();
		}

		void HandleTappedOnReminderTime(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}

		//Database
		void HandleTappedOnUpdateDatabase(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}

		void HandleTappedOnBackupDatabase(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}

		void HandleTappedOnRestoreDatabase(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
