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

			var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Home",
				IconSource = "images/icons/ic_holder.png",
				TargetType = typeof(HomePage)
			});

			listView.ItemsSource = masterPageItems;
		}
    }
}
