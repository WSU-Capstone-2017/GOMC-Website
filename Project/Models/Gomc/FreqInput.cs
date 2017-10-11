namespace Project.Models.Gomc
{
	public class FreqInput
	{
		public bool Enabled { get; set; }

		public ulong Value { get; set; }

		public FreqInput()
		{
			
		}

		public FreqInput(bool enabled, ulong value)
		{
			Enabled = enabled;
			Value = value;
		}
	}
}