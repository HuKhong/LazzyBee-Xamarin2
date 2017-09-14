using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class HelpPage : ContentPage
	{
		public HelpPage()
		{
			InitializeComponent();
			var htmlSource = new HtmlWebViewSource();
			htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

			var filePath = Path.Combine(documentsPath, "lazzybee_guide.htm");
			webViewHelp.Source = htmlSource;

		}
	}
}
