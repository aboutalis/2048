using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    //Track all the rows within the grid and also we're going to have track all of the cells too
    //because sometimes is useful to loop through all of the cells in the entire grid and other times
    //it's useful to just loop through the cells within one row so we're just going to have track both
    //the rows and all of the cells 

    public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }

    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            for (int j = 0; j < rows[i].cells.Length; j++)
            {
                rows[i].cells[j].coord = new Vector2Int(j, i);
            }
        }       
    }


    public TileCell RandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        while (cells[index].notempty)
        {
            index++;
            if (index >= cells.Length)
            {
                index = 0;
            }
            if (index == startingIndex)
            {
                return null;
            }
        }
        return cells[index];
    }

    public TileCell GetCell(int x, int y)
    {
        if(x>=0 && x<width && y>=0 && y < height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
        
    }

    public TileCell GetCell(Vector2Int coord)
    {
        return GetCell(coord.x, coord.y);
    }

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coord = cell.coord;
        coord.x += direction.x;
        coord.y -= direction.y;
        return GetCell(coord); 
    }
}
