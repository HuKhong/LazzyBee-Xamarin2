using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using LazzyBee.Main;

namespace LazzyBee
{
	public partial class MajorsListPage : ContentPage
	{
		private List<MajorObject> majorObjects = new List<MajorObject>();
		private MajorObject curMajor = null;

		public MajorsListPage()
		{
			InitializeComponent();
			curMajor = Common.loadMajorFromProperties();
			//it
			MajorObject it = new MajorObject();
			it.majorName = "it";
			it.majorThumbnail = "images/majors/it.png";

			if (curMajor != null &&
				curMajor.majorName.Equals(it.majorName))
			{
				it.checkFlag = true;
				it.imgRadioBtn = "images/majors/radiobtn-checked.png";
			}

			majorObjects.Add(it);

			//Science
			MajorObject science = new MajorObject();
			science.majorName = "Science";
			science.majorThumbnail = "images/majors/science.png";

			if (curMajor != null &&
				curMajor.majorName.Equals(science.majorName))
			{
				science.checkFlag = true;
				science.imgRadioBtn = "images/majors/radiobtn-checked.png";
			}
			majorObjects.Add(science);

			//Economic
			MajorObject economic = new MajorObject();
			economic.majorName = "Economic";
			economic.majorThumbnail = "images/majors/economic.png";

			if (curMajor != null &&
				curMajor.majorName.Equals(economic.majorName))
			{
				economic.checkFlag = true;
				economic.imgRadioBtn = "images/majors/radiobtn-checked.png";
			}

			majorObjects.Add(economic);

			//Medicine
			MajorObject medicine = new MajorObject();
			medicine.majorName = "Medicine";
			medicine.majorThumbnail = "images/majors/medicine.png";

			if (curMajor != null &&
				curMajor.majorName.Equals(medicine.majorName))
			{
				medicine.checkFlag = true;
				medicine.imgRadioBtn = "images/majors/radiobtn-checked.png";
			}

			majorObjects.Add(medicine);

			//IELTS
			MajorObject ielts = new MajorObject();
			ielts.majorName = "IELTS";
			ielts.majorThumbnail = "images/majors/ielts.png";

			if (curMajor != null &&
				curMajor.majorName.Equals(ielts.majorName))
			{
				ielts.checkFlag = true;
				ielts.imgRadioBtn = "images/majors/radiobtn-checked.png";
			}

			majorObjects.Add(ielts);

			//TOEIC
			MajorObject toeic = new MajorObject();
			toeic.majorName = "TOEIC";
			toeic.majorThumbnail = "images/majors/toeic.png";

			if (curMajor != null &&
				curMajor.majorName.Equals(toeic.majorName))
			{
				toeic.checkFlag = true;
				toeic.imgRadioBtn = "images/majors/radiobtn-checked.png";
			}

			majorObjects.Add(toeic);

			majorsListView.ItemsSource = majorObjects;
			majorsListView.ItemSelected += OnItemSelected;
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MajorObject;
			if (item != null)
			{

				majorsListView.SelectedItem = null;
				//clear old item
				if (curMajor != null)
				{
					foreach (MajorObject m in majorObjects)
					{
						if (curMajor.majorName.Equals(m.majorName))
						{
							m.checkFlag = false;
							m.imgRadioBtn = "images/majors/radiobtn-unchecked.png";
							break;
						}

					}
				}

				if (curMajor == null)
				{
					foreach (MajorObject m in majorObjects)
					{
						if (item.majorName.Equals(m.majorName))
						{
							item.checkFlag = true;
							item.imgRadioBtn = "images/majors/radiobtn-checked.png";
							break;
						}

					}
					curMajor = item;
				}
				else
				{
					//if tap on another item
					if (!item.majorName.Equals(curMajor.majorName))
					{
						foreach (MajorObject m in majorObjects)
						{
							if (item.majorName.Equals(m.majorName))
							{
								item.checkFlag = true;
								item.imgRadioBtn = "images/majors/radiobtn-checked.png";
								break;
							}

						}
						curMajor = item;
					}
					else
					{
						curMajor = null;
					}
				}

				majorsListView.ItemsSource = null;
				majorsListView.ItemsSource = majorObjects;

				Common.saveMajorToProperties(curMajor);
			}
		}

		void HandleHomeClicked(object sender, System.EventArgs e)
		{
			//throw new NotImplementedException();
			//masterPage.Detail = new NavigationPage((Page)Activator.CreateInstance(HomePage));
			LazzyBeePage lzPage = (LazzyBeePage)App.Current.MainPage;
			lzPage.Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)))
			{
				BarBackgroundColor = CommonDefine.MAIN_COLOR,
				BarTextColor = Color.White
			};
		}
	}
}
