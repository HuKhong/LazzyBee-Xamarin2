using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class StatisticsPage : ContentPage
	{
		private List<WordInfo> words;
		private Dictionary<string, object> wordsGroupByLv = new Dictionary<string, object>();

		public StatisticsPage()
		{
			InitializeComponent();

			btnShare.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnShare.TextColor = Color.White;

			double width = App.Current.MainPage.Width;
			double height = App.Current.MainPage.Height;

			//loadingIndicator.TranslationX = width / 2 - 10;
			//loadingIndicator.TranslationY = height / 2 - 50;

			//loadingIndicator.IsRunning = true;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ThreadStart threadStart = new ThreadStart(_loadingDataFromDBAndProcess);
			Thread myThread = new Thread(threadStart);
			myThread.Start();
		}

		private void _loadingDataFromDBAndProcess()
		{
			words = SqliteHelper.Instance.getStudiedList();
			Dictionary<string, string> dictCount = SqliteHelper.Instance.loadCountWordByLevelFromSystem();  //count words in each level
			int streakCount = Common.countStreak();

			Device.BeginInvokeOnMainThread(() =>
			{
				foreach (WordInfo w in words)
				{
					object obj;
					List<WordInfo> arr;

					if (wordsGroupByLv.TryGetValue(w.level, out obj) == false || obj == null)
					{
						arr = new List<WordInfo>();
					}
					else
					{
						arr = obj as List<WordInfo>;
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
						lbLv2Count.Text = arr.Count.ToString();

					}
					else if (w.level.Equals("3"))
					{
						lbLv3Count.Text = arr.Count.ToString();

					}
					else if (w.level.Equals("4"))
					{
						lbLv4Count.Text = arr.Count.ToString();

					}
					else if (w.level.Equals("5"))
					{
						lbLv5Count.Text = arr.Count.ToString();

					}
					else if (w.level.Equals("6"))
					{
						lbLv6Count.Text = arr.Count.ToString();

					}
					else if (w.level.Equals("7"))
					{
						lbLv7Count.Text = arr.Count.ToString();

					}
					else if (w.level.Equals("8"))
					{
						lbLv8Count.Text = arr.Count.ToString();

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

				//fill progress color by level
				foreach (string key in dictCount.Keys)
				{
					object obj;
					List<WordInfo> arr;

					if (wordsGroupByLv.TryGetValue(key, out obj) == false || obj == null)
					{
						arr = new List<WordInfo>();
					}
					else
					{
						arr = obj as List<WordInfo>;
					}

					if (key.Equals("1"))
					{
						col1down.HeightRequest = (1 - (arr.Count / float.Parse(dictCount[key]))) * col1up.HeightRequest;
					}
					else if (key.Equals("2"))
					{
						col2down.HeightRequest = (1 - (arr.Count / float.Parse(dictCount[key]))) * col1up.HeightRequest;
					}
					else if (key.Equals("3"))
					{
						col3down.HeightRequest = (1 - (arr.Count / float.Parse(dictCount[key]))) * col1up.HeightRequest;
					}
					else if (key.Equals("4"))
					{
						col4down.HeightRequest = (1 - (arr.Count / float.Parse(dictCount[key]))) * col1up.HeightRequest;
					}
					else if (key.Equals("5"))
					{
						col5down.HeightRequest = (1 - (arr.Count / float.Parse(dictCount[key]))) * col1up.HeightRequest;
					}
					else if (key.Equals("6"))
					{
						col6down.HeightRequest = (1 - (arr.Count / float.Parse(dictCount[key]))) * col1up.HeightRequest;
					}
					else if (key.Equals("7"))
					{
						col7down.HeightRequest = (1 - (arr.Count / float.Parse(dictCount[key]))) * col1up.HeightRequest;
					}
					else if (key.Equals("8"))
					{
						col8down.HeightRequest = (1 - (arr.Count / float.Parse(dictCount[key]))) * col1up.HeightRequest;
					}
				}

				lbStreak.Text = string.Format("{0} day(s)", streakCount.ToString());
				////loadingIndicator.IsRunning = false;
			});
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
