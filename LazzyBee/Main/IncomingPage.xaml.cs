using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using System.Net;

namespace LazzyBee
{
	public partial class IncomingPage : ContentPage
	{
		private List<WordInfo> wordsList = new List<WordInfo>();
		private List<IncomingListItem> incomingListItems = new List<IncomingListItem>();
		public IncomingPage()
		{
			InitializeComponent();

			incomingListView.ItemSelected += OnItemSelected;

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

				incomingListItems.Add(incomingItem);
			}

			incomingListView.ItemsSource = incomingListItems;
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
	}
}
