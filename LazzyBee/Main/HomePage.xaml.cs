using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LazzyBee.Main
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
			InitializeComponent();

			Button btnStartLearning;
			btnStartLearning = this.FindByName<Button>("btnStartLearning");
			btnStartLearning.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnStartLearning.TextColor = Color.White;

			Button btnIncoming;
			btnIncoming = this.FindByName<Button>("btnIncomingList");
			btnIncoming.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnIncoming.TextColor = Color.White;

			Button btnMoreWords;
			btnMoreWords = this.FindByName<Button>("btnMoreWords");
			btnMoreWords.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnMoreWords.TextColor = Color.White;
        }

		void btnStartLearningClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnStartLearningClicked");
			StudyPage stdPage = new StudyPage();
			Navigation.PushAsync(stdPage);
		}

		void btnIncomingListClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnIncomingListClicked");
			IncomingPage incomingPage = new IncomingPage();
			Navigation.PushAsync(incomingPage);
		}

		void btnMoreWordsClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnMoreWordsClicked");
		}
    }
}
