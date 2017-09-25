using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using System.Net;
using System.Threading;
using System.Collections.ObjectModel;

namespace LazzyBee
{
	public partial class IncomingPage : ContentPage
	{
		private List<WordInfo> wordsList = new List<WordInfo>();
		public static ObservableCollection<IncomingListItem> incomingListItems { get; set; }

		public IncomingPage()
		{
			incomingListItems = new ObservableCollection<IncomingListItem>();
			InitializeComponent();

			double width = App.Current.MainPage.Width;
			double height = App.Current.MainPage.Height;

			loadingIndicator.TranslationX = width / 2 - 10;
			loadingIndicator.TranslationY = height / 2 - 50;

			incomingListView.ItemSelected += OnItemSelected;

			loadingIndicator.IsRunning = true;
			ThreadStart threadStart = new ThreadStart(loadIncomingList);
			Thread myThread = new Thread(threadStart);
			myThread.Start();
		}

		void loadIncomingList()
		{

			wordsList.AddRange(SqliteHelper.Instance.getIncomingList());

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

				incomingListItems.Add(incomingItem);
			}

			Device.BeginInvokeOnMainThread(() =>
			{
				incomingListView.ItemsSource = incomingListItems;

				loadingIndicator.IsRunning = false;
			});
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as IncomingListItem;

			if (item != null)
			{
				DictionaryTabPage dictTabPage = new DictionaryTabPage();
				dictTabPage.wordInfo = item.word;
				dictTabPage.showLazzyBeeTab = true;
				Navigation.PushAsync(dictTabPage);
			}

			incomingListView.SelectedItem = null;
		}

		public void OnIgnore(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			//DisplayAlert("Ignore Context Action", mi.CommandParameter + " Ignore context action", "OK");
			IncomingListItem incomingItem = (IncomingListItem)mi.CommandParameter;
			WordInfo w = incomingItem.word;

			w.queue = WordInfo.QUEUE_SUSPENDED.ToString();

			SqliteHelper.Instance.updateWord(w);

			incomingListItems.Remove(incomingItem);

			wordsList.Remove(incomingItem.word);
			SqliteHelper.Instance.updateBufferWordList(wordsList);
		}

		public void OnDone(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			//DisplayAlert("Done Context Action", mi.CommandParameter + " Done context action", "OK");
			IncomingListItem incomingItem = (IncomingListItem)mi.CommandParameter;
			WordInfo w = incomingItem.word;

			w.queue = WordInfo.QUEUE_DONE.ToString();

			SqliteHelper.Instance.updateWord(w);

			incomingListItems.Remove(incomingItem);

			wordsList.Remove(incomingItem.word);
			SqliteHelper.Instance.updateBufferWordList(wordsList);

		}
	}
}
