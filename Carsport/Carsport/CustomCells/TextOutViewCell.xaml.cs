﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Carsport.CustomCells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TextOutViewCell : ViewCell
	{
		public TextOutViewCell ()
		{
			InitializeComponent ();
		}
	}
}