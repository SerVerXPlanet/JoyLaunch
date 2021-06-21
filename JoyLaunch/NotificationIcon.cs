/*
 * Создано в SharpDevelop.
 * Пользователь: SerVer
 * Дата: 20.07.2016
 * Время: 18:12
  */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        
        ComponentResourceManager resources = new ComponentResourceManager(typeof(NotificationIcon));
        
        NotifyIcon notifyIcon;
        ContextMenu notificationLeftMenu;
        ContextMenu notificationRightMenu;
        
        Dictionary<string,GameInfo> games = new Dictionary<string,GameInfo>();

        const char separator = ';';
        const int iconSize = 16;

        #endregion Variables




        #region Initialize icon and menu

        public NotificationIcon()
        {
            string settings = Properties.Settings.Default.settings;
            games = DeserializeGameList(settings);
            
            notifyIcon = new NotifyIcon();
            notificationLeftMenu    = new ContextMenu(InitializeLeftMenu());
            notificationRightMenu    = new ContextMenu(InitializeRightMenu());
            
            notifyIcon.ContextMenu = notificationRightMenu;
            
            notifyIcon.MouseClick += new MouseEventHandler(this.IconClick);
            
            notifyIcon.Icon = (Icon)resources.GetObject("Joystick");
        }
        
        
        MenuItem[] InitializeLeftMenu()
        {
            MenuItem[] menu = new MenuItem[games.Count];
            
            int i = 0;
            
            foreach(var game in games)
            {
                MenuItem menuItem;

                if (game.Value.Name == "-")
                {
                    menuItem = new MenuItem("-");
                }
                else
                {
                    menuItem = new MenuItem(game.Value.Name, menuGameClick);
                    menuItem.OwnerDraw = true;
                    menuItem.DrawItem += menu_DrawItem;
                    menuItem.MeasureItem += menu_MeasureItem;
                }

                menu[i] = menuItem;
                i++;
            }
            
            return menu;
        }
        
        
        MenuItem[] InitializeRightMenu()
        {
            var menuSettings = new MenuItem("Settings" , menuSettingsClick);
            menuSettings.OwnerDraw = true;
            menuSettings.DrawItem += menu_DrawItem;
            menuSettings.MeasureItem += menu_MeasureItem;
            
            var menuExit = new MenuItem("Exit" , menuExitClick);
            menuExit.OwnerDraw = true;
            menuExit.DrawItem += menu_DrawItem;
            menuExit.MeasureItem += menu_MeasureItem;
            
            MenuItem[] menu = new MenuItem[] {
                menuSettings,
                new MenuItem("-"),
                menuExit
            };
            
            return menu;
        }
        
        
        void menu_DrawItem(object sender, DrawItemEventArgs e)
        {
            MenuDrawItem(sender, e);
        }
        
        
        void menu_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            MenuMeasureItem(sender, e);
        }


        Image GetIcon(MenuItem menu)
        {
            Image img = (Bitmap)resources.GetObject(menu.Text);

            if (img == null)
            {
                var currentGame = games[menu.Index.ToString()];

                if (File.Exists(currentGame.Logo))
                {
                    FileInfo fi = new FileInfo(currentGame.Logo);
                    string ext = fi.Extension.ToLower();
                    Bitmap bmp = null;

                    if (ext == ".exe")
                    {
                        Icon ic = Icon.ExtractAssociatedIcon(fi.FullName);
                        bmp = ic.ToBitmap();
                    }
                    else if(ext == ".png" || ext == ".jpg" || ext == ".gif" || ext == ".ico" || ext == ".tif" || ext == ".bmp")
                    {
                        try
                        {
                            bmp = new Bitmap(fi.FullName);
                        }
                        catch { }
                    }

                    if(bmp != null)
                    {
                        img = new Bitmap(iconSize, iconSize);

                        using (Graphics gr = Graphics.FromImage(img))
                        {
                            gr.SmoothingMode = SmoothingMode.HighQuality;
                            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            gr.DrawImage((Image)bmp, new Rectangle(0, 0, iconSize, iconSize));
                        }
                    }
                }
            }

            if (img == null)
            {
                img = (Bitmap)resources.GetObject("App");
            }

            return img;
        }
        

        void MenuDrawItem(object obj, DrawItemEventArgs diea)
        {
            var mi = (MenuItem)obj;
            var menuFont = SystemInformation.MenuFont;
            SolidBrush menuBrush = null;
            
            menuBrush = !mi.Enabled ? new SolidBrush(SystemColors.GrayText) : (diea.State & DrawItemState.Selected) != 0 ? new SolidBrush(SystemColors.HighlightText) : new SolidBrush(SystemColors.MenuText);
            
            var strfmt = new StringFormat();

            Image menuImage = GetIcon(mi);

            Rectangle rectImage = diea.Bounds;
            int delta = (diea.Bounds.Height - menuImage.Height) / 2;
            rectImage.X += delta;
            rectImage.Y += delta;
            rectImage.Width = menuImage.Width;
            rectImage.Height = menuImage.Height;

            Rectangle rectText = diea.Bounds;
            rectText.X += rectImage.Width;

            diea.Graphics.FillRectangle((diea.State & DrawItemState.Selected) != 0 ? SystemBrushes.Highlight : SystemBrushes.Menu, diea.Bounds);

            diea.Graphics.DrawImage(menuImage, rectImage);

            diea.Graphics.DrawString(mi.Text, menuFont, menuBrush, diea.Bounds.Left + (int)(1.3 * menuImage.Width), diea.Bounds.Top + ((diea.Bounds.Height - menuFont.Height) / 2), strfmt);
        }
        
        
        void MenuMeasureItem(object obj, MeasureItemEventArgs miea)
        {
            var mi = (MenuItem)obj;
            var menuFont = SystemInformation.MenuFont;
            var strfmt = new StringFormat();
            var sizef = miea.Graphics.MeasureString(mi.Text, menuFont, 300, strfmt);

            Image menuImage = GetIcon(mi);

            miea.ItemWidth = menuImage.Width + (int)Math.Ceiling(sizef.Width);
            miea.ItemHeight = menuImage.Height + 3;//(int)Math.Ceiling(sizef.Height); // 19 px is standard height
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
            
            Properties.Settings.Default.settings = SerializeGameList(games);
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
                psi.Arguments = currentGame.Args;
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
                MessageBox.Show(String.Format("File <{0}> not found", currentGame.Path));
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
            var result = new StringBuilder();
            
            foreach(var game in gameList)
            {
                result.Append(game.Key);
                result.Append(separator);
                result.Append(game.Value.ToString());
                result.AppendLine();
            }
            
            return result.ToString();
        }
        
        
        Dictionary<string,GameInfo> DeserializeGameList(string text)
        {
            var gameList = new Dictionary<string, GameInfo>();
            var whiteSpaces = new char[] {'\r', '\n', '\t'};
            string[] multiLine = text.Trim().Split(whiteSpaces, StringSplitOptions.RemoveEmptyEntries);
            string[] multiStr;
            
            foreach(var str in multiLine)
            {
                multiStr = str.Split(separator);
                gameList.Add(multiStr[0], new GameInfo(multiStr[1], multiStr[2], multiStr[3], multiStr[4]));
            }
            
            return gameList;
        }
        
        #endregion
        
        
    }
    
}
