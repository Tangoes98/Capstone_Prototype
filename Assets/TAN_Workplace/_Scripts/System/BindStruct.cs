
[System.Serializable]
public struct BindStruct
{
    public string _BindName;
    public int _BindLevel;

    public BindStruct(string name, int level)
    {
        _BindName = name;
        _BindLevel = level;
    }
}
