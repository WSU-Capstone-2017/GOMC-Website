using System;

namespace Project.ConfigInput
{
	public class InConfNameAttribute : Attribute
	{
		public string Name { get; set; }

		public InConfNameAttribute(string name = null)
		{
			Name = name;
		}
	}
}