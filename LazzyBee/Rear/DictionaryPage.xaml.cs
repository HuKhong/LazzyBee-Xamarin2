using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading;

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
	}
}
