
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Suggestion : MonoBehaviour
{
    public TextMeshProUGUI suggestText;
    private List<int> usedIndices = new List<int>();
    private List<char> characters = new List<char>();
    public void RandomCharacter(string word)
    {
        if (usedIndices.Count < word.Length)
        {
            int index;
            do
            {
                index = Random.Range(0, word.Length);
            } while (usedIndices.Contains(index));

            usedIndices.Add(index);
            char character = word[index];

            characters.Add(character);

            suggestText.text = new string(characters.ToArray());
        }
    }
    public void SuggestTextClear()
    {
        usedIndices.Clear();
        characters.Clear();
        suggestText.text = "";
    }
}
