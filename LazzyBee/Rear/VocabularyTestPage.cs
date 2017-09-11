using System;

using Xamarin.Forms;

namespace LazzyBee
{
	public class VocabularyTestPage : ContentPage
	{
		public VocabularyTestPage()
		{
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}

