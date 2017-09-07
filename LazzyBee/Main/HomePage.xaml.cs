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

			//Button btnStartLearning = this.FindByName<Button>("btnStartLearning");
			btnStartLearning.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnStartLearning.TextColor = Color.White;

			//Button btnIncoming = this.FindByName<Button>("btnIncomingList");
			btnIncomingList.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnIncomingList.TextColor = Color.White;

			//Button btnMoreWords = this.FindByName<Button>("btnMoreWords");
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

		async void btnMoreWordsClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnMoreWordsClicked");
			int count = SqliteHelper.Instance.getCountOfInreview();
			count = count + SqliteHelper.Instance.getCountOfPickedWord();
			count = count + SqliteHelper.Instance.getCountOfStudyAgain();

			if (count > 0)
			{
               var answer = await DisplayAlert("Notice", 
				                               "You still have a few words need to learn. Complete them before adding more words.", 
				                               "Learn", "Cancel");
				if (answer == true)
				{
					StudyPage stdPage = new StudyPage();
					await Navigation.PushAsync(stdPage);
				}

			}
			else
			{
				//check and pick new words
				int countBuffer = sqlLiteHelper.getCountOfBuffer();
				int dailyTarget = Common.getDailyTarget();

				if (countBuffer < dailyTarget)
				{
					prepareWordsToStudyingQueue();
				}

				sqlLiteHelper.pickUpRandom10WordsToStudyingQueue(dailyTarget, true);

				StudyPage stdPage = new StudyPage();
				await Navigation.PushAsync(stdPage);

				//show ad full screen
				//	if (self.interstitial.isReady)
				//	{

				//[self.interstitial presentFromRootViewController:self];
				//}
			}
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
