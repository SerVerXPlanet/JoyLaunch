/*
 * Created by SharpDevelop.
 * User: SerVer
 * Date: 09.06.2021
 * Time: 23:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;


namespace JoyLaunch
{
	/// <summary>
	/// Description of GameInfo.
	/// </summary>
	public struct GameInfo
	{
		public string Logo;
		public string Name;
		public string Path;
		
		
		public GameInfo(string logo, string name, string path)
		{
			Logo = logo;
			Name = name;
			Path = path;
		}
		
		
		public override string ToString()
		{
			return string.Format("{0};{1};{2}", Logo, Name, Path);
		}
		
		
	}
}
