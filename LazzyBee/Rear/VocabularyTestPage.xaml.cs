using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LazzyBee
{
	public partial class VocabularyTestPage : ContentPage
	{
		public VocabularyTestPage()
		{
			InitializeComponent();
			webViewVocaTest.Source = "http://www.lazzybee.com/testvocab?menu=0";
		}
	}
}
