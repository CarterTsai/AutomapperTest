using System;

namespace Model
{
	public class OuterSource
	{
		public int Value { get; set; }
		public InnerSource Inner { get; set; }
	}

	public class OuterDest
	{
		public int Value { get; set; }
		public InnerDest Inner { get; set; }
	}
}