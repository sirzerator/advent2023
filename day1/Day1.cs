using System.Text;

class Day1
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

		Part2(path);

		return 0;
	}

	static public void Part1(IEnumerable<String> source)
	{
		int total = 0;

		foreach (var line in source)
		{
			char[] OnlyDigits = Array.FindAll(line.ToCharArray(), Char.IsDigit);

			if (OnlyDigits.Length == 0) continue;

			char firstDigit = OnlyDigits[0];
			char lastDigit = OnlyDigits[OnlyDigits.Length-1];

			String number = new String(new char[] { firstDigit, lastDigit });
			total += Int32.Parse(number);
		}

		Console.WriteLine($"Total is {total}");
	}

	static public void Part2(String path)
	{
		List<String> replacedLines = new List<String>();

		foreach (var line in GetLines(path))
		{
			var digits = new StringBuilder();
			var builder = new StringBuilder();

			foreach (var chr in line.ToCharArray())
			{
				if (Char.IsDigit(chr))
				{
					digits.Append(chr);
					builder.Length = 0;
					continue;
				}

				builder.Append(chr);

				var stringSoFar = builder.ToString();
				if (stringSoFar.EndsWith("one"))
				{
					digits.Append('1');
					builder.Length = 0;
					builder.Append('e');
				}
				else if (stringSoFar.EndsWith("two"))
				{
					digits.Append('2');
					builder.Length = 0;
					builder.Append('o');
				}
				else if (stringSoFar.EndsWith("three"))
				{
					digits.Append('3');
					builder.Length = 0;
					builder.Append('e');
				}
				else if (stringSoFar.EndsWith("four"))
				{
					digits.Append('4');
					builder.Length = 0;
				}
				else if (stringSoFar.EndsWith("five"))
				{
					digits.Append('5');
					builder.Length = 0;
					builder.Append('e');
				}
				else if (stringSoFar.EndsWith("six"))
				{
					digits.Append('6');
					builder.Length = 0;
					builder.Append('x');
				}
				else if (stringSoFar.EndsWith("seven"))
				{
					digits.Append('7');
					builder.Length = 0;
					builder.Append('n');
				}
				else if (stringSoFar.EndsWith("eight"))
				{
					digits.Append('8');
					builder.Length = 0;
					builder.Append('t');
				}
				else if (stringSoFar.EndsWith("nine"))
				{
					digits.Append('9');
					builder.Length = 0;
					builder.Append('e');
				}
			}

			String replaced = digits.ToString();

			replacedLines.Add(replaced);
		}

		Part1(replacedLines);
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
}
