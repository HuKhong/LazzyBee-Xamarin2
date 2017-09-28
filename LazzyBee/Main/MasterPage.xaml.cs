using System.Collections.Generic;

using Xamarin.Forms;

namespace LazzyBee
{
    public partial class MasterPage : ContentPage
    {
		public ListView ListView { get { return listView; } }

		public MasterPage()
		{
			InitializeComponent();
			var mainFunctionItems = new List<MasterPageItem>();

			//main function items
			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Home",
				IconSource = "images/icons/ic_home.png",
				TargetType = typeof(HomePage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Majors List",
				IconSource = "images/icons/ic_list.png",
				TargetType = typeof(MajorsListPage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Dictionary",
				IconSource = "images/icons/ic_dictionary.png",
				TargetType = typeof(DictionaryPage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Statistics",
				IconSource = "images/icons/ic_graph.png",
				TargetType = typeof(StatisticsPage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Vocabulary Test",
				IconSource = "images/icons/ic_extension.png",
				TargetType = typeof(VocabularyTestPage)
			});


			//settings + helps
			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Settings",
				IconSource = "images/icons/ic_setting.png",
				TargetType = typeof(SettingsPage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Help",
				IconSource = "images/icons/ic_help.png",
				TargetType = typeof(HelpPage)
			});

			//Share
			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Share",
				IconSource = "images/icons/ic_share.png",
				TargetType = null
			});

			listView.ItemsSource = mainFunctionItems;
		}
    }
}
