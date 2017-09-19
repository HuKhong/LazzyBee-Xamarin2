using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LazzyBee
{
	public partial class DictionaryPage : ContentPage
	{
		private List<WordInfo> words;
		public DictionaryPage()
		{
			InitializeComponent();

			dictionaryListView.ItemSelected += OnItemSelected;

			words = SqliteHelper.Instance.getAllWords();

			dictionaryListView.ItemsSource = words;
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
	}
}
