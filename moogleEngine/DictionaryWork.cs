public class DictionaryWork
{   
    static void FillDictionaries(Dictionary<string, Dictionary <string, int>> fileDictionaries, string [] fileNames, Dictionary<string, Dictionary <string, List<int>>> indexDictionaries, List<List<string>> texts)
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

            texts.Add(new List<string>());

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
                    texts[i].Add(tempWord);
                    
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
                double tf = (double)term.Value / document.Value.Values.Sum();

                double idf = inverseDocumentFrequency[term.Key];

                double tfidf = tf * idf;

                tfidfDictionaries[documentName][term.Key] = tfidf;
            }
        }
    }

    static void WriteDictionaries(Dictionary<string, Dictionary <string, int>> fileDictionaries, string [] fileNames, Dictionary<string, Dictionary <string, double>> tfidfDictionaries,Dictionary<string, Dictionary <string, List<int>>> indexDictionaries, List<List<string>> texts)
    {
        FillDictionaries(fileDictionaries,fileNames,indexDictionaries,texts);

        TfIdf(fileDictionaries,fileNames,tfidfDictionaries);

        string dataPath = @".\dataMyFileDictionary.bin";

        using (BinaryWriter writer = new BinaryWriter(File.Open(dataPath, FileMode.Create)))
        {
            writer.Write(fileDictionaries.Count); 
            foreach (KeyValuePair<string, Dictionary<string, int>> item in fileDictionaries)
            {
                writer.Write(item.Key);
                writer.Write(item.Value.Count);
                var auxDic = item.Value;
                foreach(KeyValuePair<string, int> subitem in auxDic)
                {
                    writer.Write(subitem.Key);
                    writer.Write(subitem.Value);
                }
            }     
        }

        string dataPathII = @".\dataMyTfIdfDictionary.bin";

        using (BinaryWriter writer = new BinaryWriter(File.Open(dataPathII, FileMode.Create)))
        {
            writer.Write(tfidfDictionaries.Count); 
            foreach (KeyValuePair<string, Dictionary<string, double>> item in tfidfDictionaries)
            {
                writer.Write(item.Key);
                writer.Write(item.Value.Count);
                var auxDic = item.Value;
                foreach(KeyValuePair<string, double> subitem in auxDic)
                {
                    writer.Write(subitem.Key);
                    writer.Write(subitem.Value);
                }
            }     
        }

        string dataPathIII = @".\dataMyIndexDictionary.bin";

        using (BinaryWriter writer = new BinaryWriter(File.Open(dataPathIII, FileMode.Create)))
        {
            writer.Write(indexDictionaries.Count); 
            foreach (KeyValuePair<string, Dictionary<string, List<int>>> item in indexDictionaries)
            {
                writer.Write(item.Key);
                writer.Write(item.Value.Count);
                var auxDic = item.Value;
                foreach(KeyValuePair<string, List<int>> subitem in auxDic)
                {
                    writer.Write(subitem.Key);
                    writer.Write(subitem.Value.Count);
                    foreach (int index in subitem.Value)
                    {
                        writer.Write(index);
                    }
                }
            }     
        }

        string dataPathIV = @".\dataMyTextsMatrix.bin";

        using(BinaryWriter writer = new BinaryWriter(File.Open(dataPathIV, FileMode.Create)))
        {
            writer.Write(texts.Count);
            for (int i = 0; i < texts.Count; i++)
            {
                writer.Write(texts[i].Count);
                for (int j = 0; j < texts[i].Count; j++)
                {
                    writer.Write(texts[i][j]);
                }
            }
        }
    }

    static void WriteMatrix(List<List<string>>texts, string [] fileNames)
    {
        
    }

    static void ReadDictionaries(Dictionary<string, Dictionary <string, int>> fileDictionaries,Dictionary<string, Dictionary <string, double>> tfidfDictionaries, Dictionary<string,Dictionary<string,List<int>>> indexDictionaries,List<List<string>>texts)
    {
        
        string dataPath = @".\dataMyFileDictionary.bin";

        using (BinaryReader reader = new BinaryReader(File.Open(dataPath, FileMode.Open)))
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                Dictionary<string, int> auxDictionary = new Dictionary<string, int>();
                int subCount = reader.ReadInt32();
                for (int j = 0; j < subCount; j++)
                {
                    string subKey = reader.ReadString();
                    int subValue = reader.ReadInt32();
                    auxDictionary.Add(subKey,subValue);
                }
                if(!fileDictionaries.ContainsKey(key))
                {
                    fileDictionaries.Add(key,auxDictionary);
                }
            }
        }
        
        string dataPathII = @".\dataMyTfIdfDictionary.bin";

        using (BinaryReader reader = new BinaryReader(File.Open(dataPathII, FileMode.Open)))
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                Dictionary<string, double> auxDictionary = new Dictionary<string, double>();
                int subCount = reader.ReadInt32();
                for (int j = 0; j < subCount; j++)
                {
                    string subKey = reader.ReadString();
                    double subValue = reader.ReadDouble();
                    auxDictionary.Add(subKey,subValue);
                }
                if(!tfidfDictionaries.ContainsKey(key))
                {
                    tfidfDictionaries.Add(key, auxDictionary);
                }
            }
        }

        string dataPathIII = @".\dataMyIndexDictionary.bin";

        using (BinaryReader reader = new BinaryReader(File.Open(dataPathIII, FileMode.Open)))
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                Dictionary<string, List<int>> auxDictionary = new Dictionary<string, List<int>>();
                int subCount = reader.ReadInt32();
                for (int j = 0; j < subCount; j++)
                {
                    string subKey = reader.ReadString();
                    int subsubCount = reader.ReadInt32();
                    List<int> indexes = new List<int>();
                    for (int k = 0; k < subsubCount; k++)
                    {
                        indexes.Add(reader.ReadInt32());
                    }
                    auxDictionary.Add(subKey,indexes);
                }
                if(!indexDictionaries.ContainsKey(key))
                {
                    indexDictionaries.Add(key,auxDictionary);
                }
            }
        }

        string dataPathIV = @".\dataMyTextsMatrix.bin";

        using(BinaryReader reader = new BinaryReader(File.Open(dataPathIV,FileMode.Open)))
        {
            int rowCount = reader.ReadInt32(); 

            for (int i = 0; i < rowCount; i++)
            {
                int colCount = reader.ReadInt32();

                List<string> newRow = new List<string>(colCount);

                for (int j = 0; j < colCount; j++)
                {
                    newRow.Add(reader.ReadString());
                }

                texts.Add(newRow);
            }
        }
    }

    public static void GetDictonaries(Dictionary<string, Dictionary <string, int>> fileDictionaries, string [] fileNames, Dictionary<string, Dictionary <string, double>> tfidfDictionaries,Dictionary<string, Dictionary <string, List<int>>> indexDictionaries, List<List<string>> texts)
    {   
        string dataPath = @".\dataMyFileDictionary.bin";

        string dataPathII = @".\dataMyTfIdfDictionary.bin";

        string dataPathIII = @".\dataMyIndexDictionary.bin";

        string dataPathIV = @".\dataMyTextsMatrix.bin";

        if(!File.Exists(dataPath) || !File.Exists(dataPathII) || !File.Exists(dataPathIII) || !File.Exists(dataPathIV))
        {
            WriteDictionaries(fileDictionaries,fileNames, tfidfDictionaries,indexDictionaries,texts);
        }
        else
        {
            ReadDictionaries(fileDictionaries, tfidfDictionaries, indexDictionaries,texts);
            
            if(fileDictionaries.Count != fileNames.Length)
            {
                fileDictionaries.Clear();
                tfidfDictionaries.Clear();
                WriteDictionaries(fileDictionaries,fileNames, tfidfDictionaries,indexDictionaries,texts);
            }
            else
            {
                foreach (string fileName in fileNames)
                {
                    if(!fileDictionaries.ContainsKey(fileName))
                    {
                        fileDictionaries.Clear();
                        tfidfDictionaries.Clear();
                        WriteDictionaries(fileDictionaries,fileNames, tfidfDictionaries,indexDictionaries,texts);
                        break;
                    }
                }
            } 
        }
    }
}