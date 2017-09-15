using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LazzyBee
{
	public partial class MajorsListPage : ContentPage
	{
		private List<MajorObject> majorObjects = new List<MajorObject>();
		public MajorsListPage()
		{
			InitializeComponent();
			MajorObject it = new MajorObject();
			it.majorName = "it";
			it.majorThumbnail = "images/majors/it.png";

			majorObjects.Add(it);

			majorsListView.ItemsSource = majorObjects;
		}
	}
}
