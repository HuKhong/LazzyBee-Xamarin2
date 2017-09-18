using System;
namespace LazzyBee
{
	public class MajorObject
	{
		public string majorName { get; set; }
		public string majorThumbnail { get; set; }
		public bool checkFlag { get; set; }
		public bool enabled { get; set; }
		public string displayName
		{
			get { return displayNameFunc(); }
		}
		public string imgRadioBtn { get; set; }

		public MajorObject()
		{
			majorName = CommonDefine.DEFAULT_SUBJECT;
	        majorThumbnail = "images/majors/blank.png";
			checkFlag = false;
	        enabled = false;
			imgRadioBtn = "images/majors/radiobtn-unchecked.png";
		}

		public MajorObject(string _majorName, string _thumbnail, bool _checkFlag)
		{
			majorName = _majorName;
	        majorThumbnail = _thumbnail;
	        checkFlag = _checkFlag;
	        enabled = false;
		}

		public string displayNameFunc() {
			string res = "";
			    
			if (majorName.ToLower().Equals("economic")) {
		        
		        res = "Economy";		        
		    } else if (majorName.ToLower().Equals("ielts")) {
		        
		        res = "IELTS";		        
		    } else if (majorName.ToLower().Equals("it")) {
		        
		        res = "IT";		        
		    } else if (majorName.ToLower().Equals("science")) {
		        
		        res = "Science";		        
		    } else if (majorName.ToLower().Equals("medicine")) {
		        
		        res = "Medicine";		        
		    }  else if (majorName.ToLower().Equals("toeic")) {
		        
		        res = "Toeic";		        
		    } else if (majorName.ToLower().Equals("coming soon")) {
		        
		        res = "Coming soon";
		    }
		    
		    return res;
		}
	}
}
