using System.Text;

class Day3
{
	static public int Main(string[] args)
	{
		String? path = args.ElementAtOrDefault<String>(0);

		if (String.IsNullOrEmpty(path))
		{
			path = "input";
		}

		Console.WriteLine("=== Part 1 ===");
		Console.WriteLine();

		Part1(GetLines(path));

		Console.WriteLine();
		Console.WriteLine("=== Part 2 ===");
		Console.WriteLine();

		Part2(GetLines(path));

		return 0;
	}

	static public void Part1(IEnumerable<String> source)
	{
		int total = 0;

		char[,] matrix = PrepareMatrix(source);

		int lineCount = source.Count();
		int lineLength = source.First().Length;
		var digits = new StringBuilder();

		int i = 0;
		foreach (var line in source)
		{
			digits.Length = 0;

			var j = 0;
			var active = false;

			while (j < lineLength) {
				char chr = matrix[i,j];
				if (Char.IsDigit(chr))
				{
					digits.Append(chr);

					if (i > 0 && j > 0 && IsSymbol(matrix[i-1,j-1]))
					{
						active = true;
					}
					if (i > 0 && IsSymbol(matrix[i-1,j]))
					{
						active = true;
					}
					if (i > 0 && j < (lineLength - 1) && IsSymbol(matrix[i-1,j+1]))
					{
						active = true;
					}
					if (j > 0 && IsSymbol(matrix[i,j-1]))
					{
						active = true;
					}
					if (j < (lineLength - 1) && IsSymbol(matrix[i,j+1]))
					{
						active = true;
					}
					if (i < (lineCount - 1) && j > 0 && IsSymbol(matrix[i+1,j-1]))
					{
						active = true;
					}
					if (i < (lineCount - 1) && IsSymbol(matrix[i+1,j]))
					{
						active = true;
					}
					if (i < (lineCount - 1) && j < (lineLength - 1) && IsSymbol(matrix[i+1,j+1]))
					{
						active = true;
					}
				}

				if (chr == '.' || IsSymbol(chr) || (digits.Length > 0 && j == lineLength - 1))
				{
					if (active)
					{
						total += Int32.Parse(digits.ToString());
						active = false;
					}
					digits.Length = 0;
				}
				j++;
			}

			i++;
		}

		Console.WriteLine($"Total is {total}");
	}

	static public void Part2(IEnumerable<String> source)
	{
		int total = 0;

		char[,] matrix = PrepareMatrix(source);

		int lineCount = source.Count();
		int lineLength = source.First().Length;

		int i = 0;
		foreach (var line in source)
		{
			int j = 0;
			while (j < lineLength) {
				if (matrix[i,j] == '*')
				{
					int foundNumbers = 0;
					int ratio = 1;
					int? number = null;

					if (Char.IsDigit(matrix[i-1,j]))
					{
						number = ExtractNumberWithin(matrix, i-1, j-2, j+2);
						if (number != null)
						{
							ratio *= (int) number;
							foundNumbers++;
						}
					}
					else
					{
						if (Char.IsDigit(matrix[i-1,j-1]))
						{
							number = ExtractNumberWithin(matrix, i-1, j-3, j);
							if (number != null)
							{
								ratio *= (int) number;
								foundNumbers++;
							}
						}

						if (Char.IsDigit(matrix[i-1,j+1]))
						{
							number = ExtractNumberWithin(matrix, i-1, j+1, j+3);
							if (number != null)
							{
								ratio *= (int) number;
								foundNumbers++;
							}
						}
					}

					if (Char.IsDigit(matrix[i+1,j]))
					{
						number = ExtractNumberWithin(matrix, i+1, j-2, j+2);
						if (number != null)
						{
							ratio *= (int) number;
							foundNumbers++;
						}
					}
					else
					{
						if (Char.IsDigit(matrix[i+1,j-1]))
						{
							number = ExtractNumberWithin(matrix, i+1, j-3, j-1);
							if (number != null)
							{
								ratio *= (int) number;
								foundNumbers++;
							}
						}

						if (Char.IsDigit(matrix[i+1,j+1]))
						{
							number = ExtractNumberWithin(matrix, i+1, j+1, j+3);
							if (number != null)
							{
								ratio *= (int) number;
								foundNumbers++;
							}
						}
					}

					if (Char.IsDigit(matrix[i,j-1]))
					{
						number = ExtractNumberWithin(matrix, i, j-3, j-1);
						if (number != null)
						{
							ratio *= (int) number;
							foundNumbers++;
						}
					}

					if (Char.IsDigit(matrix[i,j+1]))
					{
						number = ExtractNumberWithin(matrix, i, j+1, j+3);
						if (number != null)
						{
							ratio *= (int) number;
							foundNumbers++;
						}
					}

					if (foundNumbers == 2)
					{
						total += ratio;
					}
				}
				j++;
			}

			i++;
		}

		Console.WriteLine($"Total is {total}");
	}

	private static IEnumerable<String> GetLines(String path)
	{
		using (var file = new StreamReader(path))
		{
			String? line;
			while ((line = file.ReadLine()) != null)
			{
				yield return line;
			}
		}
	}

	private static char[,] PrepareMatrix(IEnumerable<String> source)
	{
		char[,] matrix = new char[source.Count(), source.First().Length];

		int i = 0;
		foreach (var line in source)
		{
			char[] chars = line.ToCharArray();
			int j = 0;
			foreach (var chr in chars)
			{
				matrix[i,j] = chr;
				j++;
			}
			i++;
		}

		return matrix;
	}

	private static bool IsSymbol(char chr)
	{
		return !Char.IsDigit(chr) && chr != '.';
	}

	private static int? ExtractNumberWithin(char[,] matrix, int line, int start, int end)
	{
		if (line < 0) return null;
		if (line >= matrix.GetLength(0)) return null;

		StringBuilder digits = new StringBuilder();

		if (start < 0)
		{
			start = 0;
		}
		if (end > (matrix.GetLength(1) - 1))
		{
			end = (matrix.GetLength(1) - 1);
		}

		if (end - start == 0) return null;

		int idx = start;
		while (idx <= end)
		{
			if (Char.IsDigit(matrix[line,idx]))
			{
				digits.Append(matrix[line,idx]);
			}
			else if (digits.Length > 0)
			{
				if (idx >= (end - (end - start)/2))
				{
					return Int32.Parse(digits.ToString());
				}

				// Corner case: keep looking for a number if interrupted before half the range
				digits.Length = 0;
			}
			idx++;
		}

		if (digits.Length == 0) return null;

		return Int32.Parse(digits.ToString());
	}
}
