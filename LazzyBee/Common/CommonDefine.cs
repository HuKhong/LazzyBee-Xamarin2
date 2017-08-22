using Xamarin.Forms;


public class CommonDefine
{
    static public Color MAIN_COLOR = Color.FromRgb(255, 200, 47);
    static public Color SECOND_COLOR = Color.FromRgb(60, 159, 30);
	static public Color BLUE_COLOR = Color.FromRgb(0, 103, 194);

	public const string DEFAULT_SUBJECT = "common";

	//default settings
	public const string DEFAULT_LEVEL = "2";
	public const string DEFAULT_NEW_CARD_PER_DAY = "5";
	public const string DEFAULT_TOTAL_CARD_PER_DAY = "40";
	public const string DEFAULT_TTS_SPEED = "0.4f";
	public const string DEFAULT_TIME_TO_SHOW_ANSWER = "3";
	public const string	DEFAULT_AUTO_PLAY_SOUND = "1";
	public const string DEFAULT_DISPLAY_MEANING = "1";
	public const string	DEFAULT_NOTIFICATION = "1";
	public const string DEFAULT_TIME_SHOW_NOTIFICATION = "13h30";

	//settings key
	public const string SETTINGS_MY_LEVEL_KEY = "LowestLevel";
	public const string SETTINGS_NEW_CARD_KEY = "DailyTarget";
	public const string SETTINGS_TOTAL_CARD_KEY = "DailyTotalTarget";
	public const string SETTINGS_TTS_SPEED_KEY = "SpeakingSpeed";
	public const string SETTINGS_TIME_SHOW_ANSWER_KEY = "TimeToShowAnswer";
	public const string SETTINGS_AUTOPLAY_KEY = "AutoPlay";
	public const string SETTINGS_DISPLAY_MEANING_KEY = "DisplayMeaning";
	public const string SETTINGS_NOTIFICATION_ONOFF_KEY = "ReminderOnOff";
	public const string SETTINGS_NOTIFICATION_TIME_KEY = "RemindTime";

	public const string SELECTED_MAJOR_KEY = "SelectedMajor";
	public const string DB_VERSION_KEY = "DatabaseVersion";
	public const string COMPLETED_FLAG_KEY = "CompletedDailyTargetFlag";
	public const string IS_FIRST_RUN_KEY = "IsFirstRun";
	public const string BACKUP_CODE_KEY = "BackupCode";
	public const string STREAK_INFO_KEY = "StreakInfo";
	public const string SHOW_GUIDE_KEY = "ShowGuide";
	public const string REVERSE_ENABL_KEY = "ReverseEnable";

	private const string PROGRESS_INREVIEW_KEY = "inreview";
	private const string PROGRESS_NEWWORDS_KEY = "newwords";
	private const string PROGRESS_PICKEDWORD_KEY = "pickedwords";
}