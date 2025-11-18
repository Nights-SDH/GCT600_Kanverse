using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public bool isPlaying => dialogPanel.activeSelf;
    public static DialogManager Instance { get; private set; }
    public List<DialogName> playedDialogs = new();

    public GameObject dialogPanel;
    public TextMeshProUGUI speakerName1;
    public TextMeshProUGUI speakerName2;
    public TextMeshProUGUI dialogText;
    public Image speakerImage1;
    public Image speakerImage2;

    public List<SpeakerSprite> speakerSprites;
    public Sprite defaultSpeakerSprite;

    private DialogName currentDialogName;
    private (DialogSpeaker, string[])[] dialogSequence;
    private int dialogIndex = 0;
    private int lineIndex = 0;

    public FadeInEffect fadeInEffect;

    private DialogSpeaker? speaker1 = null;
    private DialogSpeaker? speaker2 = null;
    private DialogSpeaker lastSpeaker = DialogSpeaker.Narration;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogPanel.SetActive(true);
    }

    private void Start()
    {
        Initialize();
        StartDialog(DialogName.Kongjwi_Intro_Birth);
    }

    private void Update()
    {
        CommandCheck();
    }

    public void ResetDialogManager()
    {
        playedDialogs.Clear();
        Initialize();
    }

    private void Initialize()
    {
        dialogIndex = 0;
        lineIndex = 0;
        speaker1 = null;
        speaker2 = null;
        lastSpeaker = DialogSpeaker.Narration;
    }

    public void SetActiveDialogPanel(bool isActive)
    {
        dialogPanel.SetActive(isActive);
        if (isActive)
        {
            fadeInEffect.ShowWithFadeIn();
        }
        else
        {
            fadeInEffect.HideWithFadeOut();
        }
    }

    public bool CommandCheck()
    {
        if (!isPlaying) return false;
        else
        {
            if (dialogPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EndDialog();
                }
                if (Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
                {
                    NextLine();
                }
            }

            // if (cutSceneController != null && cutSceneController.isPlaying)
            // {
            //     cutSceneController.CommandCheck();
            // }
            return true;
        }
    }

    // public void SetCutSceneController(CutSceneController controller)
    // {
    //     cutSceneController = controller;
    // }

    public void StartDialog(DialogName dialogName)
    {
        if (playedDialogs.Contains(dialogName))
        {
            DebugConsole.Warning($"[DialogManager] 이미 재생된 대사: {dialogName}");
            return;
        }

        Initialize();
        SetActiveDialogPanel(true);

        playedDialogs.Add(dialogName);
        currentDialogName = dialogName;
        dialogSequence = DialogScript.DialogData[dialogName][Language.kr]; // TODO: 언어 다른데서 가져오도록 수정

        InitializeSpeakers();

        ShowCurrentLine();
    }

    private void InitializeSpeakers()
    {
        // 첫 번째 화자
        if (dialogSequence.Length > 0)
        {
            speaker1 = dialogSequence[0].Item1;
            lastSpeaker = speaker1.Value;
        }

        // 두 번째 화자 찾기 (첫 번째와 다른 사람)
        for (int i = 1; i < dialogSequence.Length; i++)
        {
            var currentSpeaker = dialogSequence[i].Item1;
            if (currentSpeaker != speaker1)
            {
                speaker2 = currentSpeaker;
                break;
            }
        }

        UpdateSpeakerUI();
    }

    private void NextLine()
    {
        lineIndex++;
        var (_, lines) = dialogSequence[dialogIndex];

        if (lineIndex < lines.Length)
        {
            // 같은 화자의 다음 대사
            dialogText.text = lines[lineIndex];
        }
        else
        {
            // 다음 화자로 이동
            dialogIndex++;
            if (dialogIndex < dialogSequence.Length)
            {
                lineIndex = 0;
                ShowCurrentLine();
            }
            else
            {
                TryGetNextDialog();
            }
        }
    }

    private void ShowCurrentLine()
    {
        var (speaker, lines) = dialogSequence[dialogIndex];

        // 화자 업데이트 체크
        UpdateSpeakers(speaker);

        dialogText.text = lines[lineIndex];

        // 말하는 사람 강조
        if (speaker == speaker1)
        {
            speakerImage1.sprite = FindSpeakerSprite(speaker1 ?? DialogSpeaker.Narration);
            speakerImage2.sprite = FindSpeakerDarkSprite(speaker2 ?? DialogSpeaker.Narration);
            dialogText.alignment = TextAlignmentOptions.Left;
        }
        else if (speaker == speaker2)
        {
            speakerImage1.sprite = FindSpeakerDarkSprite(speaker1 ?? DialogSpeaker.Narration);
            speakerImage2.sprite = FindSpeakerSprite(speaker2 ?? DialogSpeaker.Narration);
            dialogText.alignment = TextAlignmentOptions.Right;
        }
    }

    private void TryGetNextDialog()
    {
        if (currentDialogName + 1 == DialogName.End)
        {
            EndDialog();
            return;
        } 
        StartDialog(currentDialogName + 1);
    }

    private void UpdateSpeakers(DialogSpeaker currentSpeaker)
    {
        if (currentSpeaker == speaker1 || currentSpeaker == speaker2)
        {
            lastSpeaker = currentSpeaker;
            return;
        }

        if (lastSpeaker == speaker1)
        {
            speaker2 = currentSpeaker;
        }
        else if (lastSpeaker == speaker2)
        {
            speaker1 = currentSpeaker;
        }
        else
        {
            speaker2 = currentSpeaker;
        }

        lastSpeaker = currentSpeaker;
        UpdateSpeakerUI();
    }

    private void UpdateSpeakerUI()
    {
        if (speaker1.HasValue)
        {
            speakerName1.text = DialogSpeakerExtensions.ToDisplayName(speaker1.Value);
            speakerImage1.sprite = FindSpeakerSprite(speaker1.Value);
            speakerImage1.enabled = true;
        }
        else
        {
            speakerName1.text = "";
            speakerImage1.enabled = false;
        }

        if (speaker2.HasValue)
        {
            speakerName2.text = DialogSpeakerExtensions.ToDisplayName(speaker2.Value);
            speakerImage2.sprite = FindSpeakerSprite(speaker2.Value);
            speakerImage2.enabled = true;
        }
        else
        {
            speakerName2.text = "";
            speakerImage2.enabled = false;
        }
    }

    private void EndDialog()
    {
        speaker1 = null;
        speaker2 = null;
        lastSpeaker = DialogSpeaker.Narration;
    }

    private Sprite FindSpeakerSprite(DialogSpeaker dialogSpeaker)
    {
        foreach (var speakerSprite in speakerSprites)
        {
            if (speakerSprite.speaker == dialogSpeaker)
            {
                return speakerSprite.sprite;
            }
        }
        return defaultSpeakerSprite;
    }

    private Sprite FindSpeakerDarkSprite(DialogSpeaker dialogSpeaker)
    {
        foreach (var speakerSprite in speakerSprites)
        {
            if (speakerSprite.speaker == dialogSpeaker)
            {
                return speakerSprite.darkSprite;
            }
        }
        return defaultSpeakerSprite;
    }
}