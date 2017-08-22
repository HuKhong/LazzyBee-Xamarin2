using System;
namespace LazzyBee
{
	public class MajorObject
	{
		public string majorName;
		public string majorThumbnail;
		public bool checkFlag;
		public bool enabled;

		public MajorObject()
		{
		}

		public MajorObject(string _majorName, string _thumbnail, bool _checkFlag)
		{
			majorName = _majorName;
	        majorThumbnail = _thumbnail;
	        checkFlag = _checkFlag;
	        enabled = false;
		}

		public string displayName() {
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
