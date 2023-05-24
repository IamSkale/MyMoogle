namespace MoogleEngine;
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
    
    public static double[] Score(Dictionary<string,Dictionary<string, double>> tfidfDictionaries, string query, string[] fileNames,Dictionary<string, Dictionary <string, int>> fileDictionaries,Dictionary<string, Dictionary <string, List<int>>> indexDictionaries, Dictionary<string,List<string>> texts)
    {
        double [] score = new double[fileNames.Length];

        double queryNorm = 0;

        var auxQueryTFIDF = QueryWork.QueryTFIDF(tfidfDictionaries,query,fileNames);

        foreach (double item in auxQueryTFIDF.Values)
        {
            queryNorm += Math.Pow(item,2);
        }
        for (int i = 0; i < fileNames.Length; i++)
        {
            double textNorm = 0;
            foreach (var item in auxQueryTFIDF)
            {
                if(tfidfDictionaries[fileNames[i]].ContainsKey(item.Key))
                {
                    score[i] += item.Value * tfidfDictionaries[fileNames[i]][item.Key];
                    textNorm += Math.Pow(tfidfDictionaries[fileNames[i]][item.Key],2);
                }
            }
            if (textNorm!=0)
            {
                score[i] = score[i]/(Math.Sqrt((float)queryNorm)*Math.Sqrt((float)textNorm));
            }
            else
            {
                score[i] = 0;
            }
        }
        return score;
    }
}