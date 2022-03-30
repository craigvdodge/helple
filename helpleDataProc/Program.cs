// See https://aka.ms/new-console-template for more information
if (args.Length < 2)
{
    Console.WriteLine("Helple Data Processor");
    Console.WriteLine("Usage: helpleDataProc input-file output-file");
    return;
}

string[] words = File.ReadAllLines(args[0]);
// Write to Sorted list to 1.order and 2.deduplicate
SortedList<string, int> outputList = new SortedList<string, int>(words.Length);

// Get some stats
SortedList<char, int> charCount = new SortedList<char, int>(26);
int rejections = 0;
int charsTotal = 0;

foreach (string word in words)
{
    if (word.Length != 5) // Extra filtering
    {
        Console.Error.WriteLine("Word list entry length error found, rejecting: " + word);
        rejections++;
        continue;
    }
    foreach (char c in word)
    {
        if (!charCount.ContainsKey(c))
        {
            charCount[c] = 1;
        }
        else
        {
            charCount[c]++;
        }
        charsTotal++;
    }

    outputList.Add(word.ToUpper(), 0);
}
if (File.Exists(args[1]))
{
    File.Delete(args[1]);
}
File.AppendAllLines(args[1], outputList.Keys);

// report stats
Console.WriteLine("Total final words: " + outputList.Count);
Console.WriteLine("Total rejected words: " + rejections);
Console.WriteLine("Character Counts:");
foreach (char c in charCount.Keys)
{
    Console.WriteLine(c + ": " + charCount[c].ToString());
}
Console.WriteLine("Total characters: " + charsTotal.ToString());
