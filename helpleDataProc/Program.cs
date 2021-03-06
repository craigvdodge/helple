// See https://aka.ms/new-console-template for more information
if (args.Length < 2)
{
    Console.WriteLine("Helple Data Processor");
    Console.WriteLine("Usage: helpleDataProc input-file output-file [common file]");
    return -1;
}

string[]? common = null;
// load common word list
if (args.Length > 2)
{
    Console.WriteLine("Using common file " + args[2]);
    common = File.ReadAllLines(args[2]);
    for (int i = 0; i < common.Length; i++)
    {
        common[i] = common[i].Trim().ToUpper();
    }
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

    outputList.Add(word, 0);
}

// Convert stats to letter score
SortedList<char, int> letterScores = new SortedList<char, int>(26);
foreach (char c in charCount.Keys)
{
    double intermediate = Convert.ToDouble(charCount[c]) / Convert.ToDouble(charsTotal);
    intermediate = intermediate * 10000;
    letterScores.Add(c, Convert.ToInt32(Math.Round(intermediate)));
    Console.WriteLine(c + ": " + letterScores[c]);
}

HelpleDataFile.DataFile dataFile = new HelpleDataFile.DataFile();

foreach (string word in outputList.Keys)
{
    string deDupedString = new string(word.ToCharArray().Distinct().ToArray());
    int score = 0;
    foreach (char c in deDupedString)
    {
        score += letterScores[c];
    }

    if (common != null)
    {
        if (common.Contains(word.ToUpper()))
        {
            int bonus = score / 4;
            score = score + bonus;
        }
    }

    dataFile.AddWord(word, score);
}

dataFile.Write(args[1]);

// report stats
Console.WriteLine("Total final words: " + outputList.Count);
Console.WriteLine("Total characters: " + charsTotal.ToString());
Console.WriteLine("Total rejected words: " + rejections);
Console.WriteLine("Character Counts:");
double percent = 0.0;
double dblTotalChars = Convert.ToDouble(charsTotal);
foreach (char c in charCount.Keys)
{
    percent = Convert.ToDouble(charCount[c]) / dblTotalChars;
    percent = percent * 100;
    percent = Math.Round(percent, 4);
    Console.WriteLine(c + ": " + charCount[c].ToString() + " (" + percent.ToString() + "%)");
}
return 0;