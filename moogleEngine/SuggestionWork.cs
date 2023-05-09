public class SuggestionWork
{       
    static int GetLevenshteinDistance(string s, string t, string query)
    {
        if(s != null)
        {
            int m = s.Length;
            int n = t.Length;
            int[,] d = new int[m + 1, n + 1];

            if (m == 0)
            {
                return n;
            }

            if (n == 0)
            {
                return m;
            }

            for (int i = 0; i <= m; i++)
            {
                d[i, 0] = i;
            }

            for (int j = 0; j <= n; j++)
            {
                d[0, j] = j;
            }

            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= m; i++)
                {
                    int cost = 0;
                    if((s[i - 1] == t[j - 1]))
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = 1;
                    }

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[m, n];
        }
        else return -1;
    }

    public static List<string> Suggestions(string query, Dictionary<string, Dictionary <string, double>> tfidfDictionaries)
    {
        List<string> aux = new List<string>();
        
        double threshold = 0.5;
        string suggestedWord = null;
        int smallestDistance = int.MaxValue;

        for (int i = 0; i < QueryWork.QueryDistribution(query).Count; i++)
        {
            if(!QueryWork.DictContainsWord(QueryWork.QueryDistribution(query)[i],tfidfDictionaries))
            {
                foreach (var dictionary in tfidfDictionaries.Values)
                {
                    foreach (string element in dictionary.Keys)
                    {
                        int distance = GetLevenshteinDistance(QueryWork.QueryDistribution(query)[i], element,query);

                        if (distance!= -10 && distance <= threshold * element.Length && distance < smallestDistance)
                        {
                            smallestDistance = distance;
                            suggestedWord = element;
                        }
                    }
                }
                if (suggestedWord != null)
                {
                    aux.Add(suggestedWord);
                }
                else
                {
                    aux.Add(QueryWork.QueryDistribution(query)[i]);
                }
            }
            else
            {
                aux.Add(QueryWork.QueryDistribution(query)[i]);
            }
        } 
        return aux;
    }
}