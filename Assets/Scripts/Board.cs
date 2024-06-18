using UnityEngine;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]{
        KeyCode.A,KeyCode.B,KeyCode.C,KeyCode.D,KeyCode.E,KeyCode.F,KeyCode.G,
        KeyCode.H,KeyCode.I,KeyCode.J,KeyCode.K,KeyCode.L,KeyCode.M,KeyCode.N,
        KeyCode.O,KeyCode.P,KeyCode.Q,KeyCode.R,KeyCode.S,KeyCode.X,KeyCode.Y,
        KeyCode.Z,
    };
    private Row[] rows;
    private int rowIndex;
    private int columnIndex;
    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }
    void Update()
    {
        Row currentRow = rows[rowIndex];
        if (Input.GetKey(KeyCode.Backspace))
        {
            columnIndex = Mathf.Max(columnIndex - 1, 0);
            currentRow.tiles[columnIndex].SetLetter('\0');
        }
        else if (columnIndex >= currentRow.tiles.Length)
        {

        }
        else
        {
            for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                {
                    currentRow.tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[i]);
                    columnIndex++;
                    break;
                }
            }
        }
    }
}
