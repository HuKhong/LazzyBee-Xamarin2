using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class VocabularyTestPage : ContentPage
	{
		public VocabularyTestPage()
		{
			InitializeComponent();

			if (CrossConnectivity.Current.IsConnected)
			{
				webViewVocaTest.Source = "http://www.lazzybee.com/testvocab?menu=0";
			}
			else
			{
				//no Internet available
				DisplayAlert("No connection", "Please double check wifi/3G connection", "OK");
			}
		}
	}
}
