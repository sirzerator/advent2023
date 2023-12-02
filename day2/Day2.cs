using System.Text.RegularExpressions;

class Day2
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

		foreach (var line in source)
		{
			var game = new Game(line);
			if (game.IsValid(12, 13, 14))
			{
				total += game.Id;
			}
		}

		Console.WriteLine($"Sum of invalid game numbers is {total}");
	}

	static public void Part2(IEnumerable<String> source)
	{
		int total = 0;

		foreach (var line in source)
		{
			var game = new Game(line);
			total += game.GetMaximumReveal().Power();
		}

		Console.WriteLine($"Sum of game powers is {total}");
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

class Game
{
	public static Regex RedRegex = new Regex(@"(\d+) red",
		RegexOptions.Compiled | RegexOptions.IgnoreCase);
	public static Regex GreenRegex = new Regex(@"(\d+) green",
		RegexOptions.Compiled | RegexOptions.IgnoreCase);
	public static Regex BlueRegex = new Regex(@"(\d+) blue",
		  RegexOptions.Compiled | RegexOptions.IgnoreCase);

	public int Id;
	public List<Reveal> Reveals = new List<Reveal>();

	public Game(String line)
	{
		var parts = line.Split(':');
		var gameId = parts[0];

		Regex idRegex = new Regex(@"\d+",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);
		MatchCollection idMatches = idRegex.Matches(gameId);
		Id = Int32.Parse(idMatches.First().Groups[0].ToString());

		var throws = parts[1].Split(';').Select(s => s.Trim());
		foreach (var game in throws)
		{
			int red = 0;
			MatchCollection redMatches = RedRegex.Matches(game);
			if (redMatches.Any()) red = Int32.Parse(redMatches.First().Groups[1].ToString());

			int green = 0;
			MatchCollection greenMatches = GreenRegex.Matches(game);
			if (greenMatches.Any()) green = Int32.Parse(greenMatches.First().Groups[1].ToString());

			int blue = 0;
			MatchCollection blueMatches = BlueRegex.Matches(game);
			if (blueMatches.Any()) blue = Int32.Parse(blueMatches.First().Groups[1].ToString());

			Reveals.Add(new Reveal(red, green, blue));
		}
	}

	public bool IsValid(int red, int green, int blue)
	{
		foreach (var reveal in Reveals)
		{
			if (!reveal.IsValid(red, green, blue)) return false;
		}

		return true;
	}

	public Reveal GetMaximumReveal()
	{
		Reveal max = new Reveal(0, 0, 0);

		foreach (var r in Reveals)
		{
			if (r.Red > max.Red) max.Red = r.Red;
			if (r.Green > max.Green) max.Green = r.Green;
			if (r.Blue > max.Blue) max.Blue = r.Blue;
		}

		return max;
	}
}

class Reveal
{
	public int Red;
	public int Green;
	public int Blue;

	public Reveal(int red, int green, int blue)
	{
		Red = red;
		Green = green;
		Blue = blue;
	}

	public bool IsValid(int red, int green, int blue)
	{
		return red >= Red && green >= Green && blue >= Blue;
	}

	public int Power()
	{
		return Red * Green * Blue;
	}
}
