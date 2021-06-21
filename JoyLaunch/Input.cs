/*
 * Created by SharpDevelop.
 * User: SerVer
 * Date: 25.11.2020
 * Time: 19:59
 */
using System.Drawing;
using System.Windows.Forms;


namespace JoyLaunch
{
	/// <summary>
	/// Description of Input.
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
			
			var resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			this.Icon = (Icon)resources.GetObject("Joystick");
		}
		
		
		public Input(string txt) : this()
		{
			TextBoxValue = txt;
		}
		
		
	}
}
