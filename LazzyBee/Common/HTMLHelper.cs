using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using LazzyBee;
using Newtonsoft.Json.Linq;

public class HTMLHelper {
	//private static string SPEAKER_IMG_LINK = "https://firebasestorage.googleapis.com/v0/b/lazeebee-977.appspot.com/o/speaker.png?alt=media&token=be4d8dc7-b5c0-4f3d-b5e2-893c30ec18ee";
private static string SPEAKER_IMG_LINK = "images/icons/ic_speaker.png";

	public static string createHTMLForQuestion (WordInfo word, MajorObject major) {
		Debug.WriteLine("createHTMLForQuestion");
		string package = "";
		string packageLowcase = CommonDefine.DEFAULT_SUBJECT;

		package = major.displayName();
		packageLowcase = major.majorName.ToLower();
		       
		if (!packageLowcase.Equals(CommonDefine.DEFAULT_SUBJECT)) {

	        //parse the answer to dictionary object
			JObject answers = JObject.Parse(word.answers);

			//A word may has many meanings corresponding to many fields (common, it, economic...)
			//The meaning of each field is considered as a package
			var dictPackages = answers["packages"];
			var dictSinglePackage = dictPackages[packageLowcase];
		        
	        if (dictSinglePackage == null) {
	            
	            package = @"";
	        }
	    }

		string htmlString = "<!DOCTYPE html>" +
			"<html>" +
			"<head>" +
			"<style>" +
			"figure {{" +
			"   text-align: center;" +
			"   margin: auto;" +
			"}}" +
			"figure.image img {{" +
			"   width: 100%% !important;" +
			"   height: auto !important;" +
			"}}" +
			"figcaption {{" +
			"   font-size: 10px;" +
			"}}" +
			"a {{" +
			"   margin-top:10px;" +
			"}}" +
			"</style>" +
			"<script>" +
			//play the text
			"function playText(content, rate) {{" +
			"   var speaker = new SpeechSynthesisUtterance();" +
			"   speaker.text = content;" +
			"   speaker.lang = 'en-US';" +
			"   speaker.rate = rate;" + //0.1
			"   speaker.pitch = 1.0;" +
			"   speaker.volume = 1.0;" +
			"   speechSynthesis.cancel();" +
			"   speechSynthesis.speak(speaker);" +
			"}}" +
			//cancel speech
			"function cancelSpeech() {{" +
			"   speechSynthesis.pause();" +
			"   speechSynthesis.cancel();" +
			"}}" +
			"</script>" +
			"</head>" +
			"<body>" +
			"<div style='width:100%%'>" +
			"{0}" +  //strWordIconTag
			"</div>" +
			"</body>" +
			"</html>";

		try {
			float speed = float.Parse(Common.loadSettingValueByKey(CommonDefine.SETTINGS_TTS_SPEED_KEY));
			Debug.WriteLine("createHTMLForQuestion speed :: " +speed.ToString());
			string strWordIconTag = @"<div style='float:left; width:90%;text-align: center;'>" +
	                        "<strong style='font-size:18pt;'> {0} </strong>" +   //%@ will be replaced by word.question
				"</div>" +
				"<div style='float:right; width:10%;'>" +
				"<a onclick='playText(\"{1}\", {2:0.0});'><img width=10% src='{3}'/><p>" +
				"</div>" +
				"<div style='width:90%'>" +
				"<center>{4}</center>" +
                "</div>";

			strWordIconTag = String.Format(strWordIconTag, word.question, word.question, speed, SPEAKER_IMG_LINK, package);
			Debug.WriteLine("createHTMLForQuestion 2 :: " +strWordIconTag);
			Debug.WriteLine("=======================");
			Debug.WriteLine("createHTMLForQuestion htmlString :: " +htmlString);
			htmlString = String.Format(htmlString, strWordIconTag);
			Debug.WriteLine("createHTMLForQuestion 3");
			Debug.WriteLine(htmlString);

		} catch (Exception e) {
			Debug.WriteLine("createHTMLForQuestion :: Exception :; " +e.ToString());
		}

		return htmlString;
	}

