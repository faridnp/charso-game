using UnityEngine;

namespace Charso
{
    public sealed class HafeziehChapterSceneBootstrap : MonoBehaviour
    {
        [SerializeField,Range(0,3)] int reconstructionStage;
        void Awake(){Application.targetFrameRate=60;HafeziehChapterBuilder.Build(reconstructionStage);}
    }
}