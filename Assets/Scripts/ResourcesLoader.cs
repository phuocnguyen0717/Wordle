using System.Collections;
using UnityEngine;

public class ResourcesLoader : MonoBehaviour
{
    public class WordData
    {
        public string[] solutions;
        public string[] validWords;
    }
    public static IEnumerator LoadDataCoroutine(WordData wordData)
    {
        ResourceRequest requestAll = Resources.LoadAsync<TextAsset>("official_wordle_all");
        yield return requestAll;
        if (requestAll != null)
        {
            TextAsset textFile = requestAll.asset as TextAsset;
            wordData.validWords = textFile.text.Split("\n");
        }
        else
        {
            wordData.validWords = new string[0];
        }

        ResourceRequest requestCommon = Resources.LoadAsync("official_wordle_common");
        yield return requestCommon;

        if (requestCommon != null)
        {
            TextAsset textFile = requestCommon.asset as TextAsset;
            wordData.solutions = textFile.text.Split("\n");
        }
        else
        {
            wordData.solutions = new string[0];
        }
    }
}
