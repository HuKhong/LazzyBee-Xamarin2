using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net;

namespace LazzyBee.Main
{
	public partial class HomePage : ContentPage
	{
		public bool showHint { get; set; }
		public List<WordInfo> words;
		public static ObservableCollection<IncomingListItem> resultItems { get; set; }

		private SqliteHelper sqlLiteHelper = SqliteHelper.Instance;

		public HomePage()
		{
			InitializeComponent();
			showHint = false;
			resultListView.IsVisible = showHint;
			resultItems = new ObservableCollection<IncomingListItem>();

			//Button btnStartLearning = this.FindByName<Button>("btnStartLearning");
			btnStartLearning.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnStartLearning.TextColor = Color.White;

			//Button btnIncoming = this.FindByName<Button>("btnIncomingList");
			btnIncomingList.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnIncomingList.TextColor = Color.White;

			//Button btnMoreWords = this.FindByName<Button>("btnMoreWords");
			btnMoreWords.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnMoreWords.TextColor = Color.White;

			ThreadStart threadStart = new ThreadStart(prepareWordsToStudyingQueue);
			Thread myThread = new Thread(threadStart);
			myThread.Start();
		}

		void loadingDataFromDB()
		{
			words = SqliteHelper.Instance.getAllWords();

			//foreach (WordInfo w in temps)
			//{
			//	words.Add(w);
			//}

			//loadResultList(words);

			//Device.BeginInvokeOnMainThread(() =>
			//{
			//	resultListView.ItemsSource = resultItems;

			//});
		}

		void loadResultList(List<WordInfo> wordsList)
		{
			if (wordsList == null)
			{
				return;
			}

			string strMeaning = "";
			string package = "";
			string packageLowcase = CommonDefine.DEFAULT_SUBJECT;
			string strPronounciation = "";

			MajorObject major = Common.loadMajorFromProperties();
			if (major != null)
			{
				package = major.displayName;
				packageLowcase = major.majorName.ToLower();
			}
			else
			{
				packageLowcase = CommonDefine.DEFAULT_SUBJECT;
			}

			resultItems.Clear();

			foreach (WordInfo word in wordsList)
			{
				IncomingListItem incomingItem = new IncomingListItem();

				//parse the answer to dictionary object
				JObject dictAnswer = JObject.Parse(word.answers);

				//A word may has many meanings corresponding to many fields (common, it, economic...)
				//The meaning of each field is considered as a package
				var dictPackages = dictAnswer["packages"];
				var dictSinglePackage = dictPackages[packageLowcase];

				if (dictSinglePackage == null)
				{
					dictSinglePackage = dictPackages[CommonDefine.DEFAULT_SUBJECT];
					package = "";
				}

				strPronounciation = (string)dictAnswer["pronoun"];

				if (strPronounciation.Equals("//") == true)
				{
					strPronounciation = "";
				}

				//meaning
				if (dictSinglePackage != null)
				{
					strMeaning = (string)dictSinglePackage["meaning"];
				}

				//remove <p>, keep <br>
				if (strMeaning != null)
				{
					strMeaning = strMeaning.Replace("<p>", String.Empty);
					strMeaning = strMeaning.Replace("</p>", String.Empty);

				}
				else
				{
					strMeaning = "";
				}

				incomingItem.Word = word.question;
				incomingItem.Pronounce = strPronounciation;
				incomingItem.Meaning = WebUtility.HtmlDecode(strMeaning);
				incomingItem.Level = string.Format("Level: {0}", word.level);
				incomingItem.word = word;

				resultItems.Add(incomingItem);

				if (resultItems.Count >= 20)
				{
					break;
				}
			}
		}

		void btnStartLearningClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnStartLearningClicked");

			//check and pick new words
			int countBuffer = sqlLiteHelper.getCountOfBuffer();
			int dailyTarget = Common.loadDailyTarget();

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
				var answer = await DisplayAlert("Attention",
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
				int dailyTarget = Common.loadDailyTarget();

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

			ThreadStart bgThreadStart = new ThreadStart(loadingDataFromDB);
			Thread bgThread = new Thread(bgThreadStart);
			bgThread.Start();
		}

		void searchBtnPressedHandle(object sender, System.EventArgs e)
		{
			SearchResultPage resultPage = new SearchResultPage();

			resultPage.wordsList = words.Where(i => i.question.Contains(searchBox.Text.ToLower())).ToList(); ;

			Navigation.PushAsync(resultPage);
		}

		void searchbarTextChangedHandle(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			while (words == null || words.Count < 1) 
			{
				return;
			}

			resultListView.BeginRefresh();

			if (string.IsNullOrWhiteSpace(e.NewTextValue))
			{
				showHint = false;
				resultListView.IsVisible = showHint;

                //loadResultList(words);
				//resultListView.ItemsSource = resultItems;
			}
			else
			{
				showHint = true;
				resultListView.IsVisible = showHint;
				List<WordInfo> temp = words.Where(i => i.question.StartsWith(e.NewTextValue.ToLower())).ToList();

				loadResultList(temp);
				resultListView.ItemsSource = resultItems;
			}

			resultListView.EndRefresh();
		}

		public void OnLearn(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);

			IncomingListItem incomingItem = (IncomingListItem)mi.CommandParameter;
			WordInfo w = incomingItem.word;

			w.queue = WordInfo.QUEUE_AGAIN.ToString();
			Algorithm.getInstance().updateWordProgressWithEaseOption(ref w, Algorithm.OPTION_AGAIN);

			SqliteHelper.Instance.updateWord(w);

			XFToast.ShortMessage("Added to learn");

			//need to remove this word from buffer and pickedwo
		}
	}
}
