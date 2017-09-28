using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class StudyPage : ContentPage
	{
		private SqliteHelper sqlLiteHelper = SqliteHelper.Instance;

		public const string BTN_TITLE_AGAIN = "Again";
		public const string BTN_TITLE_HARD = "Hard";
		public const string BTN_TITLE_NORMAL = "Normal";
		public const string BTN_TITLE_EASY = "Easy";
		public const string BTN_TITLE_SHOW_ANSWER = "Show Answer";

		public const string SCREEN_MODE_NEWWORD = "New Word";
		public const string SCREEN_MODE_AGAIN = "Learn Again";
		public const string SCREEN_MODE_REVIEW = "Review";

		private string _screenMode = SCREEN_MODE_NEWWORD;
		private bool _isShowingAnswer = false;

		private WordInfo wordInfo;
		private List<WordInfo> newWordList = new List<WordInfo>();
		private List<WordInfo> studyAgainList = new List<WordInfo>();
		private List<WordInfo> reviewWordList = new List<WordInfo>();

		//private Button btnAgain;
		//private Button btnHard;
		//private Button btnNormal;
		//private Button btnEasy;

		//private Button btnShowAnswer;

		public StudyPage()
		{
			InitializeComponent();

			//btnAgain = this.FindByName<Button>("btnAgain");
			btnAgain.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnAgain.TextColor = Color.White;

			//btnHard = this.FindByName<Button>("btnHard");
			btnHard.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnHard.TextColor = Color.White;

			//btnNormal = this.FindByName<Button>("btnNormal");
			btnNormal.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnNormal.TextColor = Color.White;

			//btnEasy = this.FindByName<Button>("btnEasy");
			btnEasy.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnEasy.TextColor = Color.White;

			//btnShowAnswer = this.FindByName<Button>("btnShowAnswer");
			btnShowAnswer.BackgroundColor = CommonDefine.SECOND_COLOR;
			btnShowAnswer.TextColor = Color.White;

			Console.WriteLine("wdith :: " + App.Current.MainPage.Width.ToString());
			double width = App.Current.MainPage.Width;

			btnAgain.WidthRequest = width / 5;
			btnHard.WidthRequest = width / 5;
			btnNormal.WidthRequest = width / 5;
			btnEasy.WidthRequest = width / 5;

			btnAgain.Text 	= BTN_TITLE_AGAIN;
			btnHard.Text 	= BTN_TITLE_HARD;
			btnNormal.Text 	= BTN_TITLE_NORMAL;
			btnEasy.Text 	= BTN_TITLE_EASY;
			btnShowAnswer.Text = BTN_TITLE_SHOW_ANSWER;

			string title = SCREEN_MODE_NEWWORD;
			if (_screenMode.Equals(SCREEN_MODE_NEWWORD))
			{
				title = SCREEN_MODE_NEWWORD;

			}
			else if (_screenMode.Equals(SCREEN_MODE_AGAIN))
			{

				title = SCREEN_MODE_AGAIN;

			}
			else if (_screenMode.Equals(SCREEN_MODE_REVIEW))
			{

				title = SCREEN_MODE_REVIEW;
			}

			this.Title = title;

			//have to get review then learn again before get new word
			reviewWordList.AddRange(sqlLiteHelper.getReviewList());
			int countOfReview = sqlLiteHelper.getCountOfInreview(); //dont use [_reviewWordList count] because it could be changed while learning

			int limit = Common.loadTotalTarget() - countOfReview;
			if (limit > 0)
			{
				studyAgainList.AddRange(sqlLiteHelper.getStudyAgainListWithLimit(limit));
			}

			int countOfNew = Common.loadTotalTarget();
			countOfNew = countOfNew - countOfReview - studyAgainList.Count;

			if (countOfNew >= 0)
			{
				if (countOfNew > Common.loadDailyTarget())
				{
					countOfNew = Common.loadDailyTarget();
				}
			}
			else
			{
				countOfNew = 0;
			}

			sqlLiteHelper.pickUpRandom10WordsToStudyingQueue(countOfNew, false);
			newWordList.AddRange(sqlLiteHelper.getNewWordsList());

			//check if the list is not empty to switch screen mode, review is the highest priority
			if (reviewWordList.Count > 0)
			{
				_screenMode = SCREEN_MODE_REVIEW;

			}
			else if (studyAgainList.Count > 0)
			{
				_screenMode = SCREEN_MODE_AGAIN;

			}
			else if (newWordList.Count > 0)
			{
				_screenMode = SCREEN_MODE_NEWWORD;
			}

			setTitleAndButtonsState();

			wordInfo = getAWordFromCurrentList(null);
            showHide4ButtonsPanel(false);

			if (wordInfo != null) {
				displayQuestion (wordInfo);
	            
	            //[self showHideButtonsPanel:NO];
	            
	        } else {
	            //[self.navigationController popViewControllerAnimated:YES];
	            
	            //[[NSNotificationCenter defaultCenter] postNotificationName:@"noWordToStudyToday" object:nil];
	        }
		}

		/******************** PRIVATE FUNCTIONS AREA ********************/
		private void displayQuestion(WordInfo wd)
		{
			WebView webView = this.FindByName<WebView>("webView");
			var htmlSource = new HtmlWebViewSource();

			MajorObject major = Common.loadMajorFromProperties();
			string html = HTMLHelper.createHTMLForQuestion(wd, major);

			htmlSource.Html = html;
			webView.Source = htmlSource;

			_isShowingAnswer = false;
			btnDict.IsEnabled = false;
		}

		private void displayAnswer(WordInfo wd)
		{
			WebView webView = this.FindByName<WebView>("webView");
			var htmlSource = new HtmlWebViewSource();

			MajorObject major = Common.loadMajorFromProperties();
			string html = HTMLHelper.createHTMLForAnswer(wd, major);

			htmlSource.Html = html;
			webView.Source = htmlSource;

			_isShowingAnswer = true;
			btnDict.IsEnabled = true;
		}

		private void setTitleAndButtonsState()
		{
			string title = SCREEN_MODE_REVIEW;
			if (_screenMode.Equals(SCREEN_MODE_NEWWORD))
			{
				title = SCREEN_MODE_NEWWORD;

				btnEasy.IsEnabled = true;
				btnNormal.IsEnabled = true;

			}
			else if (_screenMode.Equals(SCREEN_MODE_AGAIN))
			{
				title = SCREEN_MODE_AGAIN;

				btnEasy.IsEnabled = false;
				btnNormal.IsEnabled = false;

			}
			else if (_screenMode.Equals(SCREEN_MODE_REVIEW))
			{
				title = SCREEN_MODE_REVIEW;

				btnEasy.IsEnabled = true;
				btnNormal.IsEnabled = true;
			}

			this.Title = title;

			lbNewWord.Text = string.Format("New: {0}", newWordList.Count);
			lbAgain.Text = string.Format("Again: {0}", studyAgainList.Count);
			lbReview.Text = string.Format("Review: {0}", reviewWordList.Count);
		}

		//only need to check sender in case click on Again button
		private WordInfo getAWordFromCurrentList(Button sender)
		{
			WordInfo res = null;
			//remove the old word from array
			if (_screenMode.Equals(SCREEN_MODE_AGAIN))
			{
				if (wordInfo != null)
				{
					studyAgainList.Remove(wordInfo);
				}
			} else if (_screenMode.Equals(SCREEN_MODE_REVIEW))
			{
				if (wordInfo != null)
				{
					reviewWordList.Remove(wordInfo);
					SqliteHelper.Instance.updateInreviewWordList(reviewWordList);
				}

			} else if (_screenMode.Equals(SCREEN_MODE_NEWWORD))
			{
				if (wordInfo != null)
				{
					newWordList.Remove(wordInfo);
					SqliteHelper.Instance.updatePickedWordList(newWordList);
				}
			}

			//get next word, if it's nil then switch array and screen mod
   			if (_screenMode.Equals(SCREEN_MODE_AGAIN))
			{
				if (studyAgainList.Count > 0)
				{
					res = studyAgainList.ElementAt(0);
				}

			}
			else if (_screenMode.Equals(SCREEN_MODE_REVIEW))
			{
				if (reviewWordList.Count > 0)
				{
					res = reviewWordList.ElementAt(0);
				}

			}
			else if (_screenMode.Equals(SCREEN_MODE_NEWWORD))
			{
				if (newWordList.Count > 0)
				{
					res = newWordList.ElementAt(0);
				}
			}

			if (res == null) {
        		//check if the list is not empty to switch screen mode, review is the highest priority
        		if (reviewWordList.Count > 0)
				{
					_screenMode = SCREEN_MODE_REVIEW;
					res = reviewWordList.ElementAt(0);

				}
				else if (studyAgainList.Count > 0)
				{
					_screenMode = SCREEN_MODE_AGAIN;
					res = studyAgainList.ElementAt(0);

				}
				else if (newWordList.Count > 0)
				{
					_screenMode = SCREEN_MODE_NEWWORD;
					res = newWordList.ElementAt(0);

				}
				else
				{
					//back to home in this case
				}

				//re-add old to again list after set screen mode
				if (sender != null && sender.Equals(btnAgain))
				{
					studyAgainList.Add(wordInfo);

					if (res == null)
					{
						_screenMode = SCREEN_MODE_AGAIN;
						res = studyAgainList.ElementAt(0);
					}
				}
    		}

			//update title
			setTitleAndButtonsState();

			return res;
		}

		//flag = false -> show [Show Answer] button
		//flag = true -> show [4 buttons]
		private void showHide4ButtonsPanel(bool flag)
		{
			if (wordInfo != null)
			{
				string[] btnsTitle = Algorithm.getInstance().nextIntervalStringsList(wordInfo);
				string btnAgainTitle = string.Format("{0}", BTN_TITLE_AGAIN);
				string btnHardTitle = string.Format("{0}", btnsTitle[1]);
				string btnNormalTitle = string.Format("{0}", btnsTitle[2]);
				string btnEasyTitle = string.Format("{0}", btnsTitle[3]);

				btnAgain.Text 	= btnAgainTitle;
				btnHard.Text 	= btnHardTitle;
				btnNormal.Text 	= btnNormalTitle;
				btnEasy.Text 	= btnEasyTitle;
				btnShowAnswer.Text = BTN_TITLE_SHOW_ANSWER;


				if (flag)
				{
					showAnswerPanel.IsVisible = false;
					fourButtonsPanel.IsVisible = true;
					//await showAnswerPanel.FadeTo(0, 300);
					//await fourButtonsPanel.FadeTo(1, 300);
				}
				else
				{
					showAnswerPanel.IsVisible = true;
					fourButtonsPanel.IsVisible = false;
					//await showAnswerPanel.FadeTo(0, 300);
					//await fourButtonsPanel.FadeTo(1, 300);
				}
			}
		}

		/***************** BUTTONS HANDLE *****************/
		void btnAgainClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnAgainClicked");
			//update word and update db
			if (wordInfo != null) {
				Algorithm.getInstance().updateWordProgressWithEaseOption(ref wordInfo, Algorithm.OPTION_AGAIN);
				SqliteHelper.Instance.updateWord(wordInfo);
		    }

			//show next word
			wordInfo = getAWordFromCurrentList((Button)sender);

			if (wordInfo != null)
			{
				displayQuestion(wordInfo);
				showHide4ButtonsPanel(false);
			}
			else
			{
				//        [self.navigationController popViewControllerAnimated:YES];

				//        [[NSNotificationCenter defaultCenter]
				//postNotificationName:@"completedDailyTarget" object:nil];
				Navigation.PopAsync(true);
				MessagingCenter.Send<StudyPage>(this, "CompletedDailyTarget");
			}
		}

		void btnHardClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnHardClicked");
			//update word and update db
			if (wordInfo != null)
			{
				Algorithm.getInstance().updateWordProgressWithEaseOption(ref wordInfo, Algorithm.OPTION_HARD);
				SqliteHelper.Instance.updateWord(wordInfo);
			}

			//show next word
			wordInfo = getAWordFromCurrentList((Button)sender);

			if (wordInfo != null)
			{
				displayQuestion(wordInfo);
				showHide4ButtonsPanel(false);
			}
			else
			{
				//        [self.navigationController popViewControllerAnimated:YES];

				//        [[NSNotificationCenter defaultCenter]
				//postNotificationName:@"completedDailyTarget" object:nil];
			}
		}

		void btnNormalClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnNormalClicked");
			//update word and update db
			if (wordInfo != null)
			{
				Algorithm.getInstance().updateWordProgressWithEaseOption(ref wordInfo, Algorithm.OPTION_GOOD);
				SqliteHelper.Instance.updateWord(wordInfo);
			}

			//show next word
			wordInfo = getAWordFromCurrentList((Button)sender);

			if (wordInfo != null)
			{
				displayQuestion(wordInfo);
				showHide4ButtonsPanel(false);
			}
			else
			{
				//        [self.navigationController popViewControllerAnimated:YES];

				//        [[NSNotificationCenter defaultCenter]
				//postNotificationName:@"completedDailyTarget" object:nil];
			}
		}

		void btnEasyClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnEasyClicked");
			//update word and update db
			if (wordInfo != null)
			{
				Algorithm.getInstance().updateWordProgressWithEaseOption(ref wordInfo, Algorithm.OPTION_EASY);
				SqliteHelper.Instance.updateWord(wordInfo);
			}

			//show next word
			wordInfo = getAWordFromCurrentList((Button)sender);

			if (wordInfo != null)
			{
				displayQuestion(wordInfo);
				showHide4ButtonsPanel(false);
			}
			else
			{
				//        [self.navigationController popViewControllerAnimated:YES];

				//        [[NSNotificationCenter defaultCenter]
				//postNotificationName:@"completedDailyTarget" object:nil];
			}
		}

		void btnShowAnswerClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnShowAnswerClicked");
			if (wordInfo != null)
			{
                displayAnswer(wordInfo);
				showHide4ButtonsPanel(true);
			}
		}

		void btnDictionaryClicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("btnShowAnswerClicked");
			if (wordInfo != null)
			{
				if (_isShowingAnswer == true)
				{
					DictionaryTabPage dictTabPage = new DictionaryTabPage();
					dictTabPage.wordInfo = wordInfo;
					dictTabPage.showLazzyBeeTab = false;
					Navigation.PushAsync(dictTabPage);
				}
			}
		}
	}
}
