# Pog Counter using Youtube's Automatic English Transcript
Program that ingests Youtube's automatic English transcript and analyzes how many times the word "pog" is picked up by the AI. Mainly used to calculate how many times "POG", "POGGING" and its many iterations are said in [Luca Kaneshiro's](https://www.youtube.com/channel/UC7Gb7Uawe20QyFibhLl1lzA) streams. The complete list of streams analyzed will be included later on in this file. 

## Results
TLDR or IDC? Full raw results are found at [PogResults.md](https://github.com/riringo/lucakaneshiro_pogcounter/blob/main/PogCounter/PogCounter/PogResults.md). Infographic link coming soon. 

## "Cleaning" Transcripts
Using the extracted transcripts from Youtube, the program cleans the files through the following:
1. Replaces all `\t`, `\r`, and `\n` with spaces
2. Split transcript text by the following delimiters: `<' ', ',', '.', ':', '?', '!', '-'>`
3. In instances where words are spelled out (i.e., a word has only one letter) it will combine subsequent single letters into a single word, unless it is "p"
 > This is to account for instances where Luca Kaneshiro spells out "POG" or "POGGGING" multiple times in a row
4. Timestamps and cue words (i.e., [Music], [Laughter] ) are removed

## Accounting for Accent
After analysis of a random sample, determined that Youtube's AI sucks. Sometimes the words "POG" and "POGGING" are captured incorrectly. The following are common occurances of such scenarios and thus, after considerations, will be included in the POG Counter:
1. Pug <> Pog
2. Parking <> Pogging
3. Pok <> Pog
4. Plug <> Pog
5. Pong <> Pog

If you are running this locally and want to exclude, feel free to remove from `Globals.Outliers` list. List can also be updated in [Outliers](https://github.com/riringo/lucakaneshiro_pogcounter/blob/f5ed7b26ada39a4d3ebe6518dc31740f6a962e37/PogCounter/PogCounter/Models/Globals.cs#L15). 

## Calculations
Words are counted for the Pog Counter if they meet one of the following criteria:
1. Contains the word "Pog"
2. Contains the characters `{"P", "O", "G"}` in the correct order (i.e., "POOOGGGG" would be counted)
3. Part of the `Globals.Outliers` list. Check "Accounting for Accent" section for more details

## Results
Results are outputed in [PogResults.md](https://github.com/riringo/lucakaneshiro_pogcounter/blob/main/PogCounter/PogCounter/PogResults.md). Individual results for each transcript is located in the [Results Folder](https://github.com/riringo/lucakaneshiro_pogcounter/tree/main/PogCounter/PogCounter/Results).

## Deploy to Your Local
If you want to add in your own transcripts or modify the code. You are free to do so. To run it locally, just clone the branch locally and run. To add in new transcripts, add the transcript to [Transcripts Folder](https://github.com/riringo/lucakaneshiro_pogcounter/tree/main/PogCounter/PogCounter/Transcripts) and update the [Luca Kaneshiro Stream csv](https://github.com/riringo/lucakaneshiro_pogcounter/blob/main/PogCounter/PogCounter/Transcripts/LucaKaneshiroStreams.csv) with the following:
* textFile (name of the transcript file)
* runtimeInMinutes (length of stream in minutes)
* link (link to the Youtube stream)
* videoName (name of the video)

## Finding something other than POG
You can update [Globals file] (https://github.com/riringo/lucakaneshiro_pogcounter/blob/f5ed7b26ada39a4d3ebe6518dc31740f6a962e37/PogCounter/PogCounter/Models/Globals.cs#L11) with an array of characters of anything else. 

Alternatively, you can follow the sample for the "Love" in the [Analyzer file] (https://github.com/riringo/lucakaneshiro_pogcounter/blob/f5ed7b26ada39a4d3ebe6518dc31740f6a962e37/PogCounter/PogCounter/Helper/Analyzer.cs#L77) to determine how to add in your own keyword finder. 

## Updates
The repo owner is lazy so this will probably be updated once a week with new transcripts and stream information (yes, this is done manually because I'm too lazy to mess with Youtube API). 

Last Update can be found at the top of the [PogResults.md](https://github.com/riringo/lucakaneshiro_pogcounter/blob/main/PogCounter/PogCounter/PogResults.md). 


Got questions, dm me on twitter @dame_riringo
