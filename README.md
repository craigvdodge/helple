# Helple

Helple is a word list tool to help with solving Wordle puzzles. For help on using the tool, see the help on the webpage itself (Second button from the left at top).

There are other word list type tools online but with some drawbacks and this tool is designed to work more closely with Wordle and automates some reasoning. For instance, using the color coding of the responses to eliminate answers from the word list.

Also, it's an excuse to learn some Blazor Client.

The solution is broken into various projects:

## Projects

### helple

This is the web page itself.

### HelpleDataFile

This is a library that encapsulates reading and writing the actual word list data file. It's used by both helple (to read the data file) and HelpleDataProc (below, to write the file).

### HelpleDataProc

This is a command-line program that reads input data, does some stats on the data, and derives a "score" for each word. It then writes the data (as serialized JSON) into a gziped text file. The web page downloads this text file during its start up.

This program is invoked as part of the build to create content for helple.

#### Note about the scoring algorithm

The scoring algorithm is used to create a number to indicate a good word to use as a guess. It's completely ad hoc, but is considered a good enough balance between solving the puzzle in as few steps as possible and computational complexity.

The algorithm works like this:

1. Read a complete list of words. This also acts as a formatting step (make sure the case is consistent) and a check on the input (make sure all words fit the criteria) and make sure there are no duplicates. During this step, letter frequency is kept track of.

2. The letter frequency is converted into an integer score. So more common letters will have a higher score.

3. Read a second word list into memory, a common word list. While the first list is exhaustive, the second is designed to be "more likely". This will be used below.

4. For each word in the (now in memory) complete word list, add a letter score for each letter while ignoring duplicates. Duplicate letters are ignored to try and maximize the information each guess will give you. If the word appears on the common word list, the score is then given an additional 25% boost.

This algorithm, while not making any claims of rigor, seems to arrive at an answer between 3-4 guesses. This puts it close to other algorithms I've researched, while at the same time being computationally rather simple.

### HelpleSharedWeb

This is a library for code that is shared among different components in the web page (helple). This ended up being just encapsulating reading and writing settings information from local storage.
