namespace MoogleEngine;
public class DictionaryWork
{   
    public Dictionary<string, Dictionary <string, int>> fileDictionaries;

    public Dictionary<string, Dictionary <string, List<int>>> indexDictionaries;

    public Dictionary<string, Dictionary<string, double>> tfidfDictionaries;

    public Dictionary<string,List<string>> texts;

    public string [] fileNames;

    public DictionaryWork()
    {
        string [] files = Directory.GetFiles(@"..\Content");

        fileNames = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            fileNames[i] = Path.GetFileName(files[i]); 
        } 

        fileDictionaries = new Dictionary<string, Dictionary<string, int>>();

        indexDictionaries = new Dictionary<string, Dictionary<string, List<int>>>();

        tfidfDictionaries = new Dictionary<string, Dictionary<string, double>>();

        texts = new Dictionary<string, List<string>>();

        FillDictionaries(fileDictionaries,fileNames,indexDictionaries,texts);
        TfIdf(fileDictionaries,fileNames,tfidfDictionaries);
    }
    
    static void FillDictionaries(Dictionary<string, Dictionary <string, int>> fileDictionaries, string [] fileNames, Dictionary<string, Dictionary <string, List<int>>> indexDictionaries, Dictionary<string,List<string>> texts)
    {
        int i = 0;
        foreach (string fileName in fileNames)
        {
            string path = @"..\Content\" + fileName;

            string text2 = "";

            using (StreamReader oSR = File.OpenText(path))
            {
                string s = "";
                while ((s= oSR.ReadLine()) != null)
                {
                    text2 += s;   
                }
            }
            var fileWordTf = new Dictionary <string, int>();

            var wordIndex = new Dictionary <string, List<int>>();

            texts[fileName] = new List<string>();

            string tempWord = "";

            int j = 0;

            foreach(char c in text2)
            {
                if(Char.IsLetter(c))
                {
                    tempWord += Char.ToLower(c);
                }
                else if ((c == ' ' || Char.IsPunctuation(c)) && tempWord !="")
                {
                    texts[fileName].Add(tempWord);
                    
                    if(fileWordTf.ContainsKey(tempWord))
                    {
                        fileWordTf[tempWord]++;
                        wordIndex[tempWord].Add(j);
                    }
                    else
                    {
                        fileWordTf[tempWord] = 1; 
                        List<int> indexes = new List<int>();
                        indexes.Add(j);
                        wordIndex[tempWord] = indexes;
                    }
                    j++;
                    tempWord = "";
                } 
            }
            fileDictionaries[fileName] = fileWordTf;
            indexDictionaries[fileName] = wordIndex;
            i++;
        }
    }

    static void TfIdf(Dictionary<string, Dictionary <string, int>> fileDictionaries, string [] fileNames, Dictionary<string, Dictionary <string, double>> tfidfDictionaries)
    {
        int docCount = fileNames.Length;

        var documentFrequency = new Dictionary<string, int>();

        foreach (var dict in fileDictionaries.Values)
        {
            foreach (string item in dict.Keys)
            {
                documentFrequency[item] = 0;

                for (int i = 0; i < fileNames.Length; i++)
                {
                    if(fileDictionaries[fileNames[i]].ContainsKey(item))
                    {
                        documentFrequency[item]++;
                    }
                }
            }
        }

        var inverseDocumentFrequency = new Dictionary<string, double>();

        foreach (var term in documentFrequency.Keys)
        {
            inverseDocumentFrequency[term] = Math.Log((double)docCount / documentFrequency[term]);
        }

        foreach (var document in fileDictionaries)
        {
            string documentName = document.Key;

            tfidfDictionaries[documentName] = new Dictionary<string, double>();

            foreach (var term in document.Value)
            {
                double tf = 1 + Math.Log((double)term.Value);

                double idf = inverseDocumentFrequency[term.Key];

                double tfidf = tf * idf;

                tfidfDictionaries[documentName][term.Key] = tfidf;
            }
        }
    }
}