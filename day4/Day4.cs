using System.Text.RegularExpressions;

class Day4
{
	public static Regex CardRegex = new Regex(@"Card +(?<card>\d+):(?<winning>(\s+\d+)+) \|(?<numbers>(\s+\d+)+)",
		RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
		int score = 0;

		foreach (var line in source)
		{
			score += GetScore(CountWinningNumbers(line));
		}

		Console.WriteLine($"Score is {score}");
	}

	static public void Part2(IEnumerable<String> source)
	{
		int score = 0;

		var results = new Dictionary<int, int>();
		var cardsRemainingToEvaluate = new Queue<int>();

		foreach (var line in source)
		{
			var winningNumbers = CountWinningNumbers(line);
			var cardNumber = GetCardNumber(line);
			results.Add(cardNumber, winningNumbers);
			cardsRemainingToEvaluate.Enqueue(cardNumber);
		}

		while (cardsRemainingToEvaluate.Count > 0)
		{
			score += 1;

			var cardNumber = cardsRemainingToEvaluate.Dequeue();
			int winningNumbers = 0;
			results.TryGetValue(cardNumber, out winningNumbers);

			while (winningNumbers > 0)
			{
				cardNumber += 1;
				cardsRemainingToEvaluate.Enqueue(cardNumber);
				winningNumbers -= 1;
			}
		}

		Console.WriteLine($"Scratchpads count is {score}");
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

	private static int Power(int basis, int exponent)
	{
		int output = 1;
		for (var i = 0; i < exponent; i++)
		{
			output *= basis;
		}
		return output;
	}

	private static int CountWinningNumbers(string line)
	{
		MatchCollection matches = CardRegex.Matches(line);

		List<int> winning = new List<int>();
		List<int> numbers = new List<int>();
		foreach (Match match in matches) {
			var winMatch = match.Groups["winning"];
			foreach (var win in winMatch.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries))
			{
				winning.Add(Int32.Parse(win));
			}

			var numbersMatch = match.Groups["numbers"];
			foreach (var num in numbersMatch.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries))
			{
				numbers.Add(Int32.Parse(num));
			}
		}

		var remaining = winning.Intersect(numbers).ToList();

		return remaining.Count();
	}

	private static int GetScore(int remaining)
	{
		if (remaining == 0) return 0;

		return Power(2, remaining - 1);
	}

	private static int GetCardNumber(string line)
	{
		MatchCollection matches = CardRegex.Matches(line);

		return Int32.Parse(matches.First().Groups["card"].Value);
	}
}
