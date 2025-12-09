public enum DialogSpeaker
{
    Narration,             // 나레이션 [cite: 5]
    Kongjwi,               // 콩쥐 [cite: 34]
    Stepmother,            // 새어머니 [cite: 8]
    Patjwi,                // 팥쥐 [cite: 14]
    ExcavatorMole,         // 굴착기 두더지 [cite: 43]
    GlassesSparrow1,       // 안경 쓴 참새1 [cite: 80]
    GlassesSparrow2,       // 안경 쓴 참새2 [cite: 82]
    FlatClam,              // 납작한 조개 [cite: 108]
    OrigamiButterflyFairy, // 종이접기 나비 요정 [cite: 131]
    Magistrate,            // 원님 [cite: 163]
    Official,               // 이방
    Special                // User Interaction Event
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
            case DialogSpeaker.ExcavatorMole:
                return "두더지";
            case DialogSpeaker.GlassesSparrow1:
                return "참새1";
            case DialogSpeaker.GlassesSparrow2:
                return "참새2";
            case DialogSpeaker.FlatClam:
                return "조개";
            case DialogSpeaker.OrigamiButterflyFairy:
                return "나비 요정";
            case DialogSpeaker.Magistrate:
                return "원님";
            case DialogSpeaker.Official:
                return "이방";
            case DialogSpeaker.Special:
                return "플레이어";
            default:
                return "알 수 없음";
        }
    }
}