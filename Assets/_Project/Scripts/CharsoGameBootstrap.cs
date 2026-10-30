using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.EventSystems;
namespace Charso {
 public sealed class CharsoGameBootstrap:MonoBehaviour {
  PlayerProgress p; readonly List<int> picks=new List<int>(); readonly List<Button> letterButtons=new List<Button>();
  Text coins,clue,answer,status,stage; Transform letters; Button submit,hint,restore,next; int solvedIndex=-1; bool restored;
  void Awake(){Application.targetFrameRate=60;p=SaveSystem.Load();MehrganWorldBuilder.Build(p.RestorationStage);EnsureEventSystem();solvedIndex=p.CurrentLevelSolved?p.LevelIndex:-1;restored=p.CurrentLevelRestored;BuildUI();Refresh();}
  void BuildUI(){
   var c=new GameObject("Canvas",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster)).GetComponent<Canvas>();c.renderMode=RenderMode.ScreenSpaceOverlay;
   var sc=c.GetComponent<CanvasScaler>();sc.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;sc.referenceResolution=new Vector2(720,1280);sc.matchWidthOrHeight=.5f;
   var bg=Panel(c.transform,"BG",new Color32(239,245,250,55));Stretch(bg.GetComponent<RectTransform>());
   var title=Label(c.transform,"چهارسو",54,new Color32(22,65,105,255));Place(title.rectTransform,.06f,.88f,.94f,.97f);
   var sub=Label(c.transform,"ماجرای واژه‌ها و میراث ایران",24,new Color32(73,104,132,255));Place(sub.rectTransform,.06f,.83f,.94f,.89f);
   var stats=Panel(c.transform,"Stats",Color.white);Place(stats.GetComponent<RectTransform>(),.07f,.755f,.93f,.82f);
   coins=Label(stats.transform,"",28,new Color32(150,103,20,255));Place(coins.rectTransform,.08f,.02f,.78f,.98f);AddIcon(stats.transform,CharsoIcon.Coin,.82f,.16f,.94f,.84f);
   var card=Panel(c.transform,"Puzzle",Color.white);Place(card.GetComponent<RectTransform>(),.055f,.40f,.945f,.735f);
   var heading=Label(card.transform,"مرحله واژه",28,new Color32(30,78,120,255));Place(heading.rectTransform,.05f,.83f,.95f,.98f);
   clue=Label(card.transform,"",26,new Color32(45,57,68,255));Place(clue.rectTransform,.05f,.63f,.95f,.83f);
   answer=Label(card.transform,"؟",40,new Color32(18,86,142,255));Place(answer.rectTransform,.05f,.43f,.95f,.64f);
   letters=new GameObject("Letters",typeof(RectTransform)).transform;letters.SetParent(card.transform,false);Place(letters.GetComponent<RectTransform>(),.03f,.19f,.97f,.43f);
   submit=Btn(card.transform,"بررسی پاسخ",new Color32(25,116,184,255),Color.white,22);Place(submit.GetComponent<RectTransform>(),.05f,.015f,.53f,.17f);submit.onClick.AddListener(Check);DecorateButton(submit,CharsoIcon.Star);
   var clear=Btn(card.transform,"پاک کردن",new Color32(225,233,240,255),new Color32(45,72,96,255),22);Place(clear.GetComponent<RectTransform>(),.56f,.015f,.95f,.17f);clear.onClick.AddListener(Clear);
   hint=Btn(c.transform,"راهنمایی  (۳ سکه)",new Color32(232,184,77,255),new Color32(55,47,29,255),23);Place(hint.GetComponent<RectTransform>(),.07f,.32f,.48f,.38f);hint.onClick.AddListener(Hint);DecorateButton(hint,CharsoIcon.Hammer);
   next=Btn(c.transform,"معمای بعدی",new Color32(25,116,184,255),Color.white,22);Place(next.GetComponent<RectTransform>(),.52f,.32f,.93f,.38f);next.onClick.AddListener(Next);DecorateButton(next,CharsoIcon.Arrow);
   status=Label(c.transform,"حروف را انتخاب کن و پاسخ را بررسی کن.",21,new Color32(45,78,105,255));Place(status.rectTransform,.06f,.26f,.94f,.32f);
   var heritage=Panel(c.transform,"Heritage",new Color32(222,235,244,255));Place(heritage.GetComponent<RectTransform>(),.055f,.055f,.945f,.245f);
   var h=Label(heritage.transform,"بازسازی یک اثر تاریخی ایران",24,new Color32(30,78,120,255));Place(h.rectTransform,.04f,.73f,.96f,.98f);
   stage=Label(heritage.transform,"",24,new Color32(47,68,81,255));Place(stage.rectTransform,.04f,.48f,.96f,.76f);
   var note=Label(heritage.transform,"حافظیه، شیراز — نمونه مفهومی؛ مدل تاریخی هنوز تأیید نشده است.",17,new Color32(69,85,98,255));Place(note.rectTransform,.05f,.20f,.95f,.49f);
   restore=Btn(heritage.transform,"اجرای گام بازسازی (۱۰ سکه)",new Color32(38,132,102,255),Color.white,20);Place(restore.GetComponent<RectTransform>(),.10f,.01f,.90f,.20f);restore.onClick.AddListener(Restore);DecorateButton(restore,CharsoIcon.Home);
   var foot=Label(c.transform,"نسخه آزمایشی ۰.۱ • شهر مهرگان",16,new Color32(107,124,139,255));Place(foot.rectTransform,.06f,.012f,.94f,.05f);
  }
  void Refresh(){var l=WordPuzzle.Levels[p.LevelIndex];coins.text="سکه: "+p.Coins;clue.text=l.Clue;answer.text=picks.Count==0?"؟":JoinPicks();stage.text="حافظیه • گام بازسازی "+p.RestorationStage+" از ۳";BuildLetters(l);bool solved=solvedIndex==p.LevelIndex;submit.interactable=!solved;hint.interactable=!solved&&p.Coins>=3;restore.interactable=solved&&!restored&&p.Coins>=10&&p.RestorationStage<3;next.gameObject.SetActive(restored&&p.RestorationStage<3&&p.LevelIndex<WordPuzzle.Levels.Length-1);status.text=solved?(restored?"آفرین! برای ادامه، معمای بعدی را انتخاب کن.":"واژه درست بود! حالا یک گام بازسازی انجام بده."):"حروف را به ترتیب انتخاب کن و پاسخ را بررسی کن.";}
  void BuildLetters(PuzzleLevel l){foreach(Transform t in letters)Destroy(t.gameObject);letterButtons.Clear();float w=.90f/l.Letters.Length;for(int i=0;i<l.Letters.Length;i++){int idx=i;var b=Btn(letters,l.Letters[i],new Color32(226,239,249,255),new Color32(26,73,108,255),32);float x=.05f+i*w;Place(b.GetComponent<RectTransform>(),x,.08f,x+w-.02f,.92f);b.onClick.AddListener(()=>Pick(idx));letterButtons.Add(b);}}
  void Pick(int i){if(solvedIndex==p.LevelIndex||picks.Count>=WordPuzzle.Levels[p.LevelIndex].Letters.Length)return;picks.Add(i);letterButtons[i].interactable=false;answer.text=JoinPicks();}
  string JoinPicks(){string s="";foreach(int i in picks)s+=WordPuzzle.Levels[p.LevelIndex].Letters[i];return s;}
  void Clear(){picks.Clear();foreach(var b in letterButtons)b.interactable=true;answer.text="؟";}
  void Check(){if(!WordPuzzle.IsCorrect(WordPuzzle.Levels[p.LevelIndex],JoinPicks())){status.text="این پاسخ درست نیست؛ دوباره تلاش کن.";return;}solvedIndex=p.LevelIndex;p.CurrentLevelSolved=true;p.SolvedLevels++;p.Coins+=8;SaveSystem.Store(p);Refresh();status.text="درست است! ۸ سکه گرفتی.";}
  void Hint(){if(p.Coins<3||solvedIndex==p.LevelIndex)return;p.Coins-=3;SaveSystem.Store(p);coins.text="سکه: "+p.Coins;status.text="راهنمایی: واژه با «"+WordPuzzle.Levels[p.LevelIndex].Answer.Substring(0,1)+"» آغاز می‌شود.";hint.interactable=p.Coins>=3;}
  void Restore(){if(solvedIndex!=p.LevelIndex||restored||p.Coins<10||p.RestorationStage>=3)return;p.Coins-=10;p.RestorationStage++;restored=true;p.CurrentLevelRestored=true;SaveSystem.Store(p);MehrganWorldBuilder.ApplyRestorationStage(p.RestorationStage);Refresh();}
  void Next(){if(!restored||p.LevelIndex>=WordPuzzle.Levels.Length-1)return;p.LevelIndex++;solvedIndex=-1;restored=false;p.CurrentLevelSolved=false;p.CurrentLevelRestored=false;picks.Clear();SaveSystem.Store(p);Refresh();}
  static void AddIcon(Transform parent,CharsoIcon kind,float x0,float y0,float x1,float y1){var g=new GameObject("Icon_"+kind,typeof(RectTransform),typeof(Image));g.transform.SetParent(parent,false);var img=g.GetComponent<Image>();img.sprite=CharsoIconFactory.Create(kind);img.preserveAspect=true;Place(g.GetComponent<RectTransform>(),x0,y0,x1,y1);}
  static void DecorateButton(Button button,CharsoIcon kind){var label=button.GetComponentInChildren<Text>();if(label!=null)Place(label.rectTransform,.20f,0,.98f,1);AddIcon(button.transform,kind,.035f,.18f,.18f,.82f);var sh=button.gameObject.AddComponent<Shadow>();sh.effectColor=new Color(0,0,0,.14f);sh.effectDistance=new Vector2(0,-2);}
  static void EnsureEventSystem(){if(EventSystem.current!=null)return;new GameObject("EventSystem",typeof(EventSystem),typeof(StandaloneInputModule));}
  static GameObject Panel(Transform parent,string name,Color color){var g=new GameObject(name,typeof(RectTransform),typeof(Image));g.transform.SetParent(parent,false);g.GetComponent<Image>().color=color;if(name!="BG"){var sh=g.AddComponent<Shadow>();sh.effectColor=new Color(0,0,0,.12f);sh.effectDistance=new Vector2(0,-3);}return g;}
  static Text Label(Transform parent,string value,int size,Color color){var g=new GameObject("Text",typeof(RectTransform),typeof(Text));g.transform.SetParent(parent,false);var t=g.GetComponent<Text>();t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");t.text=value;t.fontSize=size;t.color=color;t.alignment=TextAnchor.MiddleCenter;t.horizontalOverflow=HorizontalWrapMode.Wrap;t.verticalOverflow=VerticalWrapMode.Truncate;t.resizeTextForBestFit=true;t.resizeTextMinSize=14;t.resizeTextMaxSize=size;t.raycastTarget=false;return t;}
  static Button Btn(Transform parent,string text,Color bg,Color fg,int size){var g=new GameObject("Button",typeof(RectTransform),typeof(Image),typeof(Button));g.transform.SetParent(parent,false);g.GetComponent<Image>().color=bg;var b=g.GetComponent<Button>();var c=b.colors;c.normalColor=bg;c.highlightedColor=Color.Lerp(bg,Color.white,.15f);c.pressedColor=Color.Lerp(bg,Color.black,.12f);b.colors=c;var t=Label(g.transform,text,size,fg);Stretch(t.rectTransform);return b;}
  static void Place(RectTransform r,float x0,float y0,float x1,float y1){r.anchorMin=new Vector2(x0,y0);r.anchorMax=new Vector2(x1,y1);r.offsetMin=Vector2.zero;r.offsetMax=Vector2.zero;r.localScale=Vector3.one;}
  static void Stretch(RectTransform r)=>Place(r,0,0,1,1);
 }
}