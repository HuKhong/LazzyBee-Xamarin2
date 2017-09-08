using System.Collections.Generic;

using Xamarin.Forms;

namespace LazzyBee.Main
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
				Title = "Majors list",
				IconSource = "images/icons/ic_list.png",
				TargetType = typeof(HomePage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Dictionary",
				IconSource = "images/icons/ic_dictionary.png",
				TargetType = typeof(HomePage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Statistics",
				IconSource = "images/icons/ic_graph.png",
				TargetType = typeof(HomePage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Vocabulary test",
				IconSource = "images/icons/ic_extension.png",
				TargetType = typeof(HomePage)
			});


			//settings + helps
			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Settings",
				IconSource = "images/icons/ic_setting.png",
				TargetType = typeof(HomePage)
			});

			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Help",
				IconSource = "images/icons/ic_help.png",
				TargetType = typeof(HomePage)
			});

			//Share
			mainFunctionItems.Add(new MasterPageItem
			{
				Title = "Share",
				IconSource = "images/icons/ic_share.png",
				TargetType = typeof(HomePage)
			});

			listView.ItemsSource = mainFunctionItems;
		}
    }
}
