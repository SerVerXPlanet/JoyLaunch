/*
 * Создано в SharpDevelop.
 * Пользователь: SerVer
 * Дата: 01.08.2016
 * Время: 18:20
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text;
using Kaitai;


namespace JoyLaunch
{
	/// <summary>
	/// Description of FormSettings.
	/// </summary>
	public partial class FormSettings : Form
	{
		Dictionary<string,GameInfo> localGames;
		
		
		public FormSettings(Dictionary<string,GameInfo> games)
		{
			InitializeComponent();
			
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			this.Icon = (Icon)resources.GetObject("Joystick");
			
			localGames = games;
			
			foreach(var game in localGames)
			{
				string[] arr = new string[4];
		        arr[0] = game.Value.Logo;
		        arr[1] = game.Value.Name;
		        arr[2] = game.Value.Path;
				arr[3] = game.Value.Args;

				ListViewItem itm = new ListViewItem(arr);
		        
		        lbGames.Items.Add(itm);
			}
		}
		
		
		void lbGamesDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
			{
				e.Effect = DragDropEffects.All;
			}
		}
		
		
		void lbGamesDragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
 
			foreach (string file in files)
			{
				FileInfo fi = new FileInfo(file);

				string args = String.Empty;
				string icoPath = String.Empty;


				if (fi.Extension == ".lnk")
				{
					string[] originalPathes = GetFullPathesFromLink(fi);

					if (originalPathes[0] != null)
					{
						fi = new FileInfo(originalPathes[0]);
					}

					if (originalPathes[1] != null)
						args = originalPathes[1];

					if (originalPathes[2] != null)
						icoPath = originalPathes[2];
				}

				string[] arr = new string[4];
				arr[0] = (icoPath == String.Empty ? fi.FullName : icoPath);
				arr[1] = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
				arr[2] = fi.FullName;
				arr[3] = args;

				ListViewItem itm = new ListViewItem(arr);
				
				lbGames.Items.Add(itm);
				
				localGames.Add(localGames.Count.ToString(), new GameInfo(arr[0], arr[1], arr[2], arr[3]));
			}
		}
		
		
		string[] GetFullPathesFromLink(FileInfo fi)
		{
			var data = WindowsLnkFile.FromFile(fi.FullName);

			string appPath = data.RelPath?.Str;

			if (appPath == null)
				appPath = data.Name?.Str;

			if (appPath != null)
			{
				if (appPath.StartsWith("@"))
					appPath = appPath.TrimStart('@');

				if (appPath.Contains(","))
					appPath = appPath.Split(',')[0];

				if (appPath.Contains("%"))
					appPath = Environment.ExpandEnvironmentVariables(appPath);

				if (appPath.Contains("..\\"))
				{
					string[] appParts = appPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
					string[] origParts = fi.FullName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

					int level = 0;

					while (appParts[level] == "..")
						level++;

					StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < origParts.Length - level - 1; i++)
                    {
						sb.Append(origParts[i]);
						sb.Append(origParts[i].EndsWith(":") ? "\\\\" : "\\");
					}

                    for (int i = level; i < appParts.Length; i++)
                    {
						sb.Append(appParts[i]);
						sb.Append('\\');
					}

					sb.Remove(sb.Length - 1, 1);

					appPath = sb.ToString();
				}
			}

			string args = data.Arguments?.Str;

			string icoPath = data.IconLocation?.Str;

			if (icoPath != null && icoPath.Contains("%"))
				icoPath = Environment.ExpandEnvironmentVariables(icoPath);

			return new string[] { appPath, args, icoPath };
		}
		
		
		void LbGamesMouseDoubleClick(object sender, MouseEventArgs e)
		{
			var lvi = ((ListView)(sender)).FocusedItem;
			
			ListViewHitTestInfo hit = lbGames.HitTest(e.Location);
			    
			int columnindex = -1;
			//string columnName = "";
		    
			if(hit.Item != null)
			{
				columnindex = hit.Item.SubItems.IndexOf(hit.SubItem);
				//columnName = lbGames.Columns[columnindex].Text;
				
				using(Input inputForm = new Input(lvi.SubItems[columnindex].Text))
				{
					inputForm.StartPosition = FormStartPosition.Manual;
					inputForm.Location = new Point(Cursor.Position.X - inputForm.Width / 4, Cursor.Position.Y - inputForm.Height / 4);
					//inputForm.Location = new Point(Cursor.Position.X - 40, Cursor.Position.Y - 10);
					var rez = inputForm.ShowDialog();
					
					if(rez == DialogResult.OK)
					{
						if(inputForm.TextBoxValue != "")
						{
							string[] multi = inputForm.TextBoxValue.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
							
							if(multi.Length > 0)
							{
								lvi.SubItems[columnindex].Text = multi[0];
								localGames[lvi.Index.ToString()] = new GameInfo(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text);
							}
						}
					}
				}
			}
		}
		
		
	}
}
