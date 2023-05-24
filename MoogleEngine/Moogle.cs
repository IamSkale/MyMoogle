namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query,DictionaryWork dictionaryWork) 
    {
        List<SearchItem> items = new List<SearchItem>();

        string[] auxName = dictionaryWork.fileNames;

        double [] auxScore = ScoreWork.Score(dictionaryWork.tfidfDictionaries,query,dictionaryWork.fileNames,dictionaryWork.fileDictionaries,dictionaryWork.indexDictionaries,dictionaryWork.texts);

        ScoreWork.InsertionSortDescending(auxScore, auxName);

        if((dictionaryWork.fileNames.Length == 1 && dictionaryWork.fileNames[0] == ".gitignore") || dictionaryWork.fileNames.Length == 0)
        {
            items.Add(new SearchItem("The Content Folder is empty","If you want to make a Search you should fill it with .txt archives", 1));
        }
        else if(query == " " || query == "" || query == null)
        {
            items.Add(new SearchItem("Write something the next time","If the query is empty you won't receive any results",1));
        }
        else
        {
            for (int i = 0; i < dictionaryWork.fileNames.Length; i++)
            {
                string auxSnippet = SnippetWork.BestSnippet(dictionaryWork.indexDictionaries[auxName[i]],query,dictionaryWork.tfidfDictionaries,dictionaryWork.fileNames,dictionaryWork.texts,i);
                
                if(auxScore[i] != 0)
                {
                    items.Add(new SearchItem(auxName[i],auxSnippet,auxScore[i]));
                }
            } 
        }  

        if(items.Count==0)
        {
            items.Add(new SearchItem("I couldn't find anything","Maybe the query was bad written or simply there's nothing like it in the texts, try with something else",1));
        }      
        return new SearchResult(items, SuggestionWork.OnlySuggestions(query,dictionaryWork.tfidfDictionaries));
    }
}