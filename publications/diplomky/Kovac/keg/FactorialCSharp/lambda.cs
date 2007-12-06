namespace test
{
	public class lambda
	{
		public delegate int function(int x);

		public static void Main(string[] args)
		{
			function b = x => 1 + x;
			var a = b(5);
			System.Console.WriteLine(a);
		}
	}

}
