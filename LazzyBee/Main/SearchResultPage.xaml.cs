using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class SearchResultPage : ContentPage
	{
		public List<WordInfo> wordsList;
		public static ObservableCollection<IncomingListItem> resultItems { get; set; }

		public SearchResultPage()
		{
			resultItems = new ObservableCollection<IncomingListItem>();
			InitializeComponent();

			double width = App.Current.MainPage.Width;
			double height = App.Current.MainPage.Height;

			loadingIndicator.TranslationX = width / 2 - 10;
			loadingIndicator.TranslationY = height / 2 - 50;

			resultListView.ItemSelected += OnItemSelected;

			loadingIndicator.IsRunning = true;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ThreadStart threadStart = new ThreadStart(loadResultList);
			Thread myThread = new Thread(threadStart);
			myThread.Start();
		}

		void loadResultList()
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
				strMeaning = (string)dictSinglePackage["meaning"];

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
			}

			Device.BeginInvokeOnMainThread(() =>
			{
				resultListView.ItemsSource = resultItems;

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

			resultListView.SelectedItem = null;
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

			//need to remove this word from buffer and pickedword
		}
	}
}
