public class SnippetWork
{
    static List<string []> Snippet(Dictionary<string,List<int>> indexDictionary, string query, Dictionary<string, Dictionary<string,double>> tfidfDictionaries, string[] fileNames, List<List<string>>texts, int filenameIndex)
    {
        List<string []> snippets = new List<string[]>();

        string [] queryDistributed = new string [SuggestionWork.Suggestions(query,tfidfDictionaries).Count];

        for (int i = 0; i < queryDistributed.Length; i++)
        {
            queryDistributed[i] = SuggestionWork.Suggestions(query,tfidfDictionaries)[i];
        }

        double [] queryTFIDFs = new double [queryDistributed.Length];

        for (int i = 0; i < queryTFIDFs.Length; i++)
        {
            queryTFIDFs[i] = QueryWork.GetTFIDF(tfidfDictionaries, QueryWork.QueryDistribution(query)[i], fileNames, filenameIndex);
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
        if(indexDictionary.ContainsKey(queryDistributed[index]))
        {
            foreach (int item in indexDictionary[queryDistributed[index]])
            {
                string[] auxArray = new string[11];
                for (int i = 0; i < 11; i++)
                {
                    auxArray[i] = Take11Words(item,queryDistributed[index],texts,fileNames,filenameIndex)[i];
                }
                snippets.Add(auxArray);
            }
        }

        return snippets;
    }

    static string[] Take11Words(int wordIndex, string word, List<List<string>> texts, string [] fileNames, int filenameIndex)
    {
        string [] elevenWords = new string [11];

        elevenWords[5] = word;

        for (int i = 0; i < 11; i++)
        {
            if(((wordIndex - 5 + i) >= 0) && ((wordIndex - 5 + i) < texts[filenameIndex].Count)) 
            {
                elevenWords[i] = texts[filenameIndex][wordIndex - 5 + i]; 
            }
            else
            {
                elevenWords[i] = "";
            }
        }
        return elevenWords;
    }

    public static string BestSnippet(Dictionary<string,List<int>> indexDictionary, string query, Dictionary<string, Dictionary<string,double>> tfidfDictionaries , string[] fileNames, List<List<string>>texts, int filenameIndex)
    {
        string [] bestSnippet = new string[11];

        string finalSnippet = "...";

        int wordsAmount = 0;

        int arrayIndex = 0;

        int auxIndex = 0;

        int auxAmount = 0;

        foreach (string[] array in Snippet(indexDictionary,query,tfidfDictionaries,fileNames,texts,filenameIndex))
        {
            foreach (string word in array)
            {
                foreach (string item in SuggestionWork.Suggestions(query,tfidfDictionaries))
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

        var stringList = Snippet(indexDictionary,query,tfidfDictionaries,fileNames,texts,filenameIndex)[auxIndex];

        for (int i = 0; i < 10; i++)
        {
            if(stringList[i] != "")
            {
                finalSnippet += stringList[i] + " ";
            }
        }
        finalSnippet += stringList[10] + "...";

        
        return finalSnippet;
    }
}