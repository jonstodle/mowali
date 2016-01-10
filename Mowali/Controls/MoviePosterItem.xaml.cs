using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Mowali.Controls
{
	public sealed partial class MoviePosterItem : UserControl
	{
		public MoviePosterItem()
		{
			this.InitializeComponent();
			this.RegisterPropertyChangedCallback(UserControl.WidthProperty, WidthProperty_Changed);
			this.DataContextChanged += MoviePosterItem_DataContextChanged;
		}

		private void WidthProperty_Changed(DependencyObject sender, DependencyProperty dp)
		{
			Height = Width * 1.5;
		}

		private void MoviePosterItem_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			
		}
	}
}
