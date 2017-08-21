using System;
using System.Collections.Generic;
using LazzyBee;

public class Algorithm {
	public const int OPTION_AGAIN = 0;  //choose to learn a word again
	public const int OPTION_HARD = 1;
	public const int OPTION_GOOD = 2;
	public const int OPTION_EASY = 3;

	public const string STR_TIME_LEARN_AGAIN = "10min";
	public const int TIME_LEARN_AGAIN = 600;    //10 mins
	public const int SECONDS_PERDAY = 86400;
	public const float BONUS_EASY = 1.4f;
	public const int MIN_FACTOR = 1300;
	public const int FORGET_FINE = 300;
	public const int DEFAULT_EFACTOR = 2500;

	private static Algorithm _instance = null;

	public static Algorithm getInstance() {
		if (_instance == null) {
			_instance = new Algorithm();

		}
		return _instance;
	}

	/************ mark internal function **************/
	private int factorAdditionValue(int easeOption) {
		int res = 0;

		if (easeOption == OPTION_AGAIN) {
			res = -300;

		} else if (easeOption == OPTION_AGAIN) {
			res = -150;

		} else if (easeOption == OPTION_GOOD) {
			res = 0;

		} else if (easeOption == OPTION_AGAIN) {
			res = 150;

		} else {
			res = 0;
		}

		return res;
	}

	/*
 	* Return string of the next time span to review those are corresponded to ease level
 	*/
	private string nextIntervalStringWithEaseOption(WordInfo wordInfo, int easeOption) {
		string str;
		int ivl = nextIntervalBySecondsWithEaseOption(wordInfo, easeOption);

		if (ivl < SECONDS_PERDAY) {
			str =  STR_TIME_LEARN_AGAIN;

		} else {
			float day = ivl / SECONDS_PERDAY;
			if (day <= 30)
				str = String.Format("{0} {1}", (int)Math.Round(day), "day(s)");
			
			else {
				float month = (float)day / 30;
				str = String.Format("{0:0.0} {1}", month, "month(s)");

				if (month > 12) {
					float year = month / 12;

					str = String.Format("{0:0.0} {1}", year, "year(s)");
				}
			}
		}

		return str;
	}

	/**
 	* Return the next interval for CARD, in seconds.
 	*/

	private int nextIntervalBySecondsWithEaseOption(WordInfo wordInfo, int easeOption) {
		if (easeOption == OPTION_AGAIN) {
			return TIME_LEARN_AGAIN; //*10 minutes
		}

		return (nextIntervalByDaysWithEaseOption(wordInfo, easeOption) * SECONDS_PERDAY);
	}

	/**
 	* Ideal next interval by days for CARD, given EASE > 0
 	*/

	private int nextIntervalByDaysWithEaseOption(WordInfo wordInfo, int easeOption) {
		int delay = daysLate(wordInfo);
		int interval = 0;

		double fct = Int32.Parse(wordInfo.eFactor) / 1000.0;
		int intLastInterval = Int32.Parse(wordInfo.lastInterval);
		int ivl_hard = Math.Max((int)((intLastInterval + delay/4) * 1.2), intLastInterval + 1);
		int ivl_good = Math.Max((int)((intLastInterval + delay/2) * fct), ivl_hard + 1);
		int ivl_easy = Math.Max((int)((intLastInterval + delay) * fct * BONUS_EASY), ivl_good + 1);

		if (easeOption == OPTION_HARD) {
			interval = ivl_hard;

		} else if (easeOption == OPTION_GOOD) {
			interval = ivl_good;

		} else if (easeOption == OPTION_EASY) {
			interval = ivl_easy;
		}

		// Should we maximize the interval?
		return interval;
	}

	/**
 	* Number of days later than scheduled.
 	* only for reviewing, not just learned few minute ago
 	*/

	private int daysLate(WordInfo wordInfo) {
		int queue = Int16.Parse(wordInfo.queue);

		if (queue != WordInfo.QUEUE_REVIEW) {
			return 0;
		}

		int due = Int32.Parse(wordInfo.due);
		int now = DateTimeHelper.getCurrentDateTimeInSeconds();    //have to get exactly date time in sec

		int diff_day = (int)(now - due)/SECONDS_PERDAY;
		return Math.Max(0, diff_day);
	}

	/************ mark external function **************/
	/*
 	* Return strings of the next time spans to review those are corresponded to ease level
 	*/

	public string[] nextIntervalStringsList(WordInfo wordInfo) {
		List<string> res = new List<string>();
		for (int i = 0; i < 4; i++){
				res.Add(nextIntervalStringWithEaseOption(wordInfo, i));
		}

		return res.ToArray();
	}

	/**
	 * Whenever a Card is answered, call this function on Card.
	 * Scheduler will update the following parameters into Card's instance:
	 * <ul>
	 * <li>due
	 * <li>last_ivl
	 * <li>queue
	 * <li>e_factor
	 * <li>rev_count
	 * </ul>
	 * After 'answerCard', the caller will check Card's data for further decisions
	 * (update database or/and put it back to app's queue)
	 */

	public  void updateWordProgressWithEaseOption(ref WordInfo wordInfo, int easeOption) {
		int nextIvl = nextIntervalBySecondsWithEaseOption(wordInfo, easeOption);
		int current = DateTimeHelper.getCurrentDateTimeInSeconds();	//have to get exactly date time in seconds

		//Now we decrease for EASE_AGAIN only when it from review queue
		int queue 	= Int16.Parse(wordInfo.queue);
		int eFactor = Int16.Parse(wordInfo.eFactor);

		if (queue == WordInfo.QUEUE_REVIEW && easeOption == OPTION_AGAIN) {
			eFactor = eFactor - FORGET_FINE;
			wordInfo.eFactor = eFactor.ToString();

		} else {
			eFactor = Math.Max(MIN_FACTOR, (eFactor + factorAdditionValue(easeOption)));
			wordInfo.eFactor = eFactor.ToString();
		}

		if (nextIvl < SECONDS_PERDAY) {
			//User forget card or just learn
         	//We don't re-count 'due', because app will put it back to learned queue

			wordInfo.queue = WordInfo.QUEUE_AGAIN.ToString();

			//Reset last-interval to reduce next review
			wordInfo.lastInterval = "0";

		} else {
			wordInfo.queue = WordInfo.QUEUE_REVIEW.ToString();
			wordInfo.due = (current + nextIvl).ToString();
			wordInfo.lastInterval =  nextIntervalByDaysWithEaseOption(wordInfo, easeOption).ToString();
		}
	}
}
