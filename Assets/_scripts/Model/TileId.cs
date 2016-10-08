using UnityEngine;

public struct TileId
{
    public HexId[] hexes;
    public Vector3 position;

    public TileId(HexId[] hexes, Vector3 position)
    {
        this.hexes = hexes;
        this.position = position;
    }

    public void SetTilePosition(Vector3 position)
    {
        this.position = position;
        for (int i = 0; i < hexes.Length; i++)
        {
            hexes[i].SetNewPosition(position);
        }
    }
}