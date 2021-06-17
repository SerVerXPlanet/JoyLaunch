/*
 * Created by SharpDevelop.
 * User: SerVer
 * Date: 09.06.2021
 * Time: 23:37
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
		public string Args;


		public GameInfo(string logo, string name, string path, string args)
		{
			Logo = logo;
			Name = name;
			Path = path;
			Args = args;
		}
		
		
		public override string ToString()
		{
			return String.Format("{0};{1};{2};{3}", Logo, Name, Path, Args);
		}
		
		
	}
}
