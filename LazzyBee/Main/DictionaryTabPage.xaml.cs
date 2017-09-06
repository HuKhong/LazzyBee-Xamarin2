using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class DictionaryTabPage : TabbedPage
	{
		private WordInfo _wordInfo;
		public WordInfo wordInfo
		{
			get
			{
				return _wordInfo;
			}
			set
			{
                this._wordInfo = value;
				this.Title = _wordInfo.question;

				//transfer to web view
				DictionaryWebView tabEN =this.FindByName<DictionaryWebView>("TabEN");
				tabEN.wordInfo = _wordInfo;
				tabEN.dictType = HTMLHelper.LANG_EN;

				DictionaryWebView tabVI = this.FindByName<DictionaryWebView>("TabVI");
				tabVI.wordInfo = _wordInfo;
				tabVI.dictType = HTMLHelper.LANG_VI;

				DictionaryWebView tabLazzyBee = this.FindByName<DictionaryWebView>("TabLazzyBee");
				tabLazzyBee.wordInfo = _wordInfo;
				tabLazzyBee.dictType = "LazzyBee";
			}
		}

		private bool _showLazzyBeeTab = true;
		public bool showLazzyBeeTab
		{
			get
			{
				return _showLazzyBeeTab;
			}
			set
			{
                this._showLazzyBeeTab = value;
				DictionaryWebView tabLazzyBee = this.FindByName<DictionaryWebView>("TabLazzyBee");

				if (_showLazzyBeeTab == false)
				{
					this.Children.Remove(tabLazzyBee);
				}
			}
		}

		public DictionaryTabPage()
		{
			InitializeComponent();
			Debug.WriteLine("DictionaryTabPage");
		}
	}
}
