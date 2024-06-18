using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TextMeshProUGUI text;
    public char letter { get; set; }
    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetLetter(char letter)
    {
        this.letter = letter;
        text.text = letter.ToString();
    }
}