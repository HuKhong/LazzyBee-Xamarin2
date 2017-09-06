using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class DictionaryWebView : ContentPage
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

				if (_dictType.Equals("") != true)
				{
					WebView webView = this.FindByName<WebView>("webView");
					var htmlSource = new HtmlWebViewSource();

					string html = "";

					if (_dictType.Equals(HTMLHelper.LANG_VI) ||
					   _dictType.Equals(HTMLHelper.LANG_EN))
					{
						html = HTMLHelper.createHTMLDict(_wordInfo, _dictType);
					}
					else
					{
						MajorObject major = new MajorObject();
						html = HTMLHelper.createHTMLForQuestion(_wordInfo, major);
					}

					htmlSource.Html = html;
					webView.Source = htmlSource;
				}
			}
		}

		private string _dictType = "";   //EN - VI - LazzyBee
		public string dictType
		{
			get
			{
				return _dictType;
			}
			set
			{
				this._dictType = value;

				if (_wordInfo != null)
				{
					WebView webView = this.FindByName<WebView>("DictWebView");
					var htmlSource = new HtmlWebViewSource();

					string html = "";

					if (_dictType.Equals(HTMLHelper.LANG_VI) ||
					   _dictType.Equals(HTMLHelper.LANG_EN))
					{
						html = HTMLHelper.createHTMLDict(_wordInfo, _dictType);
					}
					else
					{
						MajorObject major = new MajorObject();
						html = HTMLHelper.createHTMLForAnswer(_wordInfo, major);
					}

					htmlSource.Html = html;
					this.DictWebView.Source = htmlSource;
				}
			}
		}

		public DictionaryWebView()
		{
			InitializeComponent();
			Debug.WriteLine("DictionaryWebView" + this.Title);
		}
	}
}
