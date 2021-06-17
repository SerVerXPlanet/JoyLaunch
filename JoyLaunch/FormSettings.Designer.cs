/*
 * Создано в SharpDevelop.
 * Пользователь: SerVer
 * Дата: 01.08.2016
 * Время: 12:20
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
namespace JoyLaunch
{
	partial class FormSettings
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ListView lbGames;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() 
		{
		    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
		    this.lbGames = new System.Windows.Forms.ListView();
		    this.columnHeader1 = new System.Windows.Forms.ColumnHeader("(none)");
		    this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
		    this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
		    this.SuspendLayout();
		    // 
		    // lbGames
		    // 
		    this.lbGames.AllowDrop = true;
		    this.lbGames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
		    this.lbGames.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lbGames.BackgroundImage")));
		    this.lbGames.BackgroundImageTiled = true;
		    this.lbGames.BorderStyle = System.Windows.Forms.BorderStyle.None;
		    this.lbGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
		    this.lbGames.Dock = System.Windows.Forms.DockStyle.Fill;
		    this.lbGames.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
		    this.lbGames.FullRowSelect = true;
		    this.lbGames.GridLines = true;
		    this.lbGames.Location = new System.Drawing.Point(0, 0);
		    this.lbGames.MultiSelect = false;
		    this.lbGames.Name = "lbGames";
		    this.lbGames.Size = new System.Drawing.Size(688, 298);
		    this.lbGames.TabIndex = 0;
		    this.lbGames.UseCompatibleStateImageBehavior = false;
		    this.lbGames.View = System.Windows.Forms.View.Details;
		    this.lbGames.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbGamesDragDrop);
		    this.lbGames.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbGamesDragEnter);
		    this.lbGames.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LbGamesMouseDoubleClick);
		    // 
		    // columnHeader1
		    // 
		    this.columnHeader1.Text = "Logo";
		    this.columnHeader1.Width = 40;
		    // 
		    // columnHeader2
		    // 
		    this.columnHeader2.Text = "Name";
		    this.columnHeader2.Width = 120;
		    // 
		    // columnHeader3
		    // 
		    this.columnHeader3.Text = "Path";
		    this.columnHeader3.Width = 527;
		    // 
		    // FormSettings
		    // 
		    this.AllowDrop = true;
		    this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		    this.ClientSize = new System.Drawing.Size(688, 298);
		    this.Controls.Add(this.lbGames);
		    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
		    this.Name = "FormSettings";
		    this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		    this.Text = "Settings";
		    this.ResumeLayout(false);

		}

	}
}
