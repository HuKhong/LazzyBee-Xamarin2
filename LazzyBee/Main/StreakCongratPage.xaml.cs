using System;
using System.Collections.Generic;

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
		}

		void btnContinueClicked(object sender, System.EventArgs e)
		{
			Navigation.PopModalAsync(true);
		}
	}
}
