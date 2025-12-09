using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BackgroundData
{
    public Sprite backgroundSprite;
    public List<DialogName> associatedDialogNames;
}

public class BackgroundChanger: SingletonObject<BackgroundChanger>
{
    public Sprite defaultBackground;
    public Image backgroundRenderer;
    public List<BackgroundData> backgroundDataList;

    public void ChangeBackground(DialogName dialogName)
    {
        backgroundRenderer.sprite = GetBackgroundForDialog(dialogName);
    }

    public Sprite GetBackgroundForDialog(DialogName dialogName)
    {
        foreach (var backgroundData in backgroundDataList)
        {
            if (backgroundData.associatedDialogNames.Contains(dialogName))
            {
                return backgroundData.backgroundSprite;
            }
        }
        return defaultBackground;
    }
}
