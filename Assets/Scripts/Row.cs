using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] tiles { get; set; }
    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
    }
}
