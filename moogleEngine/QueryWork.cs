public class QueryWork
{
    public static List<string> QueryDistribution(string query)
    {
            List<string> queryDistributed = new List<string>();
            
            string tempword = "";

            for (int i = 0; i <  query.Length; i++)    
            {
                if(query.Substring(i,1) != " ")
                {
                    tempword += query.Substring(i,1).ToLower();
                }
                if((" " == query.Substring(i,1) && tempword != "") || i==query.Length-1 && tempword != "")
                {
                    queryDistributed.Add(tempword);
                    tempword = "";                   
                }
            }

            return queryDistributed; 
    }

    public static bool DictContainsWord(string query, Dictionary<string, Dictionary <string, double>> tfidfDictionaries)
    {
        bool dictContainsWord = false;

        if(query!=null)
        {
            foreach (var dictionary in tfidfDictionaries.Values)
            { 
                if (dictionary.ContainsKey(query))
                {
                    dictContainsWord=true;
                    break;
                }
            }
        }
        return dictContainsWord;
    }

    public static double GetTFIDF(Dictionary<string,Dictionary<string,double>> tfidfDictionaries, string word, string[]fileNames, int filenameIndex)
    {
        if(tfidfDictionaries[fileNames[filenameIndex]].ContainsKey(word))
        {
            return tfidfDictionaries[fileNames[filenameIndex]][word];
        }
        else
        {
            return 0;
        }
    }
    
    public static double [] QueryCount(Dictionary<string, Dictionary <string, int>> fileDictionaries, string query, string [] fileNames, Dictionary<string, Dictionary <string, double>> tfidfDictionaries, Dictionary<string, Dictionary <string, List<int>>> indexDictionaries, List<List<string>> texts)
    {           
        DictionaryWork.GetDictonaries(fileDictionaries,fileNames, tfidfDictionaries,indexDictionaries,texts);                     
        double [] aux = new double [fileNames.Length];

        for (int i = 0; i < SuggestionWork.Suggestions(query,tfidfDictionaries).Count; i++)
        {
            string word = SuggestionWork.Suggestions(query,tfidfDictionaries)[i];

            for (int j = 0; j < fileNames.Length; j++)
            {
                var auxVar = tfidfDictionaries[fileNames[j]];
                if(word != null && auxVar.ContainsKey(word.ToLower()))
                {
                    aux[j] += auxVar[word.ToLower()];
                }
                else
                {
                    aux[j] = 0;
                }
            }
        }

        return aux; 
    }
}
