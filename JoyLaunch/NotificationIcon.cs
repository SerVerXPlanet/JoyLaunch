/*
 * Создано в SharpDevelop.
 * Пользователь: SerVer
 * Дата: 20.07.2016
 * Время: 13:12
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Text;


namespace JoyLaunch
{
	public sealed class NotificationIcon
	{
		#region Variables
		
		NotifyIcon notifyIcon;
		ContextMenu notificationLeftMenu;
		ContextMenu notificationRightMenu;
		
		Dictionary<string,GameInfo> games = new Dictionary<string,GameInfo>();
		
		#endregion Variables
		
		
		
		
		#region Initialize icon and menu
		
		public NotificationIcon()
		{
			string settings = Properties.Settings.Default.list_of_games;
			games = DeserializeGameList(settings);
			
			notifyIcon = new NotifyIcon();
			notificationLeftMenu	= new ContextMenu(InitializeLeftMenu());
			notificationRightMenu	= new ContextMenu(InitializeRightMenu());
			
			notifyIcon.ContextMenu = notificationRightMenu;
			
			//notifyIcon.Click += IconClick;
			notifyIcon.MouseClick += new MouseEventHandler(this.IconClick);
			
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			notifyIcon.Icon = (Icon)resources.GetObject("$this.Icon");
			
		}
		
		
		MenuItem[] InitializeLeftMenu()
		{
			MenuItem[] menu = new MenuItem[games.Count];
			
			int i = 0;
			
			foreach(var game in games)
			{
				menu[i] = new MenuItem(game.Value.Name, menuGameClick);
				i++;
			}
			
			return menu;
		}
		
		
		MenuItem[] InitializeRightMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("Settings"	, menuSettingsClick),
				new MenuItem("Exit"		, menuExitClick)
			};
			
			return menu;
		}
		
		#endregion
		
		
		
		
		#region Main - Program entry point
		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			bool isFirstInstance;
			
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex mtx = new Mutex(true, "JoyLaunch", out isFirstInstance))
			{
				if (isFirstInstance)
				{
					NotificationIcon notificationIcon = new NotificationIcon();
					notificationIcon.notifyIcon.Visible = true;
					Application.Run();
					notificationIcon.notifyIcon.Dispose();
				}
				else
				{
					// The application is already running
					// TODO: Display message box or change focus to existing application instance
				}
			} // releases the Mutex
		}
		
		#endregion
		
		
		
		
		#region Event Handlers
		
		void menuSettingsClick(object sender, EventArgs e)
		{
			FormSettings formSettings = new FormSettings(games);
			formSettings.ShowDialog();
			
			notificationLeftMenu = new ContextMenu(InitializeLeftMenu());
			
			Properties.Settings.Default.list_of_games = SerializeGameList(games);
            Properties.Settings.Default.Save();
		}
		
		
		void menuExitClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		
		void menuGameClick(object sender, EventArgs e)
		{
			var currentGame = games[((MenuItem)sender).Index.ToString()];
			
			if(File.Exists(currentGame.Path))
			{
				FileInfo fi = new FileInfo(currentGame.Path);
				
				ProcessStartInfo psi = new ProcessStartInfo();
				psi.WorkingDirectory = fi.DirectoryName;
				psi.FileName = fi.Name;
				try
				{
					Process.Start(psi);
				}
				catch
				{
					MessageBox.Show("An error occurred while starting the application");
				}
			}
			else
				MessageBox.Show("File <" + currentGame.Path + "> not found");
		}
		
		
		void IconClick(object sender, EventArgs e)
		{
			MouseEventArgs me = (MouseEventArgs)e;
						
			if(me.Button == MouseButtons.Left)
			{
				notifyIcon.ContextMenu = notificationLeftMenu;
				
				MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
        		mi.Invoke(notifyIcon, null);
				
        		notifyIcon.ContextMenu = notificationRightMenu;
			}
		}

		#endregion
		
		
		
		
		#region Serialization
		
		string SerializeGameList(Dictionary<string,GameInfo> gameList)
		{
			StringBuilder result = new StringBuilder();
			
			foreach(var game in gameList)
			{
				result.Append(game.Key);
				result.Append(';');
				result.Append(game.Value.ToString());
				result.AppendLine();
			}
			
			return result.ToString();
		}
		
		
		Dictionary<string,GameInfo> DeserializeGameList(string text)
		{
			Dictionary<string,GameInfo> gameList = new Dictionary<string, GameInfo>();
			
			string[] multiLine = text.Split(new char[] {'\r', '\n', '\t'}, StringSplitOptions.RemoveEmptyEntries);
			string[] multiStr;
			
			foreach(var str in multiLine)
			{
				multiStr = str.Split(';');
				gameList.Add(multiStr[0], new GameInfo(multiStr[1], multiStr[2], multiStr[3]));
			}
			
			return gameList;
		}
		
		#endregion
		
		
	}
	
}
