using System.Text.RegularExpressions;

class Day5
{
	static bool DEBUG = false;

	static Regex FullRegex = new Regex(@"seeds: (?<seeds>(\d+\s+)+)\nseed-to-soil map:\n(?<seedtosoil>((\d+ ?)+\n)+)\nsoil-to-fertilizer map:\n(?<soiltofertilizer>((\d+ ?)+\n)+)\nfertilizer-to-water map:\n(?<fertilizertowater>((\d+ ?)+\n)+)\nwater-to-light map:\n(?<watertolight>((\d+ ?)+\n)+)\nlight-to-temperature map:\n(?<lighttotemperature>((\d+ ?)+\n)+)\ntemperature-to-humidity map:\n(?<temperaturetohumidity>((\d+ ?)+\n)+)\nhumidity-to-location map:\n(?<humiditytolocation>((\d+ ?)+\n)+)",
		RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

	static List<long> seeds = new();
	static Dictionary<long, long[]> seedToSoil = new();
	static Dictionary<long, long[]> soilToFertilizer = new();
	static Dictionary<long, long[]> fertilizerToWater = new();
	static Dictionary<long, long[]> waterToLight = new();
	static Dictionary<long, long[]> lightToTemperature = new();
	static Dictionary<long, long[]> temperatureToHumidity = new();
	static Dictionary<long, long[]> humidityToLocation = new();

	static public int Main(string[] args)
	{
		String? path = args.ElementAtOrDefault<String>(0);

		if (String.IsNullOrEmpty(path))
		{
			path = "input";
		}

		string data = File.ReadAllText(path);
		FillDataFrom(data);

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
		long lowest = Int64.MaxValue;

		foreach (var seed in seeds)
		{
			var location = GetLocationFromSeed(seed);

			if (lowest > location) lowest = location;
		}

		if (DEBUG) Console.WriteLine();
		Console.WriteLine($"Lowest location is {lowest}");
	}

	static public async void Part2()
	{
		long lowest = Int64.MaxValue;

		var tasks = new Task<long>[seeds.Count() / 2];
		var enumerator = seeds.GetEnumerator();
		int i = 0;
		while (enumerator.MoveNext())
		{
			long start = enumerator.Current;
			enumerator.MoveNext();
			long range = enumerator.Current;

			if (DEBUG) Console.WriteLine($"Queueing {start}, {range}");
			tasks[i] = GetLocationRangeFromSeedAsync(start, range);
			i++;
		}

		Task.WaitAll(tasks);

		foreach (var task in tasks)
		{
			var location = await task;
			if (lowest > location) lowest = location;
		}

		if (DEBUG) Console.WriteLine();
		Console.WriteLine($"Lowest location is {lowest}");
	}

	private static void FillDataFrom(string data)
	{
		MatchCollection matches = FullRegex.Matches(data);
		foreach (Match match in matches) {
			GroupCollection groups = match.Groups;

			foreach (var str in groups["seeds"].Value.Split(" ", StringSplitOptions.RemoveEmptyEntries))
			{
				seeds.Add(Int64.Parse(str));
			}

			StoreRanges(groups["seedtosoil"].Value.Split("\n", StringSplitOptions.RemoveEmptyEntries), seedToSoil);
			StoreRanges(groups["soiltofertilizer"].Value.Split("\n", StringSplitOptions.RemoveEmptyEntries), soilToFertilizer);
			StoreRanges(groups["fertilizertowater"].Value.Split("\n", StringSplitOptions.RemoveEmptyEntries), fertilizerToWater);
			StoreRanges(groups["watertolight"].Value.Split("\n", StringSplitOptions.RemoveEmptyEntries), waterToLight);
			StoreRanges(groups["lighttotemperature"].Value.Split("\n", StringSplitOptions.RemoveEmptyEntries), lightToTemperature);
			StoreRanges(groups["temperaturetohumidity"].Value.Split("\n", StringSplitOptions.RemoveEmptyEntries), temperatureToHumidity);
			StoreRanges(groups["humiditytolocation"].Value.Split("\n", StringSplitOptions.RemoveEmptyEntries), humidityToLocation);
		}
	}

	private static void StoreRanges(string[] lines, Dictionary<long, long[]> target)
	{
		foreach (var line in lines)
		{
			var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			long destination = Int64.Parse(parts[0]);
			long source = Int64.Parse(parts[1]);
			long width = Int64.Parse(parts[2]);

			target[source] = new long[] {  width, destination };
		}
	}

	private static long MapDestination(Dictionary<long, long[]> map, long input)
	{
		foreach (var entry in map)
		{
			long source = entry.Key;
			long width = entry.Value[0];
			long destination = entry.Value[1];

			if (source <= input && input < (source + width))
			{
				return destination + (input - source);
			}
		}

		return input;
	}

	private static long GetLocationFromSeed(long seed)
	{
		if (DEBUG) Console.Write($"{seed} => ");
		long soil = MapDestination(seedToSoil, seed);
		if (DEBUG) Console.Write($"{soil} => ");
		long fertilizer = MapDestination(soilToFertilizer, soil);
		if (DEBUG) Console.Write($"{fertilizer} => ");
		long water = MapDestination(fertilizerToWater, fertilizer);
		if (DEBUG) Console.Write($"{water} => ");
		long light = MapDestination(waterToLight, water);
		if (DEBUG) Console.Write($"{light} => ");
		long temperature = MapDestination(lightToTemperature, light);
		if (DEBUG) Console.Write($"{temperature} => ");
		long humidity = MapDestination(temperatureToHumidity, temperature);
		if (DEBUG) Console.Write($"{humidity} => ");
		long location = MapDestination(humidityToLocation, humidity);
		if (DEBUG) Console.WriteLine($"{location}");

		return location;
	}

	private static long GetLocationFromSeedRange(long start, long range)
	{
		long lowest = Int64.MaxValue;

		for (var seed = start; seed <= (start + range) && seed >= start; seed++)
		{
			var location = GetLocationFromSeed(seed);

			if (lowest > location)
			{
				lowest = location;
			}
		}

		if (DEBUG) Console.WriteLine($"Completed {start}, {range} => {lowest}");

		return lowest;
	}

	private static async Task<long> GetLocationRangeFromSeedAsync(long start, long range)
	{
		return await Task.Run(() => GetLocationFromSeedRange(start, range));
	}
}
