using System;
using Xamarin.Forms;
namespace LazzyBee
{
	[ContentProperty("Content")]
	public class MultiLineButton : ContentView
	{
		public event EventHandler Clicked;

		protected Grid ContentGrid;
		protected ContentView ContentContainer;
		protected Label TextContainer;

		public String Text
		{
			get
			{
				return (String)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
				OnPropertyChanged();
				RaiseTextChanged();
			}
		}

		public new View Content
		{
			get { return ContentContainer.Content; }
			set
			{
				if (ContentGrid.Children.Contains(value))
					return;

				ContentContainer.Content = value;
			}
		}

		public static BindableProperty TextProperty = BindableProperty.Create(
			propertyName: "Text",
			returnType: typeof(String),
			declaringType: typeof(MultiLineButton),
			defaultValue: null,
			defaultBindingMode: BindingMode.TwoWay,
			propertyChanged: TextValueChanged);

		private static void TextValueChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((MultiLineButton)bindable).TextContainer.Text = (String)newValue;
		}

		public event EventHandler TextChanged;
		private void RaiseTextChanged()
		{
			if (TextChanged != null)
				TextChanged(this, EventArgs.Empty);
		}

		public MultiLineButton()
		{
			ContentGrid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			ContentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

			ContentContainer = new ContentView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			TextContainer = new Label
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			ContentContainer.Content = TextContainer;

			ContentGrid.Children.Add(ContentContainer);

			var button = new Button
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("#01000000"),
				BorderRadius = 4,
				BorderWidth = 10,
				BorderColor = Color.Gray
			};

			button.Clicked += (sender, e) => OnClicked();

			ContentGrid.Children.Add(button);

			base.Content = ContentGrid;

		}

		public void OnClicked()
		{
			if (Clicked != null)
				Clicked(this, new EventArgs());
		}
	}
}
