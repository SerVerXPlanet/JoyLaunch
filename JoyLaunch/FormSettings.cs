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
				string[] arr = new string[3];
		        arr[0] = game.Value.Logo;
		        arr[1] = game.Value.Name;
		        arr[2] = game.Value.Path;
		        
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
				
				if (fi.Extension == ".lnk")
				{
					string originalPath = GetFullPathFromLink(fi);
					fi = new FileInfo(originalPath);
				}
			
				string[] arr = new string[3];
				arr[0] = "-";
				arr[1] = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
				arr[2] = fi.FullName;
				
				ListViewItem itm = new ListViewItem(arr);
				
				lbGames.Items.Add(itm);
				
				localGames.Add(localGames.Count.ToString(), new GameInfo(arr[0], arr[1], arr[2]));
			}
		}
		
		
		string GetFullPathFromLink(FileInfo fi)
		{
			var data = WindowsLnkFile.FromFile(fi.FullName);


			/*
			using (var fs = File.Open(fi.FullName, FileMode.Open, FileAccess.Read))
			{
				using (var reader = new BinaryReader(fs))
				{
					fs.Seek(0x14, SeekOrigin.Begin);
					UInt32 flags = reader.ReadUInt32();
					
					if ((flags & 0x01) == 1)
					{
						fs.Seek(0x4C, SeekOrigin.Begin);
						UInt16 idLength = reader.ReadUInt16();
						fs.Seek(idLength, SeekOrigin.Current);
					}
					
					long pos = fs.Position;
					UInt32 len = reader.ReadUInt32();
					fs.Seek(0x0C, SeekOrigin.Current);
					UInt32 offset = reader.ReadUInt32();
					fs.Seek(pos + offset, SeekOrigin.Begin);
					int pathLen = (int)(pos + len - fs.Position - 2);
					string originalPath = new string(reader.ReadChars(pathLen));
					
					return originalPath;
				}
			}
			*/
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
								localGames[lvi.Index.ToString()] = new GameInfo(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text);
							}
						}
					}
				}
			}
		}
		
		
	}
}
