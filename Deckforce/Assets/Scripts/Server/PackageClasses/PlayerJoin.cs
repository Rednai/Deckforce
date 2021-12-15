using System;

[SerializableAttribute]
public class PlayerJoin
{
    public string id = "";
    public string username = "";
    public int team = 0;
    public bool isClient = false;
}
