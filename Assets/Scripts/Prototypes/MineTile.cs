using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(InputReceiver))]
public class MineTile : MonoBehaviour
{
    public Tilemap tiles;

    private Tile currentTile;

    private InputReceiver input;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Start()
    {
        if (tiles == null)
        {
            this.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (input.player.GetButtonDown("Fire"))
        {
            Tile t = (Tile)tiles.GetTile(Mathv.Floor(transform.position)); // [TODO] TileToWorldPoint and back
            if (t != null)
            {
                currentTile = t;
            }
        }
    }

    private void GetTargetedTile()
    {

    }

    private void Mine()
    {

    }
}
