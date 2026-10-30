using UnityEngine;

namespace Charso
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaFitter : MonoBehaviour
    {
        RectTransform rectTransform;
        Rect lastSafeArea;
        int lastWidth;
        int lastHeight;

        void Awake(){rectTransform=GetComponent<RectTransform>();Apply();}
        void Update()
        {
            if(lastSafeArea!=Screen.safeArea||lastWidth!=Screen.width||lastHeight!=Screen.height)Apply();
        }
        void Apply()
        {
            if(rectTransform==null)rectTransform=GetComponent<RectTransform>();
            Rect safe=Screen.safeArea;
            lastSafeArea=safe;lastWidth=Screen.width;lastHeight=Screen.height;
            Vector2 min=safe.position,max=safe.position+safe.size;
            min.x/=Mathf.Max(1,Screen.width);min.y/=Mathf.Max(1,Screen.height);
            max.x/=Mathf.Max(1,Screen.width);max.y/=Mathf.Max(1,Screen.height);
            rectTransform.anchorMin=min;rectTransform.anchorMax=max;
            rectTransform.offsetMin=Vector2.zero;rectTransform.offsetMax=Vector2.zero;
        }
    }
}