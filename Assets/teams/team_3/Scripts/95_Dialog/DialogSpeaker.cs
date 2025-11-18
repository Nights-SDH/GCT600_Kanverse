public enum DialogSpeaker
{
    Narration,       // 나레이션
    Stepmother, // 새어머니
    Patjwi,     // 팥쥐
    Kongjwi     // 콩쥐
}

public static class DialogSpeakerExtensions
{
    public static string ToDisplayName(this DialogSpeaker speaker)
    {
        switch (speaker)
        {
            case DialogSpeaker.Narration:
                return "나레이션";
            case DialogSpeaker.Stepmother:
                return "새어머니";
            case DialogSpeaker.Patjwi:
                return "팥쥐";
            case DialogSpeaker.Kongjwi:
                return "콩쥐";
            default:
                return "알 수 없음";
        }
    }
}