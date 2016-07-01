using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public GameObject m_handPrefab;

    private int m_playerID;

    public void Init(int id)
    {
        m_playerID = id;

        HandManager newHand = Instantiate(m_handPrefab).GetComponent<HandManager>();
        newHand.Init(id);
    }

    public int GetID()
    {
        return m_playerID;
    }
}
