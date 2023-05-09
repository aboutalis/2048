using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    // Each row simply needs to keep track of all the cells within that row

    public TileCell[] cells { get; private set; }
    public TileCell[] savedCells { get; private set; }
    private void Awake()
    {
        cells = GetComponentsInChildren<TileCell>();
        savedCells = GetComponentsInChildren<TileCell>(); 
    }
}
