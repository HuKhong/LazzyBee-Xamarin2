using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace LazzyBee.Main
{
    public partial class HomePage : ContentPage
    {
		private SqliteHelper sqlLiteHelper = SqliteHelper.Instance;

        public HomePage()
        {
			InitializeComponent();

			Button btnStartLearning = this.FindByName<Button>("btnStartLearning");
			btnStartLearning.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnStartLearning.TextColor = Color.White;

			Button btnIncoming = this.FindByName<Button>("btnIncomingList");
			btnIncoming.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnIncoming.TextColor = Color.White;

			Button btnMoreWords = this.FindByName<Button>("btnMoreWords");
			btnMoreWords.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnMoreWords.TextColor = Color.White;

			prepareWordsToStudyingQueue();
        }

		void btnStartLearningClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnStartLearningClicked");

			//check and pick new words
			int countBuffer = sqlLiteHelper.getCountOfBuffer();
			int dailyTarget = Common.getDailyTarget();

			if (countBuffer < dailyTarget)
			{
				prepareWordsToStudyingQueue();
			}

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
//List<WordDAO> words = sqliteHelper.getAllWords();

//Debug.WriteLine("test");
//			DisplayAlert("Alert", words.Count.ToString(), "OK");
		}

		private void prepareWordsToStudyingQueue()
		{
			//MajorObject curMajorObj = (MajorObject*)[[Common sharedCommon] loadPersonalDataWithKey: KEY_SELECTED_MAJOR];

			//NSString* curMajor = curMajorObj.majorName;

			//if (curMajor == nil || curMajor.length == 0)
			//{
			//	curMajor = @"common";
			//}
			//else
			//{
			//	curMajor = [curMajor lowercaseString];
			//}
			MajorObject curMajorObj = new MajorObject();
			string major = curMajorObj.majorName;

			sqlLiteHelper.prepareWordsToStudyingQueue(CommonDefine.BUFFER_SIZE, major);
		}
	}
}
