using System.Collections.Generic;
using UnityEngine;

public class DialogScript
{
    // 튜플 정의 변경: (string, AudioClip) -> (string, AudioClip, ObjectName?)
    // 세 번째 인자(ObjectName?)는 개별 대사에서 화자가 바뀔 때 사용하고, 기본값은 null로 둡니다.
    public static Dictionary<DialogName, Dictionary<Language, (DialogSpeaker, (string, AudioClip, ObjectName?)[])[]>> DialogData = new()
    {
        // ==================================================================
        // --- 장면 1. 콩쥐의 집 ---
        // ==================================================================
        { DialogName.Scene1_Intro, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("옛날 어느 마을에 콩쥐라는 소녀가 살고 있었어요.", SoundList.Instance.IntroNarration1, null), 
                        ("콩쥐는 어머니를 일찍 여의고 아버지와 단둘이 살았지만, 마음씨가 고운 데다 부지런해서 마을 사람들에게 사랑을 많이 받았지요.", SoundList.Instance.IntroNarration2, null),
                        ("하지만 어느 날, 아버지가 새어머니와 그 딸 팥쥐를 데리고 오면서 콩쥐의 삶은 달라지기 시작했어요.", SoundList.Instance.IntroNarration3, null)
                    })
                }
            }
        }},
        { DialogName.Scene1_Stepmother, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Stepmother, new (string, AudioClip, ObjectName?)[]{ 
                        ("(겉으로 상냥하게) 콩쥐야, 앞으로 우리 잘 지내보자꾸나.", SoundList.Instance.IntroStepMother1, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("하지만 새어머니의 속마음은 달랐어요.", SoundList.Instance.IntroNarration4, null) 
                    }),
                    (DialogSpeaker.Stepmother, new (string, AudioClip, ObjectName?)[]{ 
                        ("(혼잣말, 콧방귀) 저 애는 부지런하니, 집안일은 몽땅 저 애 시키면 되겠네.", SoundList.Instance.IntroStepMother2, null) 
                    }),
                    (DialogSpeaker.Patjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(새어머니에게 살짝) 엄마, 나는 힘든 거 싫은데~ 콩쥐한테 다 시켜요.", SoundList.Instance.IntroPatjwi1, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("그날부터 새어머니와 팥쥐는 콩쥐에게 힘든 일만 골라 시키며 괴롭히기 시작했답니다.", SoundList.Instance.IntroNarration5, null) 
                    })
                }
            }
        }},

        // ==================================================================
        // --- 장면 2. 산 너머 큰 밭과 굴착기 두더지 ---
        // ==================================================================
        { DialogName.Scene2_Field_Task, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("어느 날 새어머니가 콩쥐와 팥쥐를 불렀어요.", SoundList.Instance.MoleNarration1, null) 
                    }),
                    (DialogSpeaker.Stepmother, new (string, AudioClip, ObjectName?)[]{
                        ("콩쥐야, 너는 산 너머 큰 밭을 다 매거라.", SoundList.Instance.MoleStepMother1, null),
                        ("팥쥐야, 너는 집 앞 작은 밭만 살살 매면 된다.", SoundList.Instance.MoleStepMother2, null)
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("그러더니 콩쥐에게는 부러지기 쉬운 나무 호미를, 팥쥐에게는 튼튼한 쇠 호미를 쥐여 주었지요.", SoundList.Instance.MoleNarration2, null) 
                    })
                }
            }
        }},
        { DialogName.Scene2_Field_Work, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Patjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(속으로 웃으며) 에이, 이 정도면 오늘도 나는 편하겠는걸?", SoundList.Instance.MolePatjwi1, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("팥쥐는 집 앞에서 조금 일하는 척만 하고 금세 들어가 새어머니와 놀았어요.", SoundList.Instance.MoleNarration3, null),
                        ("하지만 콩쥐는 산을 넘어 커다란 밭에서 땀을 뻘뻘 흘리며 잡초를 뽑고 있었지요.", SoundList.Instance.MoleNarration4, null)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(헉헉거리며) 이 넓은 밭을 언제 다 매나... 그래도 해야지.", SoundList.Instance.MoleKongjwi1, null) 
                    })
                }
            }
        }},
        { DialogName.Scene2_Hoe_Break, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("그때였어요. \"딱!\" 소리와 함께 나무 호미가 두 동강 나고 말았어요.", SoundList.Instance.MoleNarration5, null) 
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(울먹이며) 어떡하지... 호미도 없고, 밭도 아직 반이나 남았는데...", SoundList.Instance.MoleKongjwi2, null) 
                    })
                }
            }
        }},
        { DialogName.Scene2_Mole_Appear, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("바로 그때, 땅속에서 \"부스럭부스럭\" 소리가 나더니,", SoundList.Instance.MoleNarration6, null) 
                    }),
                    (DialogSpeaker.Special, new (string, AudioClip, ObjectName?)[]{
                        ("어디선가 [______1______]이(가) 나타나 [______2______]로 도와주는게 아니겠어요?", SoundList.Instance.EmptySlot, null)
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("어디선가 [ 두더지 ]가 나타나 [ 굴착기 ]로 도와주는게 아니겠어요?", SoundList.Instance.MoleNarration7, ObjectName.ExcavatorMole)
                    }),
                    (DialogSpeaker.ExcavatorMole, new (string, AudioClip, ObjectName?)[]{ 
                        ("(헬멧을 고쳐 쓰며) 흐음, 여기서 누가 한숨을 쉬나 했더니 콩쥐구나?", SoundList.Instance.MoleMole1, null) 
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(깜짝 놀라) 어, 누구세요?", SoundList.Instance.MoleKongjwi3, null) 
                    }),
                    (DialogSpeaker.ExcavatorMole, new (string, AudioClip, ObjectName?)[]{
                        ("나는 땅 파기의 달인, [ 굴착기 두더지 ]다.", SoundList.Instance.MoleMole2, null),
                        ("밭을 매는 건 내가 훨씬 빠르지!", SoundList.Instance.MoleMole3, null)
                    })
                }
            }
        }},
        { DialogName.Scene2_Mole_Help, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("그러더니 [ 굴착기 두더지 ]는 앞발로 밭을 이리저리 파헤치고 뒤엎으며, 잡초를 몽땅 뿌리째 뽑아 버렸어요.", SoundList.Instance.MoleNarration8, null),
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(눈이 동그래져서) 와... 정말 금세 끝났어요! 고마워요, [ 굴착기 두더지 ]님!", SoundList.Instance.MoleKongjwi4, null) 
                    }),
                    (DialogSpeaker.ExcavatorMole, new (string, AudioClip, ObjectName?)[]{ 
                        ("(쿡 웃으며) 다음에 또 힘들면 불러. 그럼 난 이만~", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("[ 굴착기 두더지 ]는 다시 땅속으로 쏙 들어가 버렸고, 콩쥐는 생각보다 훨씬 빨리 일을 마칠 수 있었답니다.", SoundList.Instance.MoleNarration9, null),
                    })
                }
            }
        }},

        // ==================================================================
        // --- 장면 3. 곡식 까기와 안경 쓴 참새들 ---
        // ==================================================================
        { DialogName.Scene3_Grain_Task, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("며칠 뒤, 새어머니는 또 콩쥐를 불렀어요.", null, null) 
                    }),
                    (DialogSpeaker.Stepmother, new (string, AudioClip, ObjectName?)[]{
                        ("우린 잔칫집에 갔다 올 테니, 그 동안", null, null),
                        ("벼 껍질 다 까고, 솥 씻고, 항아리에 물도 가득 채워 놓거라.", null, null),
                        ("못 하면 집에 들어오지도 말거라!", null, null)
                    }),
                    (DialogSpeaker.Patjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(웃으며) 엄마, 나 예쁜 옷 입고 갈래요~ 콩쥐는 또 집에 있으라고 하죠?", null, null) 
                    }),
                    (DialogSpeaker.Stepmother, new (string, AudioClip, ObjectName?)[]{ 
                        ("그렇지, 그렇지. 콩쥐야, 울지 말고 잘~ 해 보거라. (비웃으며 나감)", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("콩쥐는 커다란 벼 자루를 보며 깊은 한숨을 쉬었어요.", null, null) 
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("이걸 언제 다 까지... 손으로 하면 밤새워도 못 끝낼 텐데...", null, null) 
                    })
                }
            }
        }},
        { DialogName.Scene3_Sparrow_Help, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("바로 그때, 하늘 위에서 뭔가 반짝였어요.", null, null)
                    }),
                    (DialogSpeaker.Special, new (string, AudioClip, ObjectName?)[]{
                        ("[______1______]이 반짝이며 날아오더니, [______2______]들이 내려오는 게 아니겠어요?", SoundList.Instance.EmptySlot, null)
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("[ 작은 안경 ]이 반짝이며 날아오더니, [ 참새들 ]이 내려오는 게 아니겠어요?", null, ObjectName.GlassesSparrow)
                    }),
                    (DialogSpeaker.GlassesSparrow1, new (string, AudioClip, ObjectName?)[]{ 
                        ("(안경을 고쳐 쓰며) 음, 벼 껍질 작업인가. 통계상 우리 작업 속도가 제일 빠르지.", null, null) 
                    }),
                    (DialogSpeaker.GlassesSparrow2, new (string, AudioClip, ObjectName?)[]{ 
                        ("그럼 시작해 볼까?", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("[ 안경 쓴 참새들 ]은 벼 이삭에 줄줄이 매달려, 날개로 이삭을 힘껏 털어 주었어요.", null, null),
                        ("그러자 껍질들은 옆으로 휙휙 날아가고, 알맹이만 “토도독토도독” 바구니에 떨어졌지요.", null, null)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("우와... 벌써 다섯 섬이나 끝났어! 여러분 덕분이에요, 고마워요!", null, null) 
                    }),
                    (DialogSpeaker.GlassesSparrow1, new (string, AudioClip, ObjectName?)[]{ 
                        ("(멋있게 고개 끄덕이며) 협업은 언제나 옳다구. 그럼 우리는 다음 ARP 프로젝트가 있어서 이만!", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("[ 안경 쓴 참새들 ]은 하늘로 \"짹짹!\" 노래를 부르며 사라졌어요.", null, null) 
                    })
                }
            }
        }},

        // ==================================================================
        // --- 장면 4. 항아리의 구멍과 납작한 조개 ---
        // ==================================================================
        { DialogName.Scene4_Jar_Task, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("이제 남은 건 항아리에 물을 채우는 일이었어요.", null, null),
                        ("콩쥐는 샘물로 가서 여러 번 물을 길어 와 항아리에 부었습니다.", null, null),
                        ("하지만 아무리 부어도 물이 가득 차지 않는 거예요.", null, null)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("어? 분명히 이렇게 많이 부었는데 왜...?", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("가만히 살펴보니, 항아리 바닥에 커다란 구멍이 뻥 뚫려 있었지요.", null, ObjectName.CrackedClay) 
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(눈물이 맺혀) 또 혼나겠네... 어떡하지...", null, null) 
                    })
                }
            }
        }},
        { DialogName.Scene4_Clam_Help, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Special, new (string, AudioClip, ObjectName?)[]{
                        ("그때 물가에서 햇빛을 받으며 조용히 빛나던 [______1______] 하나가 살며시 움직이더니, [______2______] 도와주겠다는게 아니겠어요?", SoundList.Instance.EmptySlot, null)
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("그때 물가에서 햇빛을 받으며 조용히 빛나던 [ 납작한 조개 ] 하나가 살며시 움직이더니, [ 단단히 막아 ] 도와주겠다는게 아니겠어요?", null, ObjectName.FlatClam),
                    }),
                    (DialogSpeaker.FlatClam, new (string, AudioClip, ObjectName?)[]{ 
                        ("(조용한 목소리로) 구멍이 문제라면, 내가 좀 도와줄 수 있을지도 모르겠네.", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("[ 납작한 조개 ]는 항아리 안으로 들어가 껍데기를 쫙 펼쳐", null, null),
                        ("구멍을 [ 단단히 막아 ] 주었어요.", null, ObjectName.Vase_Shell),
                        ("이제 물은 더 이상 새어나가지 않고,", null, null),
                        ("콩쥐가 물을 부을 때마다 항아리 안에 차곡차곡 쌓여 갔지요.", null, null)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("정말 고마워요, [ 납작한 조개 ]야!", null, null) 
                    }),
                    (DialogSpeaker.FlatClam, new (string, AudioClip, ObjectName?)[]{ 
                        ("(살짝 반짝이며) 아무에게도 말하지 말고, 나중에 물가에 놀러 와 줘.", null, null) 
                    })
                }
            }
        }},

        // ==================================================================
        // --- 장면 5. 종이접기 나비 요정의 비단옷과 꽃신 ---
        // ==================================================================
        { DialogName.Scene5_Fairy_Appear, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("이렇게 새어머니가 시킨 힘든 일들을 거의 다 해냈을 때였어요.", null, null),
                        ("어디선가 \"바스락, 바스락\" 소리가 나기 시작했지요.", null, null)
                    }),
                    (DialogSpeaker.Special, new (string, AudioClip, ObjectName?)[]{
                        ("공중에서 [______1______]들이 팔랑팔랑 날아오더니 스스로 [______2______]가 되는 게 아니겠어요?", SoundList.Instance.EmptySlot, null)
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("공중에서 [ 하얀 종이 ]들이 팔랑팔랑 날아오더니 스스로 [ 나비 ]가 되는 게 아니겠어요?", null, ObjectName.OrigamiButterflyFairy)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("우와... [ 종이 나비 ] 다...", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("그 사이, 종이 나비들 가운데에서 [ 종이접기 나비 요정 ]이 나타났어요.", null, ObjectName.Fairy) 
                    }),
                    (DialogSpeaker.OrigamiButterflyFairy, new (string, AudioClip, ObjectName?)[]{
                        ("안녕, 콩쥐야. 너의 부지런함은 여기까지 소문이 났단다.", null, null),
                        ("이제 잔치에 갈 시간이지?", null, null)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(조심스럽게) 그런데... 저는 입고 갈 옷도, 신발도 없어요.", null, null) 
                    })
                }
            }
        }},
        { DialogName.Scene5_Fairy_Gift, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("그러자 [ 종이접기 나비 요정 ]은 두 손을 펼치며", null, null),
                        ("종이와 달빛 조각을 한데 모아 빙글빙글 돌렸어요.", null, null),
                        ("[종이접기 나비 요정]은 종이와 달빛으로", null, null),
                        ("비단옷과 꽃신을 후다닥 접어 만들어 콩쥐에게 건네주었어요.", null, null)
                    }),
                    (DialogSpeaker.OrigamiButterflyFairy, new (string, AudioClip, ObjectName?)[]{ 
                        ("이 옷과 꽃신을 신고, 너도 네가 얼마나 소중한 사람인지 잊지 말고 잔치에 다녀오렴.", null, null) 
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("정말... 감사합니다!", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("콩쥐는 [ 종이접기 나비 요정 ]이 건네준 비단옷과 꽃신을 신고", null, null),
                        ("조심조심 잔칫집으로 향했답니다.", null, null)
                    })
                }
            }
        }},

        // ==================================================================
        // --- 장면 6. 다리 위에서 잃어버린 꽃신 ---
        // ==================================================================
        { DialogName.Scene6_Lost_Shoe, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("마을 어귀에 놓인 다리를 건너던 중이었어요.", null, null),
                        ("콩쥐가 생각에 잠긴 사이, 꽃신 한 짝이 \"툭!” 하고 다리 아래 개울로 떨어지고 말았지요.", null, null)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(깜짝 놀라) 아! 내 꽃신!", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("꽃신은 물 위에 동동 떠 있다가 점점 아래로 흘러 내려갔어요.", null, null),
                        ("그 아래쪽 물가에서는 [ 납작한 조개 ]가 햇빛을 쬐고 있었는데,", null, null),
                        ("꽃신이 떠내려오는 것을 보고 \"지금이다!\" 하고 껍데기를 탁 닫았다 펴며", null, null),
                        ("꽃신을 강가 모래 위로 '툭' 튕겨 올렸어요.", null, null),
                        ("마침 그때, 원님 일행이 그 길을 지나가고 있었지요.", null, null)
                    })
                }
            }
        }},

        // ==================================================================
        // --- 장면 7. 원님과 꽃신의 주인 ---
        // ==================================================================
        { DialogName.Scene7_Magistrate_Find, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Magistrate, new (string, AudioClip, ObjectName?)[]{ 
                        ("(수레 위에서) 저기 떨어진 것이 무엇이냐?", null, null) 
                    }),
                    (DialogSpeaker.Official, new (string, AudioClip, ObjectName?)[]{ 
                        ("(달려가 꽃신을 줍고) 원님, 반짝이는 꽃신 한 짝이옵니다.", null, null) 
                    }),
                    (DialogSpeaker.Magistrate, new (string, AudioClip, ObjectName?)[]{
                        ("참으로 귀한 신이로다.", null, null),
                        ("여봐라, 이 꽃신의 주인을 찾아오너라!", null, null)
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("그리하여 마을 곳곳에 \"비단 꽃신의 주인을 찾습니다\"라는 방이 붙었어요.", null, null),
                        ("마을 처녀들은 너도나도 꽃신을 신어 보겠다며 줄을 섰지만,", null, null),
                        ("누구의 발에도 꼭 맞지는 않았습니다.", null, null)
                    })
                }
            }
        }},
        { DialogName.Scene7_Shoe_Test, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Stepmother, new (string, AudioClip, ObjectName?)[]{ 
                        ("(팥쥐를 데리고 와) 이리 와, 팥쥐야. 네 발이랑 딱 맞을 거야. 억지로라도 신어 보거라.", null, null) 
                    }),
                    (DialogSpeaker.Patjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(신발을 밀어 넣으며) 아, 너무 끼는데...", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("그때 하늘에서 [ 종이접기 나비 요정 ]이 몰래 내려와,", null, null),
                        ("[ 종이 나비들 ]을 팥쥐 주위에 [ 휘리릭 날려 보내며 ] 발등을 간질였어요.", null, null)
                    }),
                    (DialogSpeaker.Patjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("으악, 간지러워! 못 신겠어요!", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("결국 팥쥐의 발은 꽃신에 들어가지 않았지요.", null, null) 
                    }),
                    (DialogSpeaker.Official, new (string, AudioClip, ObjectName?)[]{ 
                        ("(주위를 둘러보다가) 저기 구석에 서 있는 저 처녀는 누구요? 참 얌전하고 곱게 생겼구려.", null, null) 
                    }),
                    (DialogSpeaker.Stepmother, new (string, AudioClip, ObjectName?)[]{ 
                        ("(당황하며) 아, 저 애는... 그냥 집안일이나 하는 아이...", null, null) 
                    }),
                    (DialogSpeaker.Official, new (string, AudioClip, ObjectName?)[]{ 
                        ("그대도 꽃신을 한 번 신어 보시오.", null, null) 
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("(조심스럽게) 제가... 신어도 될까요?", null, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("콩쥐가 꽃신을 살며시 발에 신어 보니,", null, null),
                        ("마치 기다렸다는 듯이 딱 맞는 게 아니겠어요?", null, null)
                    }),
                    (DialogSpeaker.Official, new (string, AudioClip, ObjectName?)[]{ 
                        ("오! 꼭 맞습니다, 원님!", null, null) 
                    }),
                    (DialogSpeaker.Magistrate, new (string, AudioClip, ObjectName?)[]{
                        ("(콩쥐를 바라보며) 그대가 이 꽃신의 주인이겠구나.", null, null),
                        ("이름이 무엇이오?", null, null)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("저는... 콩쥐라고 합니다.", null, null) 
                    })
                }
            }
        }},

        // ==================================================================
        // --- 장면 8. 결말 - 함께 바뀌어 가는 사람들 ---
        // ==================================================================
        { DialogName.Scene8_Ending, new ()
        {
            {
                Language.kr, new []{
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("그 순간, 마당 한쪽에서는 [ 굴착기 두더지 ]가 기뻐서 땅을 [ 부스럭부스럭 파고 ] 있었고,", SoundList.Instance.EndNarration0, null),
                        ("지붕 위에서는 [ 안경 쓴 참새들 ]이 콩쥐 이름을 [ 신나게 노래하고 ],", SoundList.Instance.EndNarration1, null),
                        ("개울가에서는 [ 납작한 조개 ]가 [ 햇빛을 받으며 반짝였어요 ].", SoundList.Instance.EndNarration2, null),
                        ("하늘 위에는 [ 종이접기 나비 요정 ]이 [ 종이 나비들을 흩뿌리며 ] 콩쥐를 축복하고 있었지요.", SoundList.Instance.EndNarration3, null)
                    }),
                    (DialogSpeaker.Magistrate, new (string, AudioClip, ObjectName?)[]{
                        ("콩쥐, 그대는 힘들었을 텐데도 이렇게 따뜻한 눈을 하고 있구려.", SoundList.Instance.EndMagistrate1, null),
                        ("나와 함께 이 고을 사람들을 도우며 살겠소?", SoundList.Instance.EndMagistrate2, null)
                    }),
                    (DialogSpeaker.Kongjwi, new (string, AudioClip, ObjectName?)[]{
                        ("(잠시 고민하다가) 네... 다만, 새어머니와 팥쥐도...", SoundList.Instance.EndKongjwi1, null),
                        ("언젠가는 함께 웃을 수 있으면 좋겠습니다.", SoundList.Instance.EndKongjwi2, null)
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{ 
                        ("그 말을 들은 새어머니와 팥쥐는 얼굴을 붉히며 고개를 숙였어요.", SoundList.Instance.EndNarration4, null) 
                    }),
                    (DialogSpeaker.Stepmother, new (string, AudioClip, ObjectName?)[]{ 
                        ("콩쥐야, 그동안 미안했다... 다시는 그러지 않을게.", SoundList.Instance.EndStepMother1, null) 
                    }),
                    (DialogSpeaker.Patjwi, new (string, AudioClip, ObjectName?)[]{ 
                        ("나도... 잘못했어. 앞으로는 같이 일도 하고, 같이 놀자.", SoundList.Instance.EndPatjwi1, null) 
                    }),
                    (DialogSpeaker.Narration, new (string, AudioClip, ObjectName?)[]{
                        ("그날 이후, 콩쥐와 원님은 마을 사람들을 도우며 살았고,", SoundList.Instance.EndNarration5, null),
                        ("새어머니와 팥쥐도 조금씩 마음을 고쳐 나갔답니다.", SoundList.Instance.EndNarration6, null),
                        ("[ 굴착기 두더지 ]는 논밭을 도와주고,", SoundList.Instance.EndNarration7, null),
                        ("[ 안경 쓴 참새들 ]은 씨앗을 알맞게 골라 뿌려 주었지요.", SoundList.Instance.EndNarration8, null),
                        ("[ 납작한 조개 ]는 물길을 지키며 가뭄을 막았고,", SoundList.Instance.EndNarration9, null),
                        ("[ 종이접기 나비 요정 ]은 힘들어하는 사람들에게", SoundList.Instance.EndNarration10, null),
                        ("작은 종이 나비와 함께 새로운 용기를 살짝 건네주었답니다.", SoundList.Instance.EndNarration11, null),
                        ("그래서 그 마을 사람들은 서로 도우며 웃는 법을 잊지 않고,", SoundList.Instance.EndNarration12, null),
                        ("오래오래 행복하게 살았다고 해요.", SoundList.Instance.EndNarration13, null)
                    })
                }
            }
        }}
    };
}