using System;
using Xamarin.Forms;

namespace LazzyBee.Main
{
	public partial class LazzyBeePage : MasterDetailPage
	{
		public LazzyBeePage()
		{
			InitializeComponent();
			Detail = new NavigationPage(new HomePage()) {
				BarBackgroundColor = CommonDefine.MAIN_COLOR,
				BarTextColor = Color.White
			};

			masterPage.ListView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.Windows)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item != null)
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType))
                {
                    BarBackgroundColor = CommonDefine.MAIN_COLOR,
					BarTextColor = Color.White
				};
				masterPage.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}
	}
}
