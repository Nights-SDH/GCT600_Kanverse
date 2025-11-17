using System.Collections.Generic;

public class SceneTagManager
{
    // public static List<PoolTag> PoolTagDefault { get; } = new List<PoolTag> {
    //     PoolTag.EnemyMiniMapIcon,
    //     PoolTag.IbkkugiThrow,
    //     PoolTag.ItmomiThrow,
    //     PoolTag.MiniMapIconIppali,
    //     PoolTag.MiniMapIconIbkkugi,
    //     PoolTag.MiniMapIconKoppulso,
    //     PoolTag.MiniMapIconGiljjugi,
    //     PoolTag.MiniMapIconDulduli,
    //     PoolTag.MiniMapIconItmomi,
    //     PoolTag.Soul,
    //     PoolTag.Ghost,
    //     PoolTag.Mark,
    //     PoolTag.Tentacle,
    //     PoolTag.TentacleVertical,
    //     PoolTag.TentacleHorizontal,
    //     PoolTag.GurgesThrow,
    //     PoolTag.IppaliEgg
    // };
    private static SceneTagManager _instance;
    public static SceneTagManager Instance => _instance ??= new SceneTagManager();

    // private readonly Dictionary<string, List<PoolTag>> sceneTagRestrictions = new Dictionary<string, List<PoolTag>>();

    // private List<PoolTag> allowedTagsInScene = new List<PoolTag>();

    private SceneTagManager()
    {
        InitializeSceneTagRestrictions();
        UpdateAllowedTags();
    }

    private void InitializeSceneTagRestrictions()
    {
        // sceneTagRestrictions.Add(SceneName.ProtoType.ToString(), new List<PoolTag> { PoolTag.EnemyMiniMapIcon, PoolTag.IbkkugiThrow });
    }

    private void UpdateAllowedTags()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // if (sceneTagRestrictions.ContainsKey(currentScene))
        // {
        //     allowedTagsInScene = sceneTagRestrictions[currentScene];
        // }
        // else
        // {
        //     allowedTagsInScene = PoolTagDefault ?? new List<PoolTag>();
        // }
    }

    // public bool IsTagAllowed(PoolTag tag)
    // {
    //     return allowedTagsInScene.Contains(tag);
    // }
}
