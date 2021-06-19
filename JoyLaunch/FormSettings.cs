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

			RefreshListGames();
		}
		
		
		void lbGamesDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
			{
				e.Effect = DragDropEffects.All;
			}
		}


		void RefreshListGames()
        {
			lbGames.Items.Clear();

			foreach (var game in localGames)
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

				var game = new GameInfo(
					(icoPath == String.Empty ? fi.FullName : icoPath),
					fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length),
					fi.FullName,
					args
					);

				localGames.Add(localGames.Count.ToString(), game);
			}

			RefreshListGames();
		}
		
		
		string[] GetFullPathesFromLink(FileInfo fi)
		{
			var data = WindowsLnkFile.FromFile(fi.FullName);

			string appPath = data.RelPath?.Str;

			if (appPath == null)
				appPath = data.Name?.Str;

			if (appPath == null)
				appPath = GetFullPathFromLinkEasyWay(fi);

			appPath = GetInstantFullPath(appPath, fi.FullName);

			string args = data.Arguments?.Str;

			string icoPath = data.IconLocation?.Str;

			icoPath = GetInstantFullPath(icoPath, fi.FullName);

			return new string[] { appPath, args, icoPath };
		}


		string GetInstantFullPath(string path, string originalPath)
        {
			string resultPath = path;

			if (path != null)
			{
				if (resultPath.StartsWith("@"))
					resultPath = resultPath.TrimStart('@');

				if (resultPath.Contains(","))
					resultPath = resultPath.Split(',')[0];

				if (resultPath.StartsWith(".."))
				{
					string[] appParts = resultPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
					string[] origParts = originalPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

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

					resultPath = sb.ToString();
				}

				if (resultPath.Contains("%"))
					resultPath = Environment.ExpandEnvironmentVariables(resultPath);
			}

			return resultPath;
		}


		string GetFullPathFromLinkEasyWay(FileInfo fi)
		{
			using (var fs = File.Open(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
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
		}

		private void lbGames_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				var lvi = ((ListView)(sender)).FocusedItem;

				ListViewHitTestInfo hit = lbGames.HitTest(e.Location);

				if (hit.Item != null)
				{
					int columnindex = hit.Item.SubItems.IndexOf(hit.SubItem);

					if (columnindex == 0 || columnindex == 2) // logo || name
                    {
						var filePath = string.Empty;

						using (OpenFileDialog openFileDialog = new OpenFileDialog())
						{
							if(columnindex == 0)
                            {
								openFileDialog.Filter = "images|*.png;*.jpg;*.gif;*.ico;*.tif;*.bmp";
							}
							else if (columnindex == 2)
							{
								openFileDialog.Filter = "program|*.exe";
							}

							openFileDialog.RestoreDirectory = true;

							if (openFileDialog.ShowDialog() == DialogResult.OK)
							{
								filePath = openFileDialog.FileName;
							}
						}

						if (filePath != string.Empty)
						{
							lvi.SubItems[columnindex].Text = filePath;
							localGames[lvi.Index.ToString()] = new GameInfo(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text);
						}
					}
				}
			}
		}


		void LbGamesMouseDoubleClick(object sender, MouseEventArgs e)
		{
			var lvi = ((ListView)(sender)).FocusedItem;
			
			ListViewHitTestInfo hit = lbGames.HitTest(e.Location);
			    
			if(hit.Item != null)
			{
				int columnindex = hit.Item.SubItems.IndexOf(hit.SubItem);
				//string columnName = lbGames.Columns[columnindex].Text;

				using (Input inputForm = new Input(lvi.SubItems[columnindex].Text))
				{
					inputForm.StartPosition = FormStartPosition.Manual;
					inputForm.Location = new Point(Cursor.Position.X - inputForm.Width / 4, Cursor.Position.Y - inputForm.Height / 4);
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


        private void lbGames_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.Modifiers == Keys.Control)
			{
				if ((e.KeyCode == Keys.Up))
				{
					ExchangeItems(((ListView)(sender)).FocusedItem, -1);
				}
				else if (e.KeyCode == Keys.Down)
				{
					ExchangeItems(((ListView)(sender)).FocusedItem, 1);
				}
				else if(e.KeyCode == Keys.Insert)
				{
					InsertItem(true);
				}
			}
			else if (e.KeyCode == Keys.Delete)
			{
				var lvi = ((ListView)(sender)).FocusedItem;

				if (lvi != null)
				{
					int index = lvi.Index;

					localGames.Remove(index.ToString());
					lvi.Remove();

					for (int i = index + 1; i <= localGames.Count; i++)
					{
						var element = localGames[i.ToString()];
						localGames.Add((i - 1).ToString(), element);
						localGames.Remove(i.ToString());
					}

					if (lbGames.Items.Count > 0)
					{
						int newIndex = Math.Min(index, lbGames.Items.Count - 1);

						lbGames.FocusedItem = lbGames.Items[newIndex];
						lbGames.Items[newIndex].Selected = true;
						lbGames.Items[newIndex].Focused = true;
						lbGames.EnsureVisible(newIndex);
					}
				}
			}
			else if (e.KeyCode == Keys.Insert)
			{
				InsertItem(false);
			}

		}


		void InsertItem(bool isSeparator)
        {
			var game = new GameInfo(
					isSeparator ? String.Empty : "path_to_icon",
					isSeparator ? "-" : "New Program",
					isSeparator ? String.Empty : "path_to_program",
					String.Empty
					);

			localGames.Add(localGames.Count.ToString(), game);

			RefreshListGames();

			int newIndex = lbGames.Items.Count - 1;
			lbGames.FocusedItem = lbGames.Items[newIndex];
			lbGames.Items[newIndex].Selected = true;
			lbGames.Items[newIndex].Focused = true;
			lbGames.EnsureVisible(newIndex);
		}


		void ExchangeItems(ListViewItem currentItem, int direction)
		{
			int currentIndex = currentItem.Index;
			int changeIndex = currentItem.Index + direction;

			if (changeIndex < 0 || changeIndex >= localGames.Count)
				return;

			var currentElement = localGames[currentIndex.ToString()];
			var changeElement = localGames[changeIndex.ToString()];

			localGames[currentIndex.ToString()] = changeElement;
			localGames[changeIndex.ToString()] = currentElement;

			var currentGame = lbGames.Items[currentIndex];
			var changeGame = lbGames.Items[changeIndex];

			lbGames.Items[currentIndex] = CopyItem(changeGame);
			lbGames.Items[changeIndex] = CopyItem(currentGame);

			lbGames.FocusedItem = lbGames.Items[currentIndex];
		}


		ListViewItem CopyItem(ListViewItem item)
        {
			string[] arr = new string[4];

			for (int i = 0; i < 4; i++)
			{
				arr[i] = item.SubItems[i].Text;
			}

			ListViewItem newItem = new ListViewItem(arr);

			return newItem;
		}

        
    }
}
