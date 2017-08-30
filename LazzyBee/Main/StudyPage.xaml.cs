using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace LazzyBee
{
	public partial class StudyPage : ContentPage
	{
		private SqliteHelper sqlLiteHelper = SqliteHelper.Instance;

		public const string SCREEN_MODE_NEWWORD = "New Word";
		public const string SCREEN_MODE_AGAIN = "Learn Again";
		public const string SCREEN_MODE_REVIEW = "Review";

		public string screenMode = SCREEN_MODE_NEWWORD;

		private WordInfo wordInfo;
		private List<WordInfo> newWordList = new List<WordInfo>();
		private List<WordInfo> studyAgainList = new List<WordInfo>();
		private List<WordInfo> reviewWordList = new List<WordInfo>();

		public StudyPage()
		{
			InitializeComponent();

			string title = SCREEN_MODE_NEWWORD;
			if (screenMode.Equals(SCREEN_MODE_NEWWORD))
			{
				title = SCREEN_MODE_NEWWORD;

			}
			else if (screenMode.Equals(SCREEN_MODE_AGAIN))
			{

				title = SCREEN_MODE_AGAIN;

			}
			else if (screenMode.Equals(SCREEN_MODE_REVIEW))
			{

				title = SCREEN_MODE_REVIEW;
			}

			this.Title = title;

			//have to get review then learn again before get new word
			reviewWordList.AddRange(sqlLiteHelper.getReviewList());
			int countOfReview = sqlLiteHelper.getCountOfInreview(); //dont use [_reviewWordList count] because it could be changed while learning

			int limit = Common.getTotalTarget() - countOfReview;
			if (limit > 0)
			{
				studyAgainList.AddRange(sqlLiteHelper.getStudyAgainListWithLimit(limit));
			}

			int countOfNew = Common.getTotalTarget();
			countOfNew = countOfNew - countOfReview - studyAgainList.Count;

			if (countOfNew >= 0)
			{
				if (countOfNew > Common.getDailyTarget())
				{
					countOfNew = Common.getDailyTarget();
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
				screenMode = SCREEN_MODE_REVIEW;

			}
			else if (studyAgainList.Count > 0)
			{
				screenMode = SCREEN_MODE_AGAIN;

			}
			else if (newWordList.Count > 0)
			{
				screenMode = SCREEN_MODE_NEWWORD;
			}

			setTitleAndButtonsState();

			wordInfo = getAWordFromCurrentList();

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

			MajorObject major = new MajorObject();
			string html = HTMLHelper.createHTMLForQuestion(wd, major);

			htmlSource.Html = html;
			webView.Source = htmlSource;
		}

		private void displayAnswer(WordInfo wd)
		{
			WebView webView = this.FindByName<WebView>("webView");
			var htmlSource = new HtmlWebViewSource();

			MajorObject major = new MajorObject();
			string html = HTMLHelper.createHTMLForAnswer(wd, major);

			htmlSource.Html = html;
			webView.Source = htmlSource;
		}

		private void setTitleAndButtonsState()
		{
			string title = SCREEN_MODE_REVIEW;
			if (screenMode.Equals(SCREEN_MODE_NEWWORD))
			{
				title = SCREEN_MODE_NEWWORD;

				//btnEasy.enabled = YES;
				//btnNorm.enabled = YES;

			}
			else if (screenMode.Equals(SCREEN_MODE_AGAIN))
			{
				title = SCREEN_MODE_AGAIN;

				//btnEasy.enabled = NO;
				//btnNorm.enabled = NO;

			}
			else if (screenMode.Equals(SCREEN_MODE_REVIEW))
			{
				title = SCREEN_MODE_REVIEW;

				//btnEasy.enabled = YES;
				//btnNorm.enabled = YES;
			}

			this.Title = title;
		}

		//only need to check sender in case click on Again button
		private WordInfo getAWordFromCurrentList()
		{
			WordInfo res = null;
			//remove the old word from array
			if (screenMode.Equals(SCREEN_MODE_AGAIN))
			{
				if (wordInfo != null)
				{
					studyAgainList.Remove(wordInfo);
				}
			} else if (screenMode.Equals(SCREEN_MODE_REVIEW))
			{
				if (wordInfo != null)
				{
					reviewWordList.Remove(wordInfo);
				}

			} else if (screenMode.Equals(SCREEN_MODE_NEWWORD))
			{
				if (wordInfo != null)
				{
					newWordList.Remove(wordInfo);
				}
			}

			//get next word, if it's nil then switch array and screen mod
   			if (screenMode.Equals(SCREEN_MODE_AGAIN))
			{
				if (studyAgainList.Count > 0)
				{
					res = studyAgainList.ElementAt(0);
				}

			}
			else if (screenMode.Equals(SCREEN_MODE_REVIEW))
			{
				if (reviewWordList.Count > 0)
				{
					res = reviewWordList.ElementAt(0);
				}

			}
			else if (screenMode.Equals(SCREEN_MODE_NEWWORD))
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
					screenMode = SCREEN_MODE_REVIEW;
					res = reviewWordList.ElementAt(0);

				}
				else if (studyAgainList.Count > 0)
				{
					screenMode = SCREEN_MODE_AGAIN;
					res = studyAgainList.ElementAt(0);

				}
				else if (newWordList.Count > 0)
				{
					screenMode = SCREEN_MODE_NEWWORD;
					res = newWordList.ElementAt(0);

				}
				else
				{
					//back to home in this case
				}

				//re-add old to again list after set screen mode

				//update title
				setTitleAndButtonsState();
    		}

			return res;
		}
	}
}
