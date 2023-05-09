using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Of Tile")]
public class StateOfTile : ScriptableObject
{
    //Define the data structure for a tile state. The State being what is the color of our tile like what's the background color of te tile
    //and what's the text color of the tile .Every number has a different kind of set of colors and we're 
    //going to define all in this script.

    public Color textColor;
    public Color backgroundColor;


}
