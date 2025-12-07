using System;

[Serializable]
public class SocketMessage
{
    public string type;
    public string role;       // ROLE_ASSIGN
    public string cardId;     // MOVE_CARD, UPDATE_CARD
    public float x;           // MOVE_CARD, UPDATE_CARD
    public float y;           // MOVE_CARD, UPDATE_CARD
    public float width;       // INIT_CANVAS
    public float height;      // INIT_CANVAS
}