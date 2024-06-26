using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public Color outlineColor;
        public Color fillColor;
    }
    public char letter { get; private set; }
    public State state { get; private set; }
    [Header("States")]
    public static State emptyState;
    public static State occupiedState;
    public static State correctState;
    public static State wrongSpotState;
    public static State incorrectState;
    private TextMeshProUGUI text;
    public Image fill;
    public Outline outline;
    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        fill = GetComponent<Image>();
        outline = GetComponent<Outline>();
    }
    public void SetLetter(char letter)
    {
        this.letter = letter;
        text.text = letter.ToString();
    }
    public void SetState(State state)
    {
        this.state = state;

        fill.color = state.fillColor;
        outline.effectColor = state.outlineColor;
    }
    public static void InitializeState(State empty, State occupied, State correct, State wrongSpot, State incorrect)
    {
        emptyState = empty;
        occupiedState = occupied;
        correctState = correct;
        wrongSpotState = wrongSpot;
        incorrectState = incorrect;
    }
}
