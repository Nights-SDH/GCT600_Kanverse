using System.Collections.Generic;

public class DialogScript
{
    public static Dictionary<DialogName, Dictionary<Language, (DialogSpeaker, string[])[]>> DialogData = new()
    {
        // ==================================================================
        // --- 신규 대화 데이터 (콩쥐팥쥐) ---
        // ==================================================================

        // 1. 콩쥐의 탄생
        { DialogName.Kongjwi_Intro_Birth, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "옛날 어느 마을에 사이좋은 부부가 살았어요.",
                        "이 부부에겐 오랫동안 아이가 없었지만\n정성껏 기도 끝에 예쁜 딸을 얻게 되었죠.",
                        "부부는 어여쁜 콩처럼 올곧게 자랐으면\n좋겠다는 의미에서 딸의 이름을 콩쥐 라고 지었습니다."
                    })
                }
            },
            {
                Language.en, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "Once upon a time, in a certain village, lived a happy couple.",
                        "They had no child for a long time, but after praying sincerely,\nthey were blessed with a beautiful daughter.",
                        "The couple named their daughter Kongjwi, hoping she would grow up\nupright and beautiful like a pretty bean."
                    })
                }
            }
        }},

        // 2. 콩쥐의 성장 (어머니의 죽음)
        { DialogName.Kongjwi_Intro_GrowingUp, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "하지만 콩쥐의 어머니는 콩쥐를 낳고 얼마\n지나지 않아 세상을 떠나고 말았어요.",
                        "콩쥐 아버지는 잔나비같이 안고 영영 울었죠.",
                        "다행히 콩쥐는 무럭무럭 잘 자라요.\n마을에서도 어찌나 고운지…",
                        "마을에서 콩쥐를 싫어하는 사람은\n아무도 없었답니다."
                    })
                }
            },
            {
                Language.en, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "However, not long after giving birth to Kongjwi,\nher mother passed away.",
                        "Kongjwi's father wept bitterly, holding her.",
                        "Fortunately, Kongjwi grew up well.\nShe was so lovely...",
                        "No one in the village disliked her."
                    })
                }
            }
        }},

        // 3. 새어머니와 팥쥐의 등장
        { DialogName.Kongjwi_Intro_Stepmother, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "그러던 어느날 콩쥐 아버지가\n새어머니를 데리고 와요.",
                        "새어머니는 욕심이 많고 성질이 사나웠죠.",
                        "그리고 팥쥐라는 딸도 데리고 왔어요.",
                        "나이는 콩쥐보다 한살 어렸지만\n제 엄마를 닮아 심술궂고 마음씨도\n고약했답니다."
                    })
                }
            },
            {
                Language.en, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "Then one day, Kongjwi's father\nbrought home a new stepmother.",
                        "The stepmother was greedy and had a fierce temper.",
                        "And she brought a daughter named Patjwi.",
                        "She was one year younger than Kongjwi,\nbut just like her mother, she was mean and unkind."
                    })
                }
            }
        }},

        // 4. 캘리그라피 미션
        { DialogName.Kongjwi_Mission_Calligraphy, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "🎞 2. 콩쥐의 불행한 시작 (글씨 채우기: “콩쥐”, “팥쥐”)",
                        "사용자 미션: 사용자는 ‘콩쥐’, ‘팥쥐’ 이름 부분을 캘리그라피로 써서 화면의 빈 영역을 채운다."
                    })
                }
            },
            {
                Language.en, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "🎞 2. Kongjwi's Unhappy Beginning (Fill in the words: “Kongjwi”, “Patjwi”)",
                        "User Mission: The user must write 'Kongjwi' and 'Patjwi' in calligraphy to fill the empty space on the screen."
                    })
                }
            }
        }},

         { DialogName.Kongjwi_Misfortune_Start, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "새어머니는 아버지 앞에서는\n콩쥐에게 친절하게 굴었지만...",
                        "아버지가 없으면 온갖 구박을 하기 시작했어요."
                    }),
                    (DialogSpeaker.Stepmother, new string[]{ "콩쥐야! 물 떠와라!", "콩쥐! 방 안 쓸고 뭐하니!" }),
                    (DialogSpeaker.Patjwi, new string[]{ "엄마! 쟤 또 꾸물거려! 에이, 게으름뱅이!" }),
                    (DialogSpeaker.Kongjwi, new string[]{ "흑... 흑..." }),
                    (DialogSpeaker.Narration, new string[]{ "콩쥐는 매일 눈물 마를 날이 없었답니다." })
                }
            },
            {
                Language.en, new []{
                    (DialogSpeaker.Narration, new string[]{
                        "The stepmother acted kindly to Kongjwi\nin front of the father, but...",
                        "When he wasn't around, she started tormenting her."
                    }),
                    (DialogSpeaker.Stepmother, new string[]{ "Kongjwi! Fetch water!", "Kongjwi! Why haven't you swept the room!" }),
                    (DialogSpeaker.Patjwi, new string[]{ "Mom! She's slacking again! Ugh, so lazy!" }),
                    (DialogSpeaker.Kongjwi, new string[]{ "*Sob... sob...*" }),
                    (DialogSpeaker.Narration, new string[]{ "Kongjwi cried every single day." })
                }
            }
        }},

        // 6. 밭 매기 일 시키기
        { DialogName.Kongjwi_Task_Field, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new string[]{ "어느 날 새어머니는 콩쥐와 팥쥐에게\n일을 시켰어요." }),
                    (DialogSpeaker.Stepmother, new string[]{
                        "콩쥐는 언니니까 산 너머 저 큰 밭을 다 매거라!",
                        "팥쥐는 동생이니까 집 앞 모래밭의\n풀이나 쪼끔 뽑아라!"
                    }),
                    (DialogSpeaker.Narration, new string[]{ "그러면서 콩쥐한테는 다 망가진 나무 호미를 주고\n팥쥐한테는 튼튼한 쇠 호미를 주었답니다." }),
                    (DialogSpeaker.Patjwi, new string[]{ "엄마 최고! 콩쥐 언니는 힘들겠다~ 메롱~" })
                }
            },
            {
                Language.en, new []{
                    (DialogSpeaker.Narration, new string[]{ "One day, the stepmother gave Kongjwi and Patjwi\na task." }),
                    (DialogSpeaker.Stepmother, new string[]{
                        "Kongjwi, you're the older sister, so go weed that entire big field\nover the mountain!",
                        "Patjwi, you're younger, so just pull a few weeds\nin the sand patch in front of the house!"
                    }),
                    (DialogSpeaker.Narration, new string[]{ "Then, she gave Kongjwi a broken wooden hoe\nand Patjwi a sturdy iron hoe." }),
                    (DialogSpeaker.Patjwi, new string[]{ "Mom's the best! Too bad for you, Kongjwi~ Neener neener~" })
                }
            }
        }},

        // 7. 호미 부러짐
        { DialogName.Kongjwi_Task_HoeBreaks, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Stepmother, new string[]{ "오늘 안에 다 못하면 집에 들어올 생각 마!" }),
                    (DialogSpeaker.Patjwi, new string[]{ "키킥, 꼴좋다!" }),
                    (DialogSpeaker.Narration, new string[]{
                        "누워서 떡먹기처럼 쉬운 일을 한\n팥쥐는 금세 일을 마치고 집으로 돌아왔어요.",
                        "콩쥐는 땡볕에서 나무 호미로\n열심히 잡초를 뽑았어요.",
                        "그런데 이럴 어쩌죠?"
                    }),
                    (DialogSpeaker.Kongjwi, new string[]{ "어... 어떡하지? 호미가..." }),
                    (DialogSpeaker.Narration, new string[]{
                        "그만 나무 호미가 툭~ 부러지고 말았어요.",
                        "콩쥐는 눈앞이 캄캄해져 눈물이 그렁그렁 했어요."
                    })
                }
            },
            {
                Language.en, new []{
                    (DialogSpeaker.Stepmother, new string[]{ "If you don't finish by today, don't even think about coming home!" }),
                    (DialogSpeaker.Patjwi, new string[]{ "Hehe, serves you right!" }),
                    (DialogSpeaker.Narration, new string[]{
                        "Patjwi, who had an easy job,\nfinished quickly and returned home.",
                        "Kongjwi worked hard under the hot sun,\npulling weeds with the wooden hoe.",
                        "But what happened?"
                    }),
                    (DialogSpeaker.Kongjwi, new string[]{ "Oh... what do I do? The hoe..." }),
                    (DialogSpeaker.Narration, new string[]{
                        "The wooden hoe suddenly snapped and broke.",
                        "Kongjwi's world turned dark, and her eyes welled up with tears."
                    })
                }
            }
        }},

    };
}