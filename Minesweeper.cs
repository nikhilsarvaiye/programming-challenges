namespace Minesweeper
{
	using System;

	public static class Program
	{
		private static int _rows = 3;

		private static int _columns = 5;

		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World");

			var input = new string[,] {
				{ "*", "*", ".", ".", "." },
				{ ".", ".", ".", ".", "." },
				{ ".", "*", ".", ".", "." }
			};

			var sampleInput44 = new string[,] {
				{ "*", ".", ".", "." },
				{ ".", ".", ".", "." },
				{ ".", "*", ".", "." },
				{ ".", ".", ".", "." }
			};

			var output = new string[_rows, _columns];

			for (var i = 0; i < _rows; i++)
			{
				for (var j = 0; j < _columns; j++)
				{
					var currentChar = input[i, j];
					if (currentChar == ".")
					{
						var startCount = GetAllAdjacentStarCount(i, j, input);
						output[i, j] = startCount.ToString();
					}
					else
					{
						output[i, j] = currentChar.ToString();
					}
				}
			}

			for (var i = 0; i < _rows; i++)
			{
				for (var j = 0; j < _columns; j++)
				{
					Console.Write("{0}", output[i, j]);
				}
				Console.WriteLine(string.Empty);
			}
		}
		
		public static int GetAllAdjacentStarCount(int i, int j, string[,] input)
		{
			var startCount = 0;

			var maxRowCount = _rows - 1;
			var maxColumnCount = _rows - 1;

			var mStart = i - 1 < 0 ? 0 : i - 1;
			var nStart = j - 1 < 0 ? 0 : j - 1;

			var mEnd = i + 1 > maxRowCount ? maxRowCount : i + 1;
			var nEnd = j + 1 > maxColumnCount ? maxColumnCount : j + 1;

			for (var m = mStart; m <= mEnd; m++)
			{
				for (var n = nStart; n <= nEnd; n++)
				{
					if (input[m, n] == "*")
					{
						startCount++;
					}
				}
			}

			return startCount;
		}
	}
}
