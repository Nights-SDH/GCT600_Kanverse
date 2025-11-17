using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "Audio/AudioClipList")]
public class SoundList : SingletonScriptableObject<SoundList>
{
    // Chapter BGM
    public AudioClip titleBGM;
    public AudioClip heroBaseBGM;
    public AudioClip barBGM;
    public AudioClip battleBGM;
    public AudioClip cutScenePieceBGM;
    public AudioClip cutSceneViolentBGM;

    // Chapter SFX
    public AudioClip hydeSwingSFX;
    public AudioClip hydeWalkSFK;
    public AudioClip hydeHurtSFK;
}