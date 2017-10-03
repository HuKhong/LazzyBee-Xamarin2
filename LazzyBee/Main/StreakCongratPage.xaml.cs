using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class StreakCongratPage : ContentPage
	{
		public StreakCongratPage()
		{
			InitializeComponent();
			btnContinue.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnContinue.TextColor = Color.White;

			loadAndShowStreak();
		}

		void btnContinueClicked(object sender, System.EventArgs e)
		{
			Navigation.PopModalAsync(true);
		}

		void loadAndShowStreak()
		{
			int count = Common.countStreak();

			lbCongratStreak.Text = string.Format("{0} day(s)", count);
			lbCongrat.Text = string.Format("Congratulation!\nYou have got {0} day streak.", count);

			string strStreaks = Common.loadStreak();
			int streakDay = 0;

			if (strStreaks == null)
			{
				strStreaks = "";
			}

			string[] arrStreaks = strStreaks.Split(',');
			Array.Sort(arrStreaks);
			int dayInInterval = DateTimeHelper.getBeginOfDayInSec();
			int offset = 0;
			bool status = false;
			string imgPath = "";

			for (int i = 0; i < 7; i++)
			{
				status = false;
				for (int j = 0; j < 7; j++)
				{
					if (j < arrStreaks.Count())
					{
						int.TryParse(arrStreaks[j], out streakDay);
						if (dayInInterval >= streakDay)
						{
							offset = dayInInterval - streakDay;
						}
						else
						{
							offset = streakDay - dayInInterval;
						}

						if (offset < CommonDefine.SECONDS_OF_HALFDAY)
						{
							status = true;
							break;
						}
					}
					else
					{
						break;
					}
				}

				if (status == true)
				{
					imgPath = "images/others/day_ring.png";
				}
				else
				{
					imgPath = "images/others/day_ring_gray.png";
				}

				//day 7 (today)
				if (i == 0)
				{
					lbDay7.Text = DateTimeHelper.getDayOfWeek(dayInInterval);
					imgDay7.Source = imgPath;
				} 
				else if (i == 1)
				{
					lbDay6.Text = DateTimeHelper.getDayOfWeek(dayInInterval);
					imgDay6.Source = imgPath;
				}
				else if (i == 2)
				{
					lbDay5.Text = DateTimeHelper.getDayOfWeek(dayInInterval);
					imgDay5.Source = imgPath;
				}
				else if (i == 3)
				{
					lbDay4.Text = DateTimeHelper.getDayOfWeek(dayInInterval);
					imgDay4.Source = imgPath;
				}
				else if (i == 4)
				{
					lbDay3.Text = DateTimeHelper.getDayOfWeek(dayInInterval);
					imgDay3.Source = imgPath;
				}
				else if (i == 5)
				{
					lbDay2.Text = DateTimeHelper.getDayOfWeek(dayInInterval);
					imgDay2.Source = imgPath;
				}
				else if (i == 6)
				{
					lbDay1.Text = DateTimeHelper.getDayOfWeek(dayInInterval);
					imgDay1.Source = imgPath;
				}

				dayInInterval = dayInInterval - CommonDefine.SECONDS_OF_DAY;
			}
		}
	}
}
