using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {

    public enum Terrain
    {
        plains,
        forest,
        hill,
        wasteland,
        desert,
        swamp,
        lake,
        mountain,
        coast
    }

    [SerializeField]
    private GameObject hexGroup;

    void PlaceHexGroup(int groupNumber)
    {
        GameObject group = Instantiate(hexGroup);
        group.GetComponent<HexGroup>().Init(groupNumber);
    }

    void Start()
    {
        PlaceHexGroup(0);
    }
}
