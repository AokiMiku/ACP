namespace GUI_Bases
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Windows.Input;

	public class CustomButton : DockPanel
	{
		private DockPanel container = new DockPanel();
		private Image image = new Image();
		private TextBlock text = new TextBlock();
		private ImagePosition imagePosition = ImagePosition.First;

		public event EventHandler<MouseButtonEventArgs> Click;

		public ImageSource ImageSource
		{
			get { return this.image.Source; }
			set
			{
				this.image.Source = value;
				if (value != null)
				{
					this.image.Visibility = Visibility.Visible;
				}
				else
				{
					this.image.Visibility = Visibility.Collapsed;
				}
			}
		}

		public Visibility ImageVisibility
		{
			get { return this.image.Visibility; }
			set { this.image.Visibility = value; }
		}

		public string Text
		{
			get { return this.text.Text; }
			set
			{
				this.text.Text = value;
				if (!string.IsNullOrEmpty(value))
				{
					this.text.Visibility = Visibility.Visible;
				}
				else
				{
					this.text.Visibility = Visibility.Collapsed;
				}
			}
		}

		public Brush ForeGround
		{
			get { return this.text.Foreground; }
			set { this.text.Foreground = value; }
		}

		public ImagePosition ImagePosition
		{
			get { return this.imagePosition; }
			set
			{
				this.imagePosition = value;
				this.SetImagePosition();
			}
		}

		public CustomButton()
		{
			this.image.Width = 20;
			
			this.container.HorizontalAlignment = HorizontalAlignment.Center;
			this.container.VerticalAlignment = VerticalAlignment.Center;
			this.SetImagePosition();

			this.Children.Add(this.container);

			this.MouseEnter += Container_MouseEnter;
			this.MouseLeave += Container_MouseLeave;
			this.MouseLeftButtonUp += Container_MouseLeftButtonUp;
			this.Loaded += CustomButton_Loaded;
			this.Unloaded += CustomButton_Unloaded;

			this.Background = Layout.ButtonBackground;
			this.ForeGround = Layout.ButtonForeground;
		}

		private void CustomButton_Loaded(object sender, RoutedEventArgs e)
		{
			Layout.Buttons.Add(this);
			this.ImagePosition = this.ImagePosition;
		}

		private void CustomButton_Unloaded(object sender, RoutedEventArgs e)
		{
			Layout.Buttons.Remove(this);
		}

		private void Container_MouseEnter(object sender, MouseEventArgs e)
		{
			Layout.Button_MouseEnter(this);
		}

		private void Container_MouseLeave(object sender, MouseEventArgs e)
		{
			Layout.Button_MouseLeave(this);
		}

		private void Container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.Click?.Invoke(this, e);
		}

		private void SetImagePosition()
		{
			switch (this.ImagePosition)
			{
				case ImagePosition.First:
					if (!string.IsNullOrEmpty(this.Text))
					{
						this.image.Margin = new Thickness(0, 0, 3, 0);
					}
					this.container.Children.Clear();
					this.container.Children.Add(image);
					this.container.Children.Add(text);
					break;
				case ImagePosition.Last:
					if (!string.IsNullOrEmpty(this.Text))
					{
						this.image.Margin = new Thickness(3, 0, 0, 0);
					}
					this.container.Children.Clear();
					this.container.Children.Add(text);
					this.container.Children.Add(image);
					break;
				default:
					break;
			}
		}
	}

	public enum ImagePosition : byte
	{
		First,
		Last
	}
}