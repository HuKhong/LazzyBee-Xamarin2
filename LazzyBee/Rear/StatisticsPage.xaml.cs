using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class StatisticsPage : ContentPage
	{
		private List<WordInfo> words = SqliteHelper.Instance.getStudiedList();
		private Dictionary<string, object> wordsGroupByLv = new Dictionary<string, object>();

		public StatisticsPage()
		{
			InitializeComponent();

			btnShare.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnShare.TextColor = Color.White;
		}

		private void _processData()
		{
			foreach (WordInfo w in words)
			{
				List<WordInfo> arr = wordsGroupByLv[w.level] as List<WordInfo>;

				if (arr == null)
				{
					arr = new List<WordInfo>();
				}

				arr.Add(w);

				wordsGroupByLv.Remove(w.level);
				wordsGroupByLv.Add(w.level, arr);

				//number of words by level
				if (w.level.Equals("1"))
				{
					lbLv1Count.Text = arr.Count.ToString();

				}
				else if (w.level.Equals("2"))
				{
					lbLv1Count.Text = arr.Count.ToString();

				}
				else if (w.level.Equals("3"))
				{
					lbLv1Count.Text = arr.Count.ToString();

				}
				else if (w.level.Equals("4"))
				{
					lbLv1Count.Text = arr.Count.ToString();

				}
				else if (w.level.Equals("5"))
				{
					lbLv1Count.Text = arr.Count.ToString();

				}
				else if (w.level.Equals("6"))
				{
					lbLv1Count.Text = arr.Count.ToString();

				}
				else if (w.level.Equals("7"))
				{
					lbLv1Count.Text = arr.Count.ToString();

				}
				else if (w.level.Equals("8"))
				{
					lbLv1Count.Text = arr.Count.ToString();

				}
			}

			if (words.Count == 1)
			{
				lbTotal.Text = string.Format("Total: {0} word", words.Count);
			}
			else
			{
				lbTotal.Text = string.Format("Total: {0} words", words.Count);
			}

			//fill progress by level
			Dictionary<string, string> dictCount = SqliteHelper.Instance.loadCountWordByLevelFromSystem();

			foreach (string key in dictCount.Keys)
			{
				List<WordInfo> arr = wordsGroupByLv[key] as List<WordInfo>;

				if (key.Equals("1"))
				{
					col1up.HeightRequest = (1 - (arr.Count / int.Parse(dictCount[key]))) * col1down.HeightRequest;
				}
				else if (key.Equals("2"))
				{
					col2up.HeightRequest = (1 - (arr.Count / int.Parse(dictCount[key]))) * col1down.HeightRequest;
				}
				else if (key.Equals("3"))
				{
					col3up.HeightRequest = (1 - (arr.Count / int.Parse(dictCount[key]))) * col1down.HeightRequest;
				}
				else if (key.Equals("4"))
				{
					col4up.HeightRequest = (1 - (arr.Count / int.Parse(dictCount[key]))) * col1down.HeightRequest;
				}
				else if (key.Equals("5"))
				{
					col5up.HeightRequest = (1 - (arr.Count / int.Parse(dictCount[key]))) * col1down.HeightRequest;
				}
				else if (key.Equals("6"))
				{
					col6up.HeightRequest = (1 - (arr.Count / int.Parse(dictCount[key]))) * col1down.HeightRequest;
				}
				else if (key.Equals("7"))
				{
					col7up.HeightRequest = (1 - (arr.Count / int.Parse(dictCount[key]))) * col1down.HeightRequest;
				}
				else if (key.Equals("8"))
				{
					col8up.HeightRequest = (1 - (arr.Count / int.Parse(dictCount[key]))) * col1down.HeightRequest;
				}
			}
		}

		void OnTapGestureRecognizerTapped(object sender, EventArgs args)
		{
			Debug.WriteLine("OnTapGestureRecognizerTapped");
			Device.OpenUri(new Uri("http://www.lazzybee.com/blog/can_you_learn_2000_words_per_year"));
		}

		void btnShareClicked(object sender, System.EventArgs e)
		{

		}
	}
}
