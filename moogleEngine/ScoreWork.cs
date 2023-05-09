public class ScoreWork
{   
    public static void InsertionSortDescending(double [] arr, string [] arrII) 
    {
        for (int i = 1; i < arr.Length; i++) 
        {
            int j = i;
            while (j > 0 && arr[j - 1] < arr[j]) 
            {
                double temp = arr[j];
                string tempII = arrII[j];
                arr[j] = arr[j - 1];
                arrII[j] = arrII[j-1];
                arr[j - 1] = temp;
                arrII[j-1] = tempII;
                j--;
            }
        }
    }
    
    public static void Score(Dictionary<string, Dictionary <string, int>> fileDictionaries, string query, string [] fileNames, Dictionary<string, Dictionary <string, double>> tfidfDictionaries, Dictionary<string, Dictionary <string, List<int>>> indexDictionaries, List<List<string>> texts)
    {  
        if(query == "")
        {
            Console.WriteLine("Write something the next time");            
        }
        else
        {
            int auxIndex = 0;
            for (int i = 0; i < QueryWork.QueryDistribution(query).Count; i++)
            { 
                if(!QueryWork.DictContainsWord(SuggestionWork.Suggestions(query,tfidfDictionaries)[i],tfidfDictionaries))
                {
                    auxIndex = i;
                }   
                if(QueryWork.QueryDistribution(query)[i] != SuggestionWork.Suggestions(query,tfidfDictionaries)[i])
                {
                    Console.WriteLine("When you wrote " + QueryWork.QueryDistribution(query)[i] + " maybe you meant " + SuggestionWork.Suggestions(query,tfidfDictionaries)[i]);
                }      
            }
            if(!QueryWork.DictContainsWord(SuggestionWork.Suggestions(query,tfidfDictionaries)[auxIndex],tfidfDictionaries))  
            {
                Console.WriteLine("You should check your Ortography, " + SuggestionWork.Suggestions(query,tfidfDictionaries)[auxIndex] + " doens't look like nothing in the texts"); 
            }
            else
            {
                InsertionSortDescending(QueryWork.QueryCount(fileDictionaries,query, fileNames, tfidfDictionaries,indexDictionaries,texts),fileNames);

                for (int j = 0; j < fileNames.Length; j++)
                {
                    if(QueryWork.QueryCount(fileDictionaries,query, fileNames, tfidfDictionaries,indexDictionaries,texts)[j] != 0)
                    {
                        Console.WriteLine(fileNames[j]);
                        Console.WriteLine(SnippetWork.BestSnippet(indexDictionaries[fileNames[j]],query,tfidfDictionaries,fileNames,texts,j));
                    }   
                }
            }
        }
    }
}