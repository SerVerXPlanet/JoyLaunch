/*
 * Created by SharpDevelop.
 * User: SerVer
 * Date: 25.11.2020
 * Time: 15:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace JoyLaunch
{
	/// <summary>
	/// Description of Find.
	/// </summary>
	public partial class Input : Form
	{
		public string TextBoxValue
        {
            get { return txtInput.Text; }
            set { txtInput.Text = value; }
        }
		
		
		public Input()
		{
			InitializeComponent();
			
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			this.Icon = (Icon)resources.GetObject("$this.Icon");
		}
		
		
		public Input(string txt) : this()
		{
			TextBoxValue = txt;
		}
		
		
	}
}