	public static string createHTMLForAnswer (WordInfo word, MajorObject major) {
		Debug.WriteLine("createHTMLForQuestion");
		string htmlString = "";
		string imageLink = "";
		string strExplanation = "";
		string strExample = "";
		string strMeaning = "";
		string package = "";
		string packageLowcase = CommonDefine.DEFAULT_SUBJECT;
		string strPronounciation = "";

		package = major.displayName();
		packageLowcase = major.majorName.ToLower();

		//parse the answer to dictionary object
		JObject dictAnswer = JObject.Parse(word.answers);

		//A word may has many meanings corresponding to many fields (common, it, economic...)
		//The meaning of each field is considered as a package
		var dictPackages = dictAnswer["packages"];
		var dictSinglePackage = dictPackages[packageLowcase];
		    
		if (dictSinglePackage == null) {
			dictSinglePackage = dictPackages[CommonDefine.DEFAULT_SUBJECT];
		    package = "";
		}

		strPronounciation = (string)dictAnswer["pronoun"];
    
		if (strPronounciation.Equals("//") == true) {
	        strPronounciation = "";
	    }

		//"common":{"meaning":"", "explain":"<p>The edge of something is the part of it that is farthest from the center.</p>", "example":"<p>He ran to the edge of the cliff.</p>"}}
		strExplanation = (string)dictSinglePackage["explain"];
		strExample = (string)dictSinglePackage["example"];

		/*maybe must replace "&nbsp;" by "" */
		//remove html tag, use for playing speech
		string plainExplanation = "";
		string plainExample = "";

		if (strExplanation != null) {
			strExplanation = removeNBSP(strExplanation);
			plainExplanation = removeHTML(strExplanation);

		} else {
			strExplanation = "";
		}

		if (strExample != null) {
			strExample = removeNBSP(strExample);
			plainExample = removeHTML(strExample);

		} else {
			strExample = "";
		}

		//meaning
		strMeaning = (string)dictSinglePackage["meaning"];

		//remove <p>, keep <br>
		if (strMeaning != null) {
			strMeaning = strMeaning.Replace("<p>", String.Empty);
			strMeaning = strMeaning.Replace("</p>", String.Empty);

		} else {
			strMeaning = "";
		}

		bool displayMeaningFlag = bool.Parse(Common.loadSettingValueByKey(CommonDefine.SETTINGS_DISPLAY_MEANING_KEY));
		if (displayMeaningFlag == false)
		{
			strMeaning = "";
		}

		float speed = float.Parse(Common.loadSettingValueByKey(CommonDefine.SETTINGS_TTS_SPEED_KEY));

		string strExplainIconTag 	= "";
		string strExampleIconTag 	= "";
		string strWordIconTag 		= "";
		string strNoteTag 			= "";

		//create html
		try {
			strWordIconTag = "<div style='float:right;width:10%%'>" +
				"<a onclick='playText(\"{0}\", {1:0.0});'><img width=100%% src='{2}'/></a>" +
				"</div>";
			strWordIconTag = String.Format(strWordIconTag, word.question, speed, SPEAKER_IMG_LINK);

			if (strExplanation != null && strExplanation.Length > 0) {
				strExplainIconTag = "<div style=\"float:left;width:90%%; font-size:14pt;\">" +
					"   <em>{0}</em> " + //%@ will be replaced by strExplanation
					"</div>" +
					"<div style=\"float:right;width:10%%\"> " +
					"   <p><a onclick='playText(\"{1}\", {2:0.0});'><img width=100%% src='{3}'/></a></p>" +  //%@ will be replaced by strExplanation
					"</div>";
				strExplainIconTag = String.Format(strExplainIconTag, strExplanation, plainExplanation, speed, SPEAKER_IMG_LINK);
			}

			if (strExample != null && strExample.Length > 0) {
				strExampleIconTag = "<div style=\"width:90%%; font-size:12pt;\"><strong>Example: </strong></div>" +
				"<div style=\"float:left;width:90%%; font-size:14pt;\">" +
					"   <em>{0}</em> " + //%@ will be replaced by strExample
					"</div>" +
					"<div style=\"float:right;width:10%%\"> " +
					"   <p><a onclick='playText(\"{1}\", {2:0.0});'><img width=100%% src='{3}'/></a></p>" +  //%@ will be replaced by strExample
					"</div>";
				strExampleIconTag = String.Format(strExampleIconTag, strExample, plainExample, speed, SPEAKER_IMG_LINK);
			}

			string userNote = word.userNote;

			if (userNote != null && userNote.Length > 0) {			
				userNote = userNote.Replace("", "<br>");

				string userNoteLabel = "User note";
				strNoteTag = "<div style=\"width:100%%; font-size:12pt;\"><br><center><hr></center></div>" +
					"<div style=\"width:100%%; font-size:12pt;\"><strong>{0}: </strong></div>" +
					"<div style=\"width:100%%; font-size:14pt;\">" +
					"   <em>{1}</em> " + //%@ will be replaced by word.userNote
					"</div>";

				strNoteTag = String.Format(strNoteTag, userNoteLabel, userNote);
			}

			htmlString = @"<html>" +
			    "<head>" +
				"<meta content=\"width=device-width, initial-scale=1.0, user-scalable=yes\"" +
				"name=\"viewport\">" +
				"<style>" +
				"figure {{" +
				"   text-align: center;" +
				"   margin: auto;" +
				"}}" +
				"figure.image img {{" +
				"   width: 100%% !important;" +
				"   height: auto !important;" +
				"}}" +
				"figcaption {{" +
				"   font-size: 10px;" +
				"}}" +
				"a {{" +
				"   margin-top:10px;" +
				"}}" +
				"hr {{" +
				"border: 0;" +
				"border-top: 3px double #8c8c8c;" +
				"text-align:center;" +
				"}}" +
				"</style>" +
				"<script>" +
				//play the text
				"function playText(content, rate) {{" +
				"   var speaker = new SpeechSynthesisUtterance();" +
				"   speaker.text = content;" +
				"   speaker.lang = 'en-US';" +
				"   speaker.rate = rate;" +//0.1
				"   speaker.pitch = 1.0;" +
				"   speaker.volume = 1.0;" +
				"   speechSynthesis.cancel();" +
				"   speechSynthesis.speak(speaker);" +
				"}}" +
				//cancel speech
				"function cancelSpeech() {{" +
				"   speechSynthesis.pause();" +
				"   speechSynthesis.cancel();" +
				"}}" +
				"</script>" +
				"</head>" +
				"<body>" +
				"   <div style='width:100%%'>" +
				"       <div style='float:left;width:90%%;text-align: center;'>" +
				"           <strong style='font-size:18pt;'> {0} </strong>" +    //%@ will be replaced by word
				"       </div>" +
				"       {1}" +   //%@ will be replaced by strWordIconTag

				"       <div style='width:90%%'>" +
				"           <center><font size='4'> {2} </font></center>" +  //%@ will be replaced by pronunciation
				"       </div>" +

				"           <p style=\"text-align: center;\">  </p>" +  //%@ will be replaced by image link, temporary leave it blank

				"       <div style=\"width:100%%\"></div>" +
				"            {3} " +     //%@ will be replaced by strExplainIconTag

				"            {4} " +    //%@ will be replaced by strExampleIconTag

				"       <div style='width:90%%'>" +
				"           <br><br><br><br><center>{5}<font size='4' color='blue'><em style='margin-left: 10px'> {6} </em></font></center>" +    //%@ will be replaced by meaning
				"       </div>" +
				"   </div>" +
				"   {7} " +     //%@ will be replaced by strNoteTag

				"   </body>" +
				"</html>";

			Debug.WriteLine("createHTMLForQuestion 1 :: " + htmlString);
			htmlString = String.Format(htmlString, word.question, strWordIconTag, strPronounciation, strExplainIconTag, strExampleIconTag, package, strMeaning, strNoteTag);
			Debug.WriteLine("createHTMLForQuestion 2 :: " + htmlString);

		} catch (Exception e) {
			Debug.WriteLine("createHTMLForAnswer :: Exception :; " +e.ToString());
		}

		return htmlString;
	}

	private static string removeHTML(string input) {
		return Regex.Replace(input, "<.*?>", String.Empty);
	}

	private static string removeNBSP(string input) {
		return input.Replace("&nbsp;", " ");
	}
}
