// See https://aka.ms/new-console-template for more information
Console.WriteLine("Helple Data Processor");
if (args.Length < 2)
{
    Console.WriteLine("Usage: helpleDataProc input-file output-file");
    return;
}

string[] words = File.ReadAllLines(args[0]);

// Write to Sorted list to 1.order and 2.deduplicate
SortedList<string, int> outputList = new SortedList<string, int>(words.Length);

foreach (string word in words)
{
    if (word.Length != 5) // Extra filtering
    {
        continue;
    }
    outputList.Add(word.ToUpper(), 0);
}
if (File.Exists(args[1]))
{
    File.Delete(args[1]);
}
File.AppendAllLines(args[1], outputList.Keys);
