using System.Text.Json;

try
{
    if (args.Length != 1) throw new ArgumentException("Pass one JSON input file");
    using var data = JsonDocument.Parse(File.ReadAllText(args[0]));
    Console.WriteLine(JsonSerializer.Serialize(Analyze(data.RootElement)));
    return 0;
}
catch (Exception error) when (error is IOException or JsonException or FormatException
                               or ArgumentException or InvalidOperationException
                               or KeyNotFoundException or OverflowException)
{
    Console.Error.WriteLine(error.Message);
    return 2;
}

static object Analyze(JsonElement input)
{
    decimal stock = input.GetProperty("stock").GetDecimal();
    decimal kerf = input.GetProperty("kerf").GetDecimal();
    var lengths = input.GetProperty("cuts").EnumerateArray().Select(cut => cut.GetDecimal()).OrderDescending().ToArray();
    if (stock <= 0 || kerf < 0 || lengths.Length == 0 || lengths.Any(length => length <= 0 || length > stock))
        throw new ArgumentException("Invalid stock, kerf, or cut lengths");
    var boards = new List<(List<decimal> Cuts, decimal Remaining)>();
    foreach (decimal length in lengths)
    {
        int index = boards.FindIndex(board => board.Remaining >= length + kerf);
        if (index < 0)
        {
            boards.Add((new List<decimal> { length }, stock - length));
            continue;
        }
        var board = boards[index];
        board.Cuts.Add(length);
        boards[index] = (board.Cuts, board.Remaining - length - kerf);
    }
    return new { boards = boards.Select(board => new { cuts = board.Cuts, remaining = board.Remaining }).ToArray() };
}
