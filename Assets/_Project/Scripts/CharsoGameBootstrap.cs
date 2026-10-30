using System;
using System.Collections;
using System.Collections.Generic;
using Charso.Core;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Charso
{
    /// <summary>
    /// Runtime entry point and responsive Persian-first game HUD.
    /// The first playable experience is built at runtime so the included scene stays small.
    /// </summary>
    public sealed class CharsoGameBootstrap : MonoBehaviour
    {
        PlayerProgress progress;
        readonly List<int> pickedTiles = new List<int>();
        readonly List<Button> tileButtons = new List<Button>();
        readonly List<Button> cityProjectButtons = new List<Button>();
        readonly List<RTLTextMeshPro> cityProjectLabels = new List<RTLTextMeshPro>();

        RTLTextMeshPro coinsLabel;
        RTLTextMeshPro clueLabel;
        RTLTextMeshPro answerLabel;
        RTLTextMeshPro statusLabel;
        RTLTextMeshPro stageLabel;
        RTLTextMeshPro levelLabel;
        RTLTextMeshPro cityShopBalance;
        Image progressFill;
        Transform tileTray;
        RectTransform safeRoot;
        Button submitButton;
        Button clearButton;
        Button hintButton;
        Button restoreButton;
        Button nextButton;
        Button cityButton;
        GameObject cityShopOverlay;
        GameObject infoOverlay;
        int solvedLevel = -1;
        bool restored;
        static TMP_FontAsset runtimePersianFont;
        static Sprite roundedSprite;
        static Sprite circleSprite;
        static Sprite whiteSprite;
        static Sprite backgroundSprite;
        RenderTexture cityPreviewTexture;

        static readonly Color Ink = new Color32(25, 48, 70, 255);
        static readonly Color MutedInk = new Color32(91, 113, 132, 255);
        static readonly Color Blue = new Color32(35, 111, 184, 255);
        static readonly Color Teal = new Color32(20, 139, 133, 255);
        static readonly Color Gold = new Color32(244, 190, 77, 255);
        static readonly Color Paper = new Color32(255, 255, 255, 250);
        static readonly Color SoftBlue = new Color32(235, 244, 252, 255);
        static readonly Color Coral = new Color32(242, 126, 102, 255);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void EnsureGameStartsFromAnEmptyScene()
        {
            if (UnityEngine.Object.FindFirstObjectByType<CharsoGameBootstrap>() != null)
                return;

            // A separate heritage research scene must remain independent from the main game.
            if (UnityEngine.Object.FindFirstObjectByType<HafeziehChapterSceneBootstrap>() != null)
                return;

            var entry = new GameObject("CharsoGame - Automatic Entry");
            entry.AddComponent<CharsoGameBootstrap>();
            Debug.LogWarning("Charso: no game bootstrap was present in the opened scene; the main game was started automatically.");
        }

        void Awake()
        {
            Application.targetFrameRate = 60;
            try
            {
                progress = SaveSystem.Load();
                if (progress == null)
                    progress = new PlayerProgress();

                progress.LevelIndex = Mathf.Clamp(progress.LevelIndex, 0, WordPuzzle.Levels.Length - 1);
                solvedLevel = progress.CurrentLevelSolved ? progress.LevelIndex : -1;
                restored = progress.CurrentLevelRestored;

                MehrganWorldBuilder.Build(progress.RestorationStage);
                MehrganWorldBuilder.ApplyCityProjects(progress.OwnedCityProjectIds);
                EnsureEventSystem();
                BuildUI();
                Refresh();
                Debug.Log("CHARSO_GAME_BOOTSTRAP_READY | level=" + (progress.LevelIndex + 1));
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                BuildStartupError(exception);
            }
        }

        static TMP_FontAsset ResolvePersianFont()
        {
            if (runtimePersianFont != null)
                return runtimePersianFont;

            try
            {
                // Prefer the project-bundled Vazirmatn font. This avoids depending on which
                // fonts happen to be installed on Windows, Android or a tablet.
                Font source = Resources.Load<Font>("Fonts/Vazirmatn-Regular");

                // OS font fallback is only a safety net for damaged/missing resource imports.
                if (source == null)
                {
                    string[] installed = Font.GetOSInstalledFontNames();
                    var candidates = new List<string>();
                    string[] preferred = {
                        "Vazirmatn UI", "Vazirmatn", "Noto Sans Arabic UI",
                        "Noto Naskh Arabic UI", "Noto Sans Arabic", "Noto Naskh Arabic",
                        "Tahoma", "Segoe UI", "Arial", "Roboto"
                    };

                    for (int p = 0; p < preferred.Length; p++)
                    {
                        for (int i = 0; i < installed.Length; i++)
                        {
                            if (installed[i].IndexOf(preferred[p], StringComparison.OrdinalIgnoreCase) >= 0 &&
                                !candidates.Contains(installed[i]))
                                candidates.Add(installed[i]);
                        }
                    }

                    for (int i = 0; i < installed.Length && candidates.Count < 28; i++)
                    {
                        if (!candidates.Contains(installed[i]))
                            candidates.Add(installed[i]);
                    }

                    source = candidates.Count > 0
                        ? Font.CreateDynamicFontFromOSFont(candidates.ToArray(), 96)
                        : Font.CreateDynamicFontFromOSFont(new[] { "Arial", "Tahoma", "Roboto" }, 96);
                }

                if (source != null)
                    runtimePersianFont = TMP_FontAsset.CreateFontAsset(source);
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Charso Persian font fallback: " + exception.Message);
            }

            if (runtimePersianFont == null && TMP_Settings.defaultFontAsset != null)
                runtimePersianFont = TMP_Settings.defaultFontAsset;

            return runtimePersianFont;
        }

        void BuildUI()
        {
            var canvasObject = new GameObject("Charso UI", typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(CanvasGroup));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            scaler.referencePixelsPerUnit = 100;

            safeRoot = new GameObject("Safe Area", typeof(RectTransform), typeof(SafeAreaFitter)).GetComponent<RectTransform>();
            safeRoot.SetParent(canvasObject.transform, false);
            Stretch(safeRoot);

            var background = MakeImage(safeRoot, "Soft blue background", BackgroundSprite(), Color.white);
            Stretch(background.rectTransform);
            background.raycastTarget = false;

            BuildHeader();
            BuildHeroCard();
            BuildProgressStrip();
            BuildPuzzleCard();
            BuildStatusBanner();
            BuildBottomNavigation();
            BuildCityShopPanel();
            AddEntranceAnimation(canvasObject);
        }

        void BuildHeader()
        {
            var header = Panel(safeRoot, "Header", new Color32(255, 255, 255, 238), 0.04f, 0.918f, 0.96f, 0.988f);
            AddShadow(header, 0.08f, -5f);

            var mark = Shape(header.transform, "Brand mark", 0.885f, 0.19f, 0.97f, 0.81f,
                new Color32(25, 132, 139, 255), true);
            AddShape(header.transform, "Brand star", 0.912f, 0.39f, 0.944f, 0.69f, Gold, true);

            var title = Label(header.transform, "چهارسو", 38, Ink, TextAlignmentOptions.Right, true);
            Place(title.rectTransform, 0.55f, 0.42f, 0.87f, 0.92f);

            var subtitle = Label(header.transform, "ماجرای واژه‌ها و میراث ایران", 17, MutedInk, TextAlignmentOptions.Right);
            Place(subtitle.rectTransform, 0.48f, 0.08f, 0.87f, 0.47f);

            var coinPill = Panel(header.transform, "Coin balance", new Color32(255, 246, 221, 255), 0.035f, 0.16f, 0.42f, 0.84f);
            var coinIcon = Shape(coinPill.transform, "Coin icon", 0.79f, 0.18f, 0.98f, 0.82f, Gold, true);
            AddShape(coinPill.transform, "Coin glint", 0.84f, 0.58f, 0.89f, 0.76f, new Color32(255, 250, 218, 255), true);
            coinsLabel = Label(coinPill.transform, "۰ سکه", 23, new Color32(130, 88, 26, 255), TextAlignmentOptions.Center, true);
            Place(coinsLabel.rectTransform, 0.05f, 0.04f, 0.78f, 0.96f);
            AddFloat(coinIcon.rectTransform, 2f, 1.7f);
        }

        void BuildHeroCard()
        {
            var hero = Panel(safeRoot, "Mehrgan chapter card", new Color32(20, 79, 101, 255), 0.04f, 0.686f, 0.96f, 0.902f);
            AddShadow(hero, 0.15f, -7f);

            var glow = Shape(hero.transform, "Sky glow", 0.00f, 0.0f, 0.68f, 1f,
                new Color32(56, 148, 163, 92), true);
            glow.GetComponent<Image>().sprite = CircleSprite();
            AddLiveCityViewport(hero.transform);

            var chapterTag = Panel(hero.transform, "Chapter tag", new Color32(255, 255, 255, 34), 0.61f, 0.72f, 0.94f, 0.91f);
            var tagText = Label(chapterTag.transform, "فصل اول  •  مهرگان", 18, new Color32(255, 232, 183, 255), TextAlignmentOptions.Center, true);
            Stretch(tagText.rectTransform);

            var welcome = Label(hero.transform, "شهر منتظر قهرمانشه!", 31, Color.white, TextAlignmentOptions.Right, true);
            Place(welcome.rectTransform, 0.51f, 0.45f, 0.94f, 0.70f);

            var message = Label(hero.transform, "هر واژه که پیدا کنی،\nیک قدم شهر رو زنده‌تر می‌کنی.", 19,
                new Color32(224, 243, 246, 255), TextAlignmentOptions.Right);
            Place(message.rectTransform, 0.49f, 0.18f, 0.94f, 0.47f);

            var sparkle = Shape(hero.transform, "Hero sparkle", 0.77f, 0.30f, 0.81f, 0.38f, Gold, true);
            AddFloat(sparkle, 2.8f, 1.9f);
            var sparkle2 = Shape(hero.transform, "Hero sparkle small", 0.90f, 0.51f, 0.925f, 0.56f, new Color32(255, 255, 255, 235), true);
            AddFloat(sparkle2, 3.4f, 2.2f);
        }

        void AddLiveCityViewport(Transform parent)
        {
            // Show the real 3D Mehrgan scene inside the chapter card instead of a flat placeholder.
            Camera cityCamera = Camera.main;
            if (cityCamera == null)
            {
                Debug.LogError("Charso city preview could not find the main world camera.");
                return;
            }

            cityPreviewTexture = new RenderTexture(720, 440, 16, RenderTextureFormat.ARGB32);
            cityPreviewTexture.name = "Charso Mehrgan Live View";
            cityPreviewTexture.antiAliasing = 2;
            cityPreviewTexture.filterMode = FilterMode.Bilinear;
            cityPreviewTexture.Create();

            cityCamera.targetTexture = cityPreviewTexture;
            cityCamera.orthographic = true;
            cityCamera.orthographicSize = 22f;
            cityCamera.clearFlags = CameraClearFlags.SolidColor;
            cityCamera.backgroundColor = new Color32(150, 205, 221, 255);

            var viewport = Panel(parent, "Live 3D Mehrgan viewport",
                new Color32(25, 73, 91, 255), 0.035f, 0.075f, 0.585f, 0.91f);
            viewport.AddComponent<RectMask2D>();

            var rawObject = new GameObject("Rendered Mehrgan city", typeof(RectTransform),
                typeof(CanvasRenderer), typeof(RawImage));
            rawObject.transform.SetParent(viewport.transform, false);
            var rawRect = rawObject.GetComponent<RectTransform>();
            Stretch(rawRect);

            var raw = rawObject.GetComponent<RawImage>();
            raw.texture = cityPreviewTexture;
            raw.color = Color.white;
            raw.raycastTarget = false;

            var shade = Panel(viewport.transform, "Live view lower shade",
                new Color32(8, 39, 53, 80), 0f, 0f, 1f, 0.19f);
            var caption = Label(shade.transform, "مهرگان  •  نمای زنده شهر", 15,
                Color.white, TextAlignmentOptions.Center, true);
            Stretch(caption.rectTransform);

            var liveDot = Shape(viewport.transform, "Live city indicator",
                0.055f, 0.875f, 0.095f, 0.925f, new Color32(102, 225, 168, 255), true);
            AddFloat(liveDot, 1.5f, 1.6f);
        }

        void OnDestroy()
        {
            if (cityPreviewTexture != null)
            {
                Camera cityCamera = Camera.main;
                if (cityCamera != null && cityCamera.targetTexture == cityPreviewTexture)
                    cityCamera.targetTexture = null;

                cityPreviewTexture.Release();
                Destroy(cityPreviewTexture);
                cityPreviewTexture = null;
            }
        }

        void BuildProgressStrip()
        {
            var strip = Panel(safeRoot, "City progress strip", new Color32(255, 255, 255, 236),
                0.04f, 0.642f, 0.96f, 0.68f);

            stageLabel = Label(strip.transform, "پیشرفت شهر ۰ از ۳", 18, Teal, TextAlignmentOptions.Right, true);
            Place(stageLabel.rectTransform, 0.56f, 0.08f, 0.96f, 0.92f);

            var track = Panel(strip.transform, "Progress track", new Color32(225, 235, 241, 255),
                0.34f, 0.34f, 0.55f, 0.66f);
            var fillObject = MakeImage(track.transform, "Progress fill", RoundedSprite(), Teal);
            fillObject.type = Image.Type.Filled;
            fillObject.fillMethod = Image.FillMethod.Horizontal;
            fillObject.fillOrigin = (int)Image.OriginHorizontal.Left;
            Place(fillObject.rectTransform, 0f, 0f, 1f, 1f);
            progressFill = fillObject;

            restoreButton = Button(strip.transform, "بازسازی  •  ۱۰ سکه", new Color32(255, 235, 190, 255),
                new Color32(122, 78, 24, 255), 16, true);
            Place(restoreButton.GetComponent<RectTransform>(), 0.035f, 0.05f, 0.31f, 0.95f);
            restoreButton.onClick.AddListener(RestoreCity);
            restoreButton.gameObject.SetActive(false);

            nextButton = Button(strip.transform, "مرحله بعد  ←", Teal, Color.white, 16, true);
            Place(nextButton.GetComponent<RectTransform>(), 0.035f, 0.05f, 0.31f, 0.95f);
            nextButton.onClick.AddListener(GoToNextPuzzle);
            nextButton.gameObject.SetActive(false);
        }

        void BuildStatusBanner()
        {
            var statusSurface = Panel(safeRoot, "Status banner", new Color32(255, 255, 255, 195),
                0.07f, 0.132f, 0.93f, 0.193f);
            statusLabel = Label(statusSurface.transform, "سرنخ را بخوان و حروف را بچین.", 17,
                MutedInk, TextAlignmentOptions.Center);
            Stretch(statusLabel.rectTransform);
        }

        void BuildPuzzleCard()
        {
            var puzzle = Panel(safeRoot, "Puzzle card", Paper, 0.04f, 0.205f, 0.96f, 0.632f);
            AddShadow(puzzle, 0.12f, -6f);

            var heading = Label(puzzle.transform, "معمای امروز", 27, Ink, TextAlignmentOptions.Right, true);
            Place(heading.rectTransform, 0.51f, 0.87f, 0.94f, 0.98f);

            levelLabel = Label(puzzle.transform, "مرحله ۱", 20, Teal, TextAlignmentOptions.Left, true);
            Place(levelLabel.rectTransform, 0.06f, 0.87f, 0.47f, 0.98f);

            var clueTag = Label(puzzle.transform, "سرنخ", 15, MutedInk, TextAlignmentOptions.Right);
            Place(clueTag.rectTransform, 0.68f, 0.76f, 0.92f, 0.85f);

            var clueCard = Panel(puzzle.transform, "Clue surface", new Color32(239, 246, 251, 255), 0.055f, 0.61f, 0.945f, 0.77f);
            clueLabel = Label(clueCard.transform, "", 24, Ink, TextAlignmentOptions.Right);
            Place(clueLabel.rectTransform, 0.055f, 0.04f, 0.945f, 0.96f);

            var answerCard = Panel(puzzle.transform, "Answer surface", new Color32(232, 244, 250, 255), 0.12f, 0.44f, 0.88f, 0.57f);
            answerLabel = Label(answerCard.transform, "حروف را انتخاب کن", 31, Blue, TextAlignmentOptions.Center, true);
            Stretch(answerLabel.rectTransform);

            var tray = Panel(puzzle.transform, "Letter tray", new Color32(247, 249, 252, 255), 0.055f, 0.205f, 0.945f, 0.405f);
            tileTray = new GameObject("Letter tiles", typeof(RectTransform)).transform;
            tileTray.SetParent(tray.transform, false);
            Place(tileTray.GetComponent<RectTransform>(), 0.035f, 0.08f, 0.965f, 0.92f);

            hintButton = Button(puzzle.transform, "راهنمایی  •  ۳ سکه", new Color32(255, 242, 211, 255),
                new Color32(127, 86, 28, 255), 18);
            Place(hintButton.GetComponent<RectTransform>(), 0.055f, 0.045f, 0.36f, 0.17f);
            hintButton.onClick.AddListener(UseHint);

            clearButton = Button(puzzle.transform, "پاک کردن", new Color32(235, 241, 247, 255), MutedInk, 18);
            Place(clearButton.GetComponent<RectTransform>(), 0.385f, 0.045f, 0.62f, 0.17f);
            clearButton.onClick.AddListener(ClearTiles);

            submitButton = Button(puzzle.transform, "بررسی پاسخ", Blue, Color.white, 20, true);
            Place(submitButton.GetComponent<RectTransform>(), 0.645f, 0.045f, 0.945f, 0.17f);
            submitButton.onClick.AddListener(CheckAnswer);
        }

        void BuildBottomNavigation()
        {
            var nav = Panel(safeRoot, "Bottom navigation", new Color32(255, 255, 255, 246), 0.07f, 0.045f, 0.93f, 0.125f);
            AddShadow(nav, 0.10f, -4f);

            var help = Button(nav.transform, "راهنما", new Color32(242, 246, 250, 255), MutedInk, 17);
            Place(help.GetComponent<RectTransform>(), 0.035f, 0.16f, 0.30f, 0.84f);
            help.onClick.AddListener(OpenHelp);

            cityButton = Button(nav.transform, "شهر مهرگان", new Color32(224, 243, 239, 255), Teal, 17, true);
            Place(cityButton.GetComponent<RectTransform>(), 0.335f, 0.16f, 0.665f, 0.84f);
            cityButton.onClick.AddListener(OpenCityShop);

            var progressButton = Button(nav.transform, "پیشرفت من", new Color32(242, 246, 250, 255), MutedInk, 17);
            Place(progressButton.GetComponent<RectTransform>(), 0.70f, 0.16f, 0.965f, 0.84f);
            progressButton.onClick.AddListener(OpenProgress);
        }

        void BuildCityShopPanel()
        {
            cityShopOverlay = FullScreenOverlay("City shop overlay");
            var card = Panel(cityShopOverlay.transform, "City shop card", new Color32(250, 252, 254, 255),
                0.045f, 0.12f, 0.955f, 0.88f);
            AddShadow(card, 0.22f, -8f);

            var title = Label(card.transform, "بازار بازسازی مهرگان", 29, Ink, TextAlignmentOptions.Right, true);
            Place(title.rectTransform, 0.22f, 0.88f, 0.94f, 0.98f);

            var close = Button(card.transform, "بستن", new Color32(235, 241, 247, 255), MutedInk, 17);
            Place(close.GetComponent<RectTransform>(), 0.045f, 0.89f, 0.205f, 0.98f);
            close.onClick.AddListener(() => cityShopOverlay.SetActive(false));

            cityShopBalance = Label(card.transform, "", 21, new Color32(134, 92, 27, 255), TextAlignmentOptions.Right, true);
            Place(cityShopBalance.rectTransform, 0.06f, 0.80f, 0.94f, 0.88f);

            var listSurface = Panel(card.transform, "Projects surface", new Color32(241, 246, 250, 255),
                0.045f, 0.055f, 0.955f, 0.775f);
            for (int i = 0; i < RewardEconomy.CityProjects.Length; i++)
            {
                CityProjectDefinition project = RewardEconomy.CityProjects[i];
                float top = 0.975f - i * 0.122f;
                float bottom = top - 0.105f;
                var button = Button(listSurface.transform, "", new Color32(255, 255, 255, 255), Ink, 17);
                Place(button.GetComponent<RectTransform>(), 0.025f, bottom, 0.975f, top);
                button.onClick.AddListener(() => PurchaseCityProject(project.Id));
                cityProjectButtons.Add(button);
                cityProjectLabels.Add(button.GetComponentInChildren<RTLTextMeshPro>());
            }

            cityShopOverlay.SetActive(false);
        }

        GameObject FullScreenOverlay(string objectName)
        {
            var overlay = new GameObject(objectName, typeof(RectTransform), typeof(Image));
            overlay.transform.SetParent(safeRoot, false);
            Stretch(overlay.GetComponent<RectTransform>());
            var image = overlay.GetComponent<Image>();
            image.sprite = WhiteSprite();
            image.color = new Color(8f / 255f, 25f / 255f, 42f / 255f, 0.78f);
            return overlay;
        }

        void OpenCityShop()
        {
            RefreshCityShop();
            cityShopOverlay.SetActive(true);
            cityShopOverlay.transform.SetAsLastSibling();
            AnimatePanel(cityShopOverlay.transform);
        }

        void RefreshCityShop()
        {
            if (cityShopBalance != null)
                cityShopBalance.text = "موجودی تو: " + PersianNumber(progress.Coins) + " سکه";

            for (int i = 0; i < cityProjectButtons.Count && i < RewardEconomy.CityProjects.Length; i++)
            {
                CityProjectDefinition project = RewardEconomy.CityProjects[i];
                bool owned = Array.IndexOf(progress.OwnedCityProjectIds ?? Array.Empty<string>(), project.Id) >= 0;
                cityProjectLabels[i].text = owned
                    ? project.DisplayName + "  •  بازسازی شده"
                    : project.DisplayName + "  •  " + PersianNumber(project.CoinCost) + " سکه";
                cityProjectButtons[i].interactable = !owned && progress.Coins >= project.CoinCost;
                var colors = cityProjectButtons[i].colors;
                colors.normalColor = owned ? new Color32(215, 240, 224, 255) : new Color32(255, 255, 255, 255);
                cityProjectButtons[i].colors = colors;
            }
        }

        void PurchaseCityProject(string projectId)
        {
            var candidate = new SaveGameData
            {
                SchemaVersion = 3,
                LevelIndex = progress.LevelIndex,
                SoftCurrency = progress.Coins,
                RestorationStage = progress.RestorationStage,
                SolvedLevels = progress.SolvedLevels,
                CurrentLevelSolved = progress.CurrentLevelSolved,
                CurrentLevelRestored = progress.CurrentLevelRestored,
                LastCompletedPuzzleId = progress.LastCompletedPuzzleId,
                AppliedTransactionIds = progress.AppliedTransactionIds,
                OwnedCityProjectIds = progress.OwnedCityProjectIds
            };

            if (!CityProjectSystem.TryPurchase(candidate, projectId, out string error))
            {
                SetStatus(error, false);
                RefreshCityShop();
                return;
            }

            var newProgress = new PlayerProgress
            {
                SchemaVersion = 3,
                LevelIndex = progress.LevelIndex,
                Coins = candidate.SoftCurrency,
                RestorationStage = progress.RestorationStage,
                SolvedLevels = progress.SolvedLevels,
                CurrentLevelSolved = progress.CurrentLevelSolved,
                CurrentLevelRestored = progress.CurrentLevelRestored,
                LastCompletedPuzzleId = progress.LastCompletedPuzzleId,
                AppliedTransactionIds = candidate.AppliedTransactionIds,
                OwnedCityProjectIds = candidate.OwnedCityProjectIds
            };

            if (!SaveSystem.Store(newProgress))
            {
                SetStatus("ذخیره انجام نشد؛ سکه‌ها کم نشدند.", false);
                return;
            }

            progress = newProgress;
            MehrganWorldBuilder.ApplyCityProjects(progress.OwnedCityProjectIds);
            Refresh();
            RefreshCityShop();
            SetStatus("بخش تازه‌ای از مهرگان جان گرفت!", true);
        }

        void Refresh()
        {
            PuzzleLevel level = WordPuzzle.Levels[progress.LevelIndex];
            coinsLabel.text = PersianNumber(progress.Coins) + " سکه";
            levelLabel.text = "مرحله " + PersianNumber(progress.LevelIndex + 1) + " از " + PersianNumber(WordPuzzle.Levels.Length);
            clueLabel.text = level.Clue;
            answerLabel.text = pickedTiles.Count == 0 ? "حروف را به ترتیب انتخاب کن" : JoinPickedTiles();
            stageLabel.text = "پیشرفت شهر  " + PersianNumber(progress.RestorationStage) + " از ۳";
            progressFill.fillAmount = progress.SolvedLevels / (float)WordPuzzle.Levels.Length;

            bool solved = solvedLevel == progress.LevelIndex;
            bool milestone = WordPuzzle.RequiresRestoration(progress.LevelIndex);
            submitButton.interactable = !solved;
            hintButton.interactable = !solved && progress.Coins >= 3;
            stageLabel.text = "پیشرفت شهر  " + PersianNumber(progress.RestorationStage) + " از ۳";
            restoreButton.gameObject.SetActive(solved && milestone && !restored);
            restoreButton.interactable = progress.Coins >= 10;
            nextButton.gameObject.SetActive(solved && restored && progress.LevelIndex < WordPuzzle.Levels.Length - 1);
            BuildTiles(level);
            RefreshCityShop();
            if (solved)
                SetStatus(restored ? "آفرین! آماده‌ای برای ماجرای بعدی؟" : "آفرین! حالا بخش تازه‌ای از شهر را بازسازی کن.", true);
            else
                SetStatus("سرنخ را بخوان، حروف را بچین و پاسخ را پیدا کن.", false);
        }

        void BuildTiles(PuzzleLevel level)
        {
            foreach (Transform child in tileTray)
                Destroy(child.gameObject);
            tileButtons.Clear();

            int count = Mathf.Max(1, level.Letters.Length);
            float gap = count >= 7 ? 0.018f : 0.025f;
            float width = (0.92f - gap * (count - 1)) / count;

            for (int i = 0; i < count; i++)
            {
                int tileIndex = i;
                // Right-to-left tile order; selection order remains the authored answer order.
                float right = 0.96f - i * (width + gap);
                float left = right - width;
                Color tileColor = i % 2 == 0
                    ? new Color32(225, 241, 249, 255)
                    : new Color32(237, 244, 250, 255);

                var button = Button(tileTray, level.Letters[i], tileColor, Ink, count >= 7 ? 24 : 29, true);
                Place(button.GetComponent<RectTransform>(), left, 0.02f, right, 0.98f);
                button.onClick.AddListener(() => PickTile(tileIndex));
                tileButtons.Add(button);
            }
        }

        void PickTile(int index)
        {
            if (solvedLevel == progress.LevelIndex || pickedTiles.Count >= WordPuzzle.Levels[progress.LevelIndex].Letters.Length)
                return;
            if (pickedTiles.Contains(index))
                return;

            pickedTiles.Add(index);
            if (index >= 0 && index < tileButtons.Count)
                tileButtons[index].interactable = false;
            answerLabel.text = JoinPickedTiles();
            Pop(answerLabel.transform);
        }

        string JoinPickedTiles()
        {
            var result = string.Empty;
            for (int i = 0; i < pickedTiles.Count; i++)
                result += WordPuzzle.Levels[progress.LevelIndex].Letters[pickedTiles[i]];
            return result;
        }

        void ClearTiles()
        {
            pickedTiles.Clear();
            for (int i = 0; i < tileButtons.Count; i++)
                tileButtons[i].interactable = true;
            answerLabel.text = "حروف را به ترتیب انتخاب کن";
        }

        void CheckAnswer()
        {
            PuzzleLevel level = WordPuzzle.Levels[progress.LevelIndex];
            if (!WordPuzzle.IsCorrect(level, JoinPickedTiles()))
            {
                SetStatus("این واژه درست نیست؛ یک بار دیگه امتحان کن.", false);
                Shake(answerLabel.transform);
                return;
            }

            PlayerProgress before = CloneProgress(progress);
            solvedLevel = progress.LevelIndex;
            progress.CurrentLevelSolved = true;
            progress.SolvedLevels = Mathf.Max(progress.SolvedLevels, progress.LevelIndex + 1);

            int reward = RewardEconomy.PuzzleReward(level.Difficulty);
            bool rewarded = ApplyCurrency("puzzle:" + level.Id + ":solve", reward);
            progress.LastCompletedPuzzleId = "word-" + level.Id;
            restored = !WordPuzzle.RequiresRestoration(progress.LevelIndex);
            progress.CurrentLevelRestored = restored;

            if (!SaveSystem.Store(progress))
            {
                progress = before;
                solvedLevel = -1;
                restored = false;
                Refresh();
                SetStatus("ذخیره انجام نشد؛ پاسخ دوباره ثبت نشده است.", false);
                return;
            }

            Refresh();
            SetStatus(rewarded ? "درست پیدا کردی! " + PersianNumber(reward) + " سکه گرفتی." : "این پاسخ قبلاً ثبت شده بود.", true);
            Pop(coinsLabel.transform);
            SpawnConfetti();
        }

        void UseHint()
        {
            if (progress.Coins < 3 || solvedLevel == progress.LevelIndex)
                return;

            PlayerProgress before = CloneProgress(progress);
            if (!ApplyCurrency("hint:" + progress.LevelIndex + ":" + Guid.NewGuid().ToString("N"), -3))
                return;

            if (!SaveSystem.Store(progress))
            {
                progress = before;
                Refresh();
                SetStatus("ذخیره راهنمایی ناموفق بود؛ سکه‌ای کم نشد.", false);
                return;
            }

            coinsLabel.text = PersianNumber(progress.Coins) + " سکه";
            hintButton.interactable = progress.Coins >= 3;
            string firstLetter = WordPuzzle.Levels[progress.LevelIndex].Answer.Substring(0, 1);
            SetStatus("راهنمایی: واژه با حرف «" + firstLetter + "» شروع می‌شود.", true);
            Pop(hintButton.transform);
        }

        void RestoreCity()
        {
            if (solvedLevel != progress.LevelIndex || restored ||
                !WordPuzzle.RequiresRestoration(progress.LevelIndex) || progress.Coins < 10)
                return;

            PlayerProgress before = CloneProgress(progress);
            int newStage = Mathf.Min(3, progress.RestorationStage + 1);
            if (!ApplyCurrency("restore:" + progress.LevelIndex + ":" + newStage, -10))
                return;

            progress.RestorationStage = newStage;
            restored = true;
            progress.CurrentLevelRestored = true;

            if (!SaveSystem.Store(progress))
            {
                progress = before;
                restored = progress.CurrentLevelRestored;
                MehrganWorldBuilder.ApplyRestorationStage(progress.RestorationStage);
                Refresh();
                SetStatus("ذخیره بازسازی ناموفق بود؛ سکه‌ای کم نشد.", false);
                return;
            }

            MehrganWorldBuilder.ApplyRestorationStage(progress.RestorationStage);
            Refresh();
            SetStatus("یک قدم دیگر، مهرگان زیباتر شد!", true);
            SpawnConfetti();
        }

        void GoToNextPuzzle()
        {
            if (solvedLevel != progress.LevelIndex || !restored ||
                progress.LevelIndex >= WordPuzzle.Levels.Length - 1)
                return;

            PlayerProgress before = CloneProgress(progress);
            progress.LevelIndex++;
            progress.CurrentLevelSolved = false;
            progress.CurrentLevelRestored = false;

            if (!SaveSystem.Store(progress))
            {
                progress = before;
                Refresh();
                SetStatus("ذخیره مرحله بعد انجام نشد؛ دوباره تلاش کن.", false);
                return;
            }

            solvedLevel = -1;
            restored = false;
            pickedTiles.Clear();
            Refresh();
            AnimatePanel(safeRoot);
        }

        bool ApplyCurrency(string transactionId, int delta)
        {
            var state = new RewardLedgerState {
                SoftCurrency = progress.Coins,
                AppliedTransactionIds = progress.AppliedTransactionIds
            };
            bool applied = RewardLedger.TryApply(state, transactionId, delta);
            if (applied)
            {
                progress.Coins = state.SoftCurrency;
                progress.AppliedTransactionIds = state.AppliedTransactionIds;
            }
            return applied;
        }

        static PlayerProgress CloneProgress(PlayerProgress source)
        {
            return new PlayerProgress {
                SchemaVersion = source.SchemaVersion,
                LevelIndex = source.LevelIndex,
                Coins = source.Coins,
                RestorationStage = source.RestorationStage,
                SolvedLevels = source.SolvedLevels,
                CurrentLevelSolved = source.CurrentLevelSolved,
                CurrentLevelRestored = source.CurrentLevelRestored,
                LastCompletedPuzzleId = source.LastCompletedPuzzleId,
                AppliedTransactionIds = (string[])(source.AppliedTransactionIds ?? Array.Empty<string>()).Clone(),
                OwnedCityProjectIds = (string[])(source.OwnedCityProjectIds ?? Array.Empty<string>()).Clone()
            };
        }

        void OpenHelp()
        {
            ShowInfo("چطور بازی کنیم؟",
                "۱. سرنخ هر مرحله را بخوان.\n۲. حروف را از راست به چپ انتخاب کن.\n۳. پاسخ درست سکه می‌دهد.\n۴. با سکه‌ها بخش‌های شهر مهرگان را بازسازی کن.\n۵. برای واژه‌های سخت از راهنمایی استفاده کن.");
        }

        void OpenProgress()
        {
            string body = "مرحله فعلی: " + PersianNumber(progress.LevelIndex + 1) + "\n" +
                "معماهای حل‌شده: " + PersianNumber(progress.SolvedLevels) + " از " + PersianNumber(WordPuzzle.Levels.Length) + "\n" +
                "سکه‌ها: " + PersianNumber(progress.Coins) + "\n" +
                "گام‌های بازسازی: " + PersianNumber(progress.RestorationStage) + " از ۳";
            ShowInfo("گزارش ماجراجویی تو", body);
        }

        void ShowInfo(string title, string body)
        {
            if (infoOverlay != null)
                Destroy(infoOverlay);

            infoOverlay = FullScreenOverlay("Info overlay");
            var card = Panel(infoOverlay.transform, "Info card", new Color32(250, 252, 254, 255),
                0.055f, 0.27f, 0.945f, 0.73f);
            AddShadow(card, 0.22f, -8f);

            var heading = Label(card.transform, title, 28, Ink, TextAlignmentOptions.Right, true);
            Place(heading.rectTransform, 0.07f, 0.80f, 0.93f, 0.96f);
            var text = Label(card.transform, body, 21, MutedInk, TextAlignmentOptions.Right);
            Place(text.rectTransform, 0.08f, 0.23f, 0.92f, 0.78f);

            var close = Button(card.transform, "متوجه شدم", Teal, Color.white, 19, true);
            Place(close.GetComponent<RectTransform>(), 0.28f, 0.055f, 0.72f, 0.19f);
            close.onClick.AddListener(() => Destroy(infoOverlay));
            AnimatePanel(infoOverlay.transform);
        }

        void SetStatus(string message, bool positive)
        {
            if (statusLabel == null)
                return;
            statusLabel.text = message;
            statusLabel.color = positive ? Teal : MutedInk;
        }

        void BuildStartupError(Exception exception)
        {
            if (safeRoot == null)
            {
                var canvasObject = new GameObject("Charso Startup Diagnostics", typeof(Canvas), typeof(CanvasScaler));
                var canvas = canvasObject.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 500;
                var scaler = canvasObject.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1080, 1920);
                safeRoot = new GameObject("Safe Area", typeof(RectTransform)).GetComponent<RectTransform>();
                safeRoot.SetParent(canvasObject.transform, false);
                Stretch(safeRoot);
            }

            var back = Panel(safeRoot, "Startup error", new Color32(250, 252, 254, 255), 0.06f, 0.18f, 0.94f, 0.82f);
            var title = Label(back.transform, "چهارسو نتوانست شروع شود", 29, Coral, TextAlignmentOptions.Center, true);
            Place(title.rectTransform, 0.06f, 0.79f, 0.94f, 0.96f);
            var details = Label(back.transform, exception.GetType().Name + "\n" + exception.Message,
                18, Ink, TextAlignmentOptions.Left);
            Place(details.rectTransform, 0.06f, 0.08f, 0.94f, 0.76f);
        }

        void SpawnConfetti()
        {
            var burst = safeRoot.gameObject.AddComponent<CharsoConfettiBurst>();
            burst.Play(RoundedSprite(), CircleSprite());
        }

        static void AddEntranceAnimation(GameObject canvasObject)
        {
            var group = canvasObject.GetComponent<CanvasGroup>();
            group.alpha = 0f;
            canvasObject.AddComponent<CharsoCanvasEntrance>();
        }

        static void AnimatePanel(Transform target)
        {
            var motion = target.GetComponent<CharsoPanelMotion>();
            if (motion == null)
                motion = target.gameObject.AddComponent<CharsoPanelMotion>();
            motion.Play();
        }

        static void Pop(Transform target)
        {
            var motion = target.GetComponent<CharsoPanelMotion>();
            if (motion == null)
                motion = target.gameObject.AddComponent<CharsoPanelMotion>();
            motion.Play(1.08f);
        }

        static void Shake(Transform target)
        {
            var motion = target.GetComponent<CharsoPanelMotion>();
            if (motion == null)
                motion = target.gameObject.AddComponent<CharsoPanelMotion>();
            motion.Shake();
        }

        static void AddFloat(RectTransform rect, float height, float speed)
        {
            var motion = rect.gameObject.AddComponent<CharsoFloatMotion>();
            motion.height = height;
            motion.speed = speed;
        }

        static string PersianNumber(int value)
        {
            return value.ToString().Replace('0', '۰').Replace('1', '۱').Replace('2', '۲')
                .Replace('3', '۳').Replace('4', '۴').Replace('5', '۵')
                .Replace('6', '۶').Replace('7', '۷').Replace('8', '۸').Replace('9', '۹');
        }

        static RTLTextMeshPro Label(Transform parent, string value, float size, Color color,
            TextAlignmentOptions alignment, bool bold = false)
        {
            var go = new GameObject("RTL Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(RTLTextMeshPro));
            go.transform.SetParent(parent, false);
            var text = go.GetComponent<RTLTextMeshPro>();
            text.font = ResolvePersianFont();
            text.fontSize = size;
            text.color = color;
            text.alignment = alignment;
            text.enableAutoSizing = true;
            text.fontSizeMin = Mathf.Max(12, size * 0.72f);
            text.fontSizeMax = size;
            text.enableWordWrapping = true;
            text.overflowMode = TextOverflowModes.Ellipsis;
            text.raycastTarget = false;
            text.Farsi = true;
            text.PreserveNumbers = true;
            text.ForceFix = false;
            text.FixTags = true;
            if (bold)
                text.fontStyle = FontStyles.Bold;
            text.text = value;
            return text;
        }

        static Button Button(Transform parent, string label, Color background, Color foreground,
            float size, bool bold = false)
        {
            var go = new GameObject("Action button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);

            var image = go.GetComponent<Image>();
            image.sprite = RoundedSprite();
            image.type = Image.Type.Sliced;
            image.color = background;

            var button = go.GetComponent<Button>();
            button.targetGraphic = image;
            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color32(235, 246, 252, 255);
            colors.pressedColor = new Color32(215, 230, 241, 255);
            colors.selectedColor = Color.white;
            colors.disabledColor = new Color32(205, 214, 222, 170);
            colors.fadeDuration = 0.08f;
            button.colors = colors;

            var text = Label(go.transform, label, size, foreground, TextAlignmentOptions.Center, bold);
            Place(text.rectTransform, 0.06f, 0.06f, 0.94f, 0.94f);
            go.AddComponent<CharsoButtonMotion>();

            var shadow = go.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0.12f, 0.22f, 0.12f);
            shadow.effectDistance = new Vector2(0f, -2f);
            return button;
        }

        static GameObject Panel(Transform parent, string name, Color color,
            float x0, float y0, float x1, float y1)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            Place(go.GetComponent<RectTransform>(), x0, y0, x1, y1);
            var image = go.GetComponent<Image>();
            image.sprite = RoundedSprite();
            image.type = Image.Type.Sliced;
            image.color = color;
            image.raycastTarget = false;
            return go;
        }

        static RectTransform Shape(Transform parent, string name, float x0, float y0, float x1,
            float y1, Color color, bool circle)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            Place(rect, x0, y0, x1, y1);
            var image = go.GetComponent<Image>();
            image.sprite = circle ? CircleSprite() : RoundedSprite();
            image.type = circle ? Image.Type.Simple : Image.Type.Sliced;
            image.color = color;
            image.raycastTarget = false;
            return rect;
        }

        static Image MakeImage(Transform parent, string name, Sprite sprite, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color;
            return image;
        }

        static void AddShadow(GameObject target, float alpha, float distance)
        {
            var shadow = target.AddComponent<Shadow>();
            shadow.effectColor = new Color(0.05f, 0.12f, 0.18f, alpha);
            shadow.effectDistance = new Vector2(0f, distance);
        }

        static void EnsureEventSystem()
        {
            if (EventSystem.current != null)
                return;
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        static void Place(RectTransform rect, float x0, float y0, float x1, float y1)
        {
            rect.anchorMin = new Vector2(x0, y0);
            rect.anchorMax = new Vector2(x1, y1);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;
        }

        static void Stretch(RectTransform rect)
        {
            Place(rect, 0f, 0f, 1f, 1f);
        }

        static Sprite RoundedSprite()
        {
            if (roundedSprite != null)
                return roundedSprite;

            const int size = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.name = "Charso rounded UI sprite";
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = Mathf.Min(x, size - 1 - x);
                float dy = Mathf.Min(y, size - 1 - y);
                float cornerX = Mathf.Max(0f, 14f - dx);
                float cornerY = Mathf.Max(0f, 14f - dy);
                float outside = Mathf.Sqrt(cornerX * cornerX + cornerY * cornerY);
                byte alpha = (byte)Mathf.Clamp(Mathf.RoundToInt((14f - outside + 1f) * 255f), 0, 255);
                texture.SetPixel(x, y, new Color32(255, 255, 255, alpha));
            }
            texture.Apply();
            roundedSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f),
                size, 0, SpriteMeshType.FullRect, new Vector4(18, 18, 18, 18));
            return roundedSprite;
        }

        static Sprite CircleSprite()
        {
            if (circleSprite != null)
                return circleSprite;

            const int size = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.name = "Charso circle UI sprite";
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            Vector2 center = new Vector2(31.5f, 31.5f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                byte alpha = (byte)Mathf.Clamp(Mathf.RoundToInt((32f - distance) * 255f), 0, 255);
                texture.SetPixel(x, y, new Color32(255, 255, 255, alpha));
            }
            texture.Apply();
            circleSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            return circleSprite;
        }

        static Sprite WhiteSprite()
        {
            if (whiteSprite != null)
                return whiteSprite;
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.SetPixels(new[] { Color.white, Color.white, Color.white, Color.white });
            texture.Apply();
            whiteSprite = Sprite.Create(texture, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 2);
            return whiteSprite;
        }

        static Sprite BackgroundSprite()
        {
            if (backgroundSprite != null)
                return backgroundSprite;
            const int width = 4;
            const int height = 128;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.name = "Charso calm blue background";
            texture.filterMode = FilterMode.Bilinear;
            for (int y = 0; y < height; y++)
            {
                float t = y / (float)(height - 1);
                Color color = Color.Lerp(new Color32(242, 247, 252, 255), new Color32(226, 239, 249, 255), t);
                for (int x = 0; x < width; x++)
                    texture.SetPixel(x, y, color);
            }
            texture.Apply();
            backgroundSprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 128);
            return backgroundSprite;
        }
    }

    public sealed class CharsoFloatMotion : MonoBehaviour
    {
        public float height = 4f;
        public float speed = 1.5f;
        RectTransform rect;
        Vector2 origin;
        float phase;

        void Awake()
        {
            rect = transform as RectTransform;
            if (rect != null)
                origin = rect.anchoredPosition;
            phase = UnityEngine.Random.Range(0f, 6.28f);
        }

        void Update()
        {
            if (rect == null)
                return;
            float offset = Mathf.Sin(Time.unscaledTime * speed + phase) * height;
            rect.anchoredPosition = origin + new Vector2(0f, offset);
        }
    }

    public sealed class CharsoButtonMotion : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        Vector3 targetScale = Vector3.one;
        void Awake() { targetScale = transform.localScale; }
        void Update() { transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * 18f); }
        public void OnPointerDown(PointerEventData eventData) { targetScale = Vector3.one * 0.95f; }
        public void OnPointerUp(PointerEventData eventData) { targetScale = Vector3.one; }
        public void OnPointerExit(PointerEventData eventData) { targetScale = Vector3.one; }
    }

    public sealed class CharsoCanvasEntrance : MonoBehaviour
    {
        CanvasGroup group;
        float elapsed;
        void Awake() { group = GetComponent<CanvasGroup>(); transform.localScale = Vector3.one * 0.985f; }
        void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / 0.38f);
            group.alpha = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = Vector3.Lerp(Vector3.one * 0.985f, Vector3.one, Mathf.SmoothStep(0f, 1f, t));
            if (t >= 1f) enabled = false;
        }
    }

    public sealed class CharsoPanelMotion : MonoBehaviour
    {
        Vector3 originalScale;
        Vector3 originalPosition;
        float elapsed;
        float duration;
        float peak = 1.06f;
        bool shaking;
        bool running;

        void Awake()
        {
            originalScale = transform.localScale;
            originalPosition = transform.localPosition;
        }

        public void Play(float scalePeak = 1.06f)
        {
            if (originalScale == Vector3.zero)
                originalScale = transform.localScale;
            peak = scalePeak;
            elapsed = 0f;
            duration = 0.26f;
            shaking = false;
            running = true;
        }

        public void Shake()
        {
            if (originalPosition == Vector3.zero)
                originalPosition = transform.localPosition;
            elapsed = 0f;
            duration = 0.30f;
            shaking = true;
            running = true;
        }

        void Update()
        {
            if (!running)
                return;
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, duration));
            if (shaking)
            {
                float strength = Mathf.Sin(t * Mathf.PI * 7f) * (1f - t) * 10f;
                transform.localPosition = originalPosition + new Vector3(strength, 0f, 0f);
            }
            else
            {
                float pulse = Mathf.Sin(t * Mathf.PI);
                transform.localScale = originalScale * (1f + (peak - 1f) * pulse);
            }
            if (t >= 1f)
            {
                transform.localScale = originalScale;
                transform.localPosition = originalPosition;
                running = false;
            }
        }
    }

    public sealed class CharsoConfettiBurst : MonoBehaviour
    {
        sealed class Piece
        {
            public RectTransform rect;
            public Vector2 velocity;
            public float rotation;
            public float rotationSpeed;
        }

        readonly List<Piece> pieces = new List<Piece>();
        float elapsed;

        public void Play(Sprite rounded, Sprite circle)
        {
            elapsed = 0f;
            for (int i = 0; i < 22; i++)
            {
                var go = new GameObject("Celebration particle", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                go.transform.SetParent(transform, false);
                var rect = go.GetComponent<RectTransform>();
                float size = UnityEngine.Random.Range(10f, 22f);
                rect.sizeDelta = new Vector2(size, size * UnityEngine.Random.Range(0.65f, 1.4f));
                rect.anchorMin = new Vector2(0.5f, 0.54f);
                rect.anchorMax = rect.anchorMin;
                rect.anchoredPosition = Vector2.zero;

                var image = go.GetComponent<Image>();
                image.sprite = i % 3 == 0 ? circle : rounded;
                image.color = new[] {
                    new Color32(248, 190, 81, 255), new Color32(39, 157, 153, 255),
                    new Color32(92, 151, 214, 255), new Color32(241, 129, 102, 255)
                }[i % 4];
                image.raycastTarget = false;

                pieces.Add(new Piece {
                    rect = rect,
                    velocity = new Vector2(UnityEngine.Random.Range(-220f, 220f), UnityEngine.Random.Range(160f, 420f)),
                    rotation = UnityEngine.Random.Range(-80f, 80f),
                    rotationSpeed = UnityEngine.Random.Range(-300f, 300f)
                });
            }
        }

        void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            for (int i = pieces.Count - 1; i >= 0; i--)
            {
                Piece piece = pieces[i];
                if (piece.rect == null)
                {
                    pieces.RemoveAt(i);
                    continue;
                }

                piece.velocity += Vector2.down * (620f * Time.unscaledDeltaTime);
                piece.rect.anchoredPosition += piece.velocity * Time.unscaledDeltaTime;
                piece.rotation += piece.rotationSpeed * Time.unscaledDeltaTime;
                piece.rect.localRotation = Quaternion.Euler(0f, 0f, piece.rotation);

                var image = piece.rect.GetComponent<Image>();
                Color color = image.color;
                color.a = Mathf.Clamp01(1f - elapsed / 1.35f);
                image.color = color;

                if (elapsed >= 1.35f)
                    Destroy(piece.rect.gameObject);
            }

            if (elapsed >= 1.5f)
                Destroy(this);
        }
    }
}
