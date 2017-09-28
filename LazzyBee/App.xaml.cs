using Xamarin.Forms;

namespace LazzyBee
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			//configs
			initialConfiguration();

			MainPage = new LazzyBeePage();

		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		public void initialConfiguration() {
			//  NSString* curLang = [[NSUserDefaults standardUserDefaults] objectForKey:@"CurrentLanguageInApp"];

			//  if (curLang == nil) {

			//LocalizationSetLanguage(@"vi");
			//curLang = @"vi";
			//      [[NSUserDefaults standardUserDefaults] setObject:curLang forKey:@"CurrentLanguageInApp"];

			//  } else {
			//      if ([curLang isEqualToString:@"vi"]) {
			//	LocalizationSetLanguage(@"vi");

			//      } else if ([curLang isEqualToString:@"en"]) {

			//	LocalizationSetLanguage(@"en");
			//      }
			//  }
			//  [FIRAnalytics setUserPropertyString:curLang forName:PROPERTY_SELECTED_LANG];

			//speed		    
			if (Common.checkKey(CommonDefine.SETTINGS_TTS_SPEED_KEY) == false) {

				Common.saveSpeakingSpeed(double.Parse(CommonDefine.DEFAULT_TTS_SPEED));
		    }
		    
		    //time to remind
		    if (Common.checkKey(CommonDefine.SETTINGS_NOTIFICATION_TIME_KEY) == false) {

				Common.saveRemindTime(CommonDefine.DEFAULT_TIME_SHOW_NOTIFICATION);
		    }
		    
		    //level
			if (Common.checkKey(CommonDefine.SETTINGS_MY_LEVEL_KEY) == false) {

				Common.saveSelectedLevel(CommonDefine.DEFAULT_LEVEL);
		    }

			//reminder on/off
			if (Common.checkKey(CommonDefine.SETTINGS_NOTIFICATION_ONOFF_KEY) == false)
			{

				Common.saveRemindOnOffFlag(bool.Parse(CommonDefine.DEFAULT_NOTIFICATION));
			}

			//autoplay sound
			if (Common.checkKey(CommonDefine.SETTINGS_AUTOPLAY_KEY) == false)
			{

				Common.saveAutoplayFlag(bool.Parse(CommonDefine.DEFAULT_AUTO_PLAY_SOUND));
			}

			//display meaning
			if (Common.checkKey(CommonDefine.SETTINGS_DISPLAY_MEANING_KEY) == false)
			{
				Common.saveDisplayMeaningFlag(bool.Parse(CommonDefine.DEFAULT_DISPLAY_MEANING));
			}
		    
		    //new word target
			if (Common.checkKey(CommonDefine.SETTINGS_NEW_CARD_KEY) == false)
			{
				Common.saveDailyTarget(int.Parse(CommonDefine.DEFAULT_NEW_CARD_PER_DAY));
			}

			//total target
			if (Common.checkKey(CommonDefine.SETTINGS_TOTAL_CARD_KEY) == false)
			{

				Common.saveTotalTarget(int.Parse(CommonDefine.DEFAULT_TOTAL_CARD_PER_DAY));
			}

		    //time to show answer
			if (Common.checkKey(CommonDefine.SETTINGS_TIME_SHOW_ANSWER_KEY) == false)
			{
				Common.saveWaitingTimeToShowAnswer(int.Parse(CommonDefine.DEFAULT_TIME_TO_SHOW_ANSWER));
			}
		    
			//db version
			if (Common.checkKey(CommonDefine.DB_VERSION_KEY) == false)
			{
				Common.saveCurrentDBVersion("8");
			}
		    
		    if (Common.checkKey(CommonDefine.IS_FIRST_RUN_KEY) == false)
			{
				Common.saveIsFirstRunFlag(true);
			}

			if (Common.checkKey(CommonDefine.SHOW_GUIDE_KEY) == false)
			{
				Common.saveShowGuideFlag(true);
			}

		    if (Common.checkKey(CommonDefine.REVERSE_ENABLE_KEY) == false)
			{
				Common.saveReverseEnableFlag(false);
			}
		}
	}
}
