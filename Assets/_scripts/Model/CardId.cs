﻿[System.Serializable]
public struct CardId
{
    public string name;
    public int identifier;

    public CardId(string name, int identifier)
    {
        this.name = name;
        this.identifier = identifier;
    }
}