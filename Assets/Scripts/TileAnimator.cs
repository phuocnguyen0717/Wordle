using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileAnimator : MonoBehaviour
{
    public float flipDuration = 0.5f;

    private Board board;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    public IEnumerator FlipTile(char userGuess, char actualChar)
    {
        for (float i = 0; i <= 90; i += Time.deltaTime * (180 / flipDuration))
        {
            transform.localEulerAngles = new Vector3(0, i, 0);
            yield return null;
        }

        UpdateTileStateAndColor(userGuess, actualChar);

        for (float i = 90; i <= 180; i += Time.deltaTime * (180 / flipDuration))
        {
            transform.localEulerAngles = new Vector3(0, i, 0);
            yield return null;
        }

        transform.localEulerAngles = Vector3.zero;
    }

    private void UpdateTileStateAndColor(char userGuess, char actualChar)
    {
        Tile tile = GetComponent<Tile>();
        if (userGuess == actualChar)
        {
            tile.SetState(board.correctState);
            GetComponent<Image>().color = Color.green;
        }
        else if (board.word.Contains(userGuess.ToString()))
        {
            tile.SetState(board.wrongSpotState);
            GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            tile.SetState(board.incorrectState);
            GetComponent<Image>().color = Color.gray;
        }
    }
}
