using System;
namespace LazzyBee
{
	[Serializable]
	public class WordInfo
	{
		public const int QUEUE_UNKNOWN = 0;
		public const int QUEUE_AGAIN = 1;
		public const int QUEUE_REVIEW = 2;
		public const int QUEUE_NEW_WORD = 3;
		public const int QUEUE_SUSPENDED = -1;  //ignore
		public const int QUEUE_DONE = -2;   //learned

		public string wordid;
		public string gid;
		public string question { get; set; }
		public string answers;
		public string status;
		public string subcats;
		public string package;
		public string level;
		public string queue;
		public string due;
		public string revCount;
		public string lastInterval;
		public string eFactor;
		public string langVN;
		public string langEN;
		public string userNote;
		public int priority;

		public bool isFromServer;

		public WordInfo()
		{
			this.wordid = @"";
	        this.gid = @"";
	        this.question = @"";
	        this.answers = @"";
	        this.subcats = @"";
	        this.status = @"";
	        this.package = @"";
	        this.level = @"";
	        this.queue = @"";
	        this.due = @"";
	        this.revCount = @"";
	        this.lastInterval = @"";
	        this.eFactor = @"";
	        this.langEN = @"";
	        this.langVN = @"";
	        this.userNote = @"";
	        this.priority = 0;
			this.isFromServer = false;
		}
	}
}