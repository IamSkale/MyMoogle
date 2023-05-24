namespace MoogleEngine;
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

    public static Dictionary<string, double> QueryTFIDF (Dictionary<string,Dictionary<string, double>> tfidfDictionaries, string query, string[] fileNames)
    {
        var queryTFIDF = new Dictionary<string,double>();

        var auxSuggestions = SuggestionWork.Suggestions(query,tfidfDictionaries);

        foreach (string item in auxSuggestions)
        {
            int queryCount = 1;
            int docCount = 0;
            double documents = fileNames.Length;
            foreach (var dict in tfidfDictionaries.Values)
            {
                if(dict.ContainsKey(item))
                {
                    docCount++;
                }
            }
            if(docCount==0)
            {
                queryTFIDF[item] = 0;
            }
            else if(!queryTFIDF.ContainsKey(item))
            {
                queryTFIDF[item] = (1 + Math.Log(1)) * (Math.Log(documents/docCount));
            }
            else
            {
                queryCount++;
                queryTFIDF[item] = (1 + Math.Log(queryCount)) * (Math.Log(documents/docCount));
            }
        }
        return queryTFIDF;
    }
}
