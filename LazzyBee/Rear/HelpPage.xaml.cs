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
			htmlSource.Html = @"<!DOCTYPE html>
								<html>
								<body>
									<iframe src=""lazzybee_guide.htm"" width=""100%"" height=""100%"" frameborder=""0"" scrolling=""no"">
									</iframe>
								</body>
								</html>";
			webViewHelp.Source = htmlSource;

		}
	}
}
