using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroGBCreator : MonoBehaviour
{
    public Tile tilePrefab;
    public StateOfTile[] tileStates;

    private TileGrid grid;

    private List<Tile> tiles;

    private int counter = 0;

    private bool waiting;

    private AudioManager audio;

    private Animator animator;

    public int highValue = 0;

    public TMP_FontAsset newFont;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = FindObjectOfType<AudioManager>();
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>();
    }

    public void CreateTiles()
    {
        // Create 1_1 tile
        CreateTile(tileStates[0], 2, grid.cells[0]);

        // Create 1_2 tile
        CreateTile(tileStates[0], 2, grid.cells[1]);

        // Create 1_3 tile
        CreateTile(tileStates[1], 4, grid.cells[2]);

        // Create 1_4 tile
        CreateTile(tileStates[0], 2, grid.cells[3]);

        // Create 2_1 tile
        //CreateTile(tileStates[0], 2, grid.cells[4]);

        // Create 2_2 tile
        CreateTile(tileStates[1], 4, grid.cells[5]);

        // Create 1_3 tile
        CreateTile(tileStates[3], 16, grid.cells[6]);

        // Create 1_4 tile
        CreateTile(tileStates[2], 8, grid.cells[7]);

        // Create 3_1 tile
        CreateTile(tileStates[1], 4, grid.cells[8]);

        // Create 3_2 tile
        CreateTile(tileStates[4], 32, grid.cells[9]);

        // Create 3_3 tile
        CreateTile(tileStates[5], 64, grid.cells[10]);

        // Create 3_4 tile
        CreateTile(tileStates[9], 1024, grid.cells[11]);

        // Create 4_1 tile
        CreateTile(tileStates[3], 8, grid.cells[12]);

        // Create 4_2 tile
        CreateTile(tileStates[9], 1024, grid.cells[13]);

        // Create 4_3 tile
        CreateTile(tileStates[1], 4, grid.cells[14]);

        // Create 4_4 tile
        CreateTile(tileStates[0], 2, grid.cells[15]);
    }

    private void CreateTile(StateOfTile state, int value, TileCell cell)
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);

        tile.SetState(state, value);

        tile.PlayAppearAnimation();
        tile.Spawn(cell);
        tiles.Add(tile);
    }

    
    private void Start()
    {
        CreateTiles();

        InvokeRepeating("FunctionToCall", 6.5f, .2f);
    }

    private void FunctionToCall()
    {
        // Code to be executed every 5 seconds
        if (counter == 0)
        {
            Move(Vector2Int.up, 0, 1, 1, 1);
            CreateTile(tileStates[0], 2, grid.cells[12]);
            counter++;
        }
        else if (counter == 1)
        {
            Move(Vector2Int.left, 1, 1, 0, 1);
            CreateTile(tileStates[0], 2, grid.cells[3]);
            counter++;
        }
        else if (counter == 2)
        {
            Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            CreateTile(tileStates[0], 2, grid.cells[3]);
            counter++;
        }
        else if (counter == 3)
        {
            Move(Vector2Int.left, 1, 1, 0, 1);
            CreateTile(tileStates[0], 2, grid.cells[3]);
            counter++;
        }
        else if (counter == 4)
        {
            Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            CreateTile(tileStates[1], 4, grid.cells[2]);
            counter++;
        }
        else if (counter == 5)
        {
            Move(Vector2Int.left, 1, 1, 0, 1);
            CreateTile(tileStates[0], 2, grid.cells[3]);
            counter++;
        }
        else if (counter == 6)
        {
            Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            CreateTile(tileStates[0], 2, grid.cells[0]);
            counter++;
        }
        else if (counter == 7)
        {
            Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            CreateTile(tileStates[0], 2, grid.cells[2]);
            counter++;
        }
        else if (counter == 8)
        {
            Move(Vector2Int.left, 1, 1, 0, 1);
            CreateTile(tileStates[0], 2, grid.cells[3]);
            counter++;
        }
        else if (counter == 9)
        {
            Move(Vector2Int.left, 1, 1, 0, 1);
            CreateTile(tileStates[0], 2, grid.cells[2]);
            counter++;
        }
        else if (counter == 10)
        {
            Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            CreateTile(tileStates[0], 2, grid.cells[2]);
            counter++;
        }
        else
        {
            animator.Play("ZoomInGameboard");
            //CancelInvoke("FunctionToCall");
        }
    }

    private void Move(Vector2Int direction, int startX, int incrX, int startY, int incrY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < grid.width; x += incrX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.notempty)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
            audio.Play("Move");
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adj = grid.GetAdjacentCell(tile.cell, direction);

        while (adj != null)
        {
            if (adj.notempty)
            {
                if (CanMerge(tile, adj.tile))
                {
                    Merge(tile, adj.tile);
                    return true;
                }
                break;
            }

            newCell = adj;
            adj = grid.GetAdjacentCell(adj, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        int number = b.number * 2;
        
        
        b.SetState(tileStates[index], number);

        FindMax(b);

        b.PlayMergeAnimation();
    }

    public void FindMax(Tile tile)
    {
        if (tile.number > highValue)
        {
            highValue = tile.number;

            if (highValue == 2048)
            {
                tile.GetComponentInChildren<TextMeshProUGUI>().font = newFont;
                tile.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            }
        }
    }

    private int IndexOf(StateOfTile state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }
    }
}
