using UnityEngine;

public class GameManagerUX: SingletonObject<GameManagerUX>
{
    [Header("User Test Infos")]
    public bool isHost;
    public int moveCount = 0;
    
    [Header("User Test Settings")]
    public CardSetType selectedCardSetType;
    public CanvasSizeType selectedCanvasSizeType;
    public float intervalCardAndCanvas = -0.01f;
}
