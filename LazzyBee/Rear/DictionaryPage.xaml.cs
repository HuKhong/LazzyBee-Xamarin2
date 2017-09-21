using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading;
using System.Linq;

namespace LazzyBee
{
	public partial class DictionaryPage : ContentPage
	{
		private List<WordInfo> words;
		public DictionaryPage()
		{
			InitializeComponent();

			double width = App.Current.MainPage.Width;
			double height = App.Current.MainPage.Height;

			loadingIndicator.TranslationX = width / 2 - 10;
			loadingIndicator.TranslationY = height / 2 - 50;

			dictionaryListView.ItemSelected += OnItemSelected;
			loadingIndicator.IsRunning = true;
			ThreadStart threadStart = new ThreadStart(loadingDataFromDB);
			Thread myThread = new Thread(threadStart);
			myThread.Start();
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as WordInfo;

			if (item != null)
			{
				DictionaryTabPage dictTabPage = new DictionaryTabPage();
				dictTabPage.wordInfo = item;
				dictTabPage.showLazzyBeeTab = true;
				Navigation.PushAsync(dictTabPage);
			}

			dictionaryListView.SelectedItem = null;
		}

		void loadingDataFromDB()
		{
			words = SqliteHelper.Instance.getAllWords();
			Device.BeginInvokeOnMainThread(() =>
			{
				dictionaryListView.ItemsSource = words;

				loadingIndicator.IsRunning = false;
			});
		}

		void searchBtnPressedHandle(object sender, System.EventArgs e)
		{
			SearchResultPage resultPage = new SearchResultPage();

			resultPage.wordsList = words.Where(i => i.question.Contains(searchBox.Text.ToLower())).ToList();;

			Navigation.PushAsync(resultPage);
		}

		void searchbarTextChangedHandle(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			dictionaryListView.BeginRefresh();

			if (string.IsNullOrWhiteSpace(e.NewTextValue))
			{
				dictionaryListView.ItemsSource = words;
			}
			else
			{
				dictionaryListView.ItemsSource = words.Where(i => i.question.Contains(e.NewTextValue.ToLower()));
			}

			dictionaryListView.EndRefresh();
		}
	}
}
