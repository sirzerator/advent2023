class Day6
{
	static bool DEBUG = false;

	static List<Tuple<long, long>> tTimeDistance = new();
	static long RaceTime = 0;
	static long RaceDistance = 0;

	static public int Main(string[] args)
	{
		String? path = args.ElementAtOrDefault<String>(0);

		if (String.IsNullOrEmpty(path))
		{
			path = "input";
		}

		string data = File.ReadAllText(path);
		FillTuples(data);

		Console.WriteLine("=== Part 1 ===");
		Console.WriteLine();

		Part1();

		Console.WriteLine();
		Console.WriteLine("=== Part 2 ===");
		Console.WriteLine();

		Part2();

		return 0;
	}

	static public void Part1()
	{
		long solutions = 1;

		foreach (var x in tTimeDistance)
		{
			var options = SolveRace(x.Item1, x.Item2);
			solutions *= options;
		}

		Console.WriteLine($"Multiplying the solution counts gives {solutions}");
	}

	static public void Part2()
	{
		var solutions = SolveRace(RaceTime, RaceDistance);
		Console.WriteLine($"Ways to win this race {solutions}");
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

	private static void FillTuples(string data)
	{
		var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

		var firstLine = lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries);
		var times = firstLine[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => Int64.Parse(s)).ToArray();
		RaceTime = Int64.Parse(String.Join("", times));

		var secondLine = lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries);
		var distances = secondLine[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => Int64.Parse(s)).ToArray();
		RaceDistance = Int64.Parse(String.Join("", distances));

		for (var i = 0; i < times.Count(); i++) {
			tTimeDistance.Add(new Tuple<long, long>(times[i], distances[i]));
		}
	}

	private static long SolveRace(long time, long distance)
	{
		if (DEBUG) Console.WriteLine($"{time} ms => {distance} mm");

		// (time - speed) * speed == distance + 1 (we must beat the distance, not just equalize)
		// speed * time - speed * speed == distance + 1 (distribute left side)
		// speed * time - speed * speed - (distance + 1) == 0 (move right side)
		// speed * time - speed^2 - (distance + 1) == 0 (simplify speed * speed)
		// -speed * time + speed^2 + (distance + 1) == 0 (change signs)
		// speed^2 - speed * time + (distance + 1) == 0 (this is the equation to solve)
		long dtb = distance + 1; // Distance to beat

		var max = (long) Math.Floor((time + Math.Sqrt(time * time - 4*dtb)) / 2);
		var min = (long) Math.Ceiling((time - Math.Sqrt(time * time - 4*dtb)) / 2);
		if (DEBUG) Console.WriteLine($"{min}, {max} => {max - min + 1} ways");

		return max - min + 1;
	}
}
