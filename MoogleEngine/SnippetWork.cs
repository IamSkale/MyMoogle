namespace MoogleEngine;
public class SnippetWork
{
    static List<string []> Snippet(Dictionary<string,List<int>> indexDictionary, string query, Dictionary<string, Dictionary<string,double>> tfidfDictionaries, string[] fileNames, Dictionary<string,List<string>>texts, int filenameIndex)
    {
        List<string []> snippets = new List<string[]>();

        var auxSuggestions = SuggestionWork.Suggestions(query,tfidfDictionaries);

        double [] queryTFIDFs = new double [auxSuggestions.Count];

        for (int i = 0; i < queryTFIDFs.Length; i++)
        {
            queryTFIDFs[i] = QueryWork.GetTFIDF(tfidfDictionaries, auxSuggestions[i], fileNames, filenameIndex);
        }

        double max = 0;
        int index = 0;
        for (int i = 0; i < queryTFIDFs.Length; i++)
        {
            if(queryTFIDFs[i]>max)
            {
                index = i;
            }
        }
        if(indexDictionary.ContainsKey(auxSuggestions[index]))
        {
            foreach (int item in indexDictionary[auxSuggestions[index]])
            {
                string[] auxArray = new string[11];
                for (int i = 0; i < 11; i++)
                {
                    auxArray[i] = Take11Words(item,auxSuggestions[index],texts,fileNames,filenameIndex)[i];
                }
                snippets.Add(auxArray);
            }
        }

        return snippets;
    }

    static string[] Take11Words(int wordIndex, string word, Dictionary<string,List<string>> texts, string []fileNames, int filenameIndex)
    {
        string [] elevenWords = new string [11];

        elevenWords[5] = word;

        for (int i = 0; i < 11; i++)
        {
            if(((wordIndex - 5 + i) >= 0) && ((wordIndex - 5 + i) < texts[fileNames[filenameIndex]].Count)) 
            {
                elevenWords[i] = texts[fileNames[filenameIndex]][wordIndex - 5 + i]; 
            }
            else
            {
                elevenWords[i] = "";
            }
        }
        return elevenWords;
    }

    public static string BestSnippet(Dictionary<string,List<int>> indexDictionary, string query, Dictionary<string, Dictionary<string,double>> tfidfDictionaries , string[] fileNames, Dictionary<string,List<string>>texts, int filenameIndex)
    {
        string [] bestSnippet = new string[11];

        string finalSnippet = "";

        int wordsAmount = 0;

        int arrayIndex = 0;

        int auxIndex = 0;

        int auxAmount = 0;

        var snippets = Snippet(indexDictionary,query,tfidfDictionaries,fileNames,texts,filenameIndex);

        var auxSuggestions = SuggestionWork.Suggestions(query,tfidfDictionaries);

        foreach (string[] array in snippets)
        {
            foreach (string word in array)
            {
                foreach (string item in auxSuggestions)
                {
                    if(word == item)
                    {
                        wordsAmount++;
                    }
                }
            }
            if(wordsAmount>auxAmount)
            {
                auxAmount = wordsAmount;
                auxIndex = arrayIndex;
            }
            wordsAmount = 0;
            arrayIndex++;
        }
        
        if(snippets.Count>0)
        {
            var stringList = snippets[auxIndex];

            for (int i = 0; i < 10; i++)
            {
                if(stringList[i] != "")
                {
                    finalSnippet += stringList[i] + " ";
                }
            }
            finalSnippet += stringList[10];
        }
        
        return finalSnippet;
    }
}