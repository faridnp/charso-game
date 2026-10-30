using UnityEngine;
using UnityEngine.EventSystems;

namespace Charso
{
    /// <summary>Touch and mouse exploration controls for the isometric Mehrgan overview.</summary>
    [RequireComponent(typeof(Camera))]
    public sealed class MehrganCameraController : MonoBehaviour
    {
        [SerializeField] float dragSensitivity=.035f;
        [SerializeField] float zoomSensitivity=1.8f;
        [SerializeField] float minZoom=20f;
        [SerializeField] float maxZoom=43f;
        Camera cam;
        Vector2 lastPointer;
        bool dragging;

        void Awake(){cam=GetComponent<Camera>();}
        void Update()
        {
            if(cam==null||!cam.orthographic)return;
            HandleTouch();
            HandleMouse();
            cam.orthographicSize=Mathf.Clamp(cam.orthographicSize, minZoom,maxZoom);
            Vector3 p=transform.position;
            p.x=Mathf.Clamp(p.x,-15f,50f);
            p.z=Mathf.Clamp(p.z,-55f,20f);
            transform.position=p;
        }

        void HandleTouch()
        {
            if(Input.touchCount>=2)
            {
                dragging=false;
                Touch a=Input.GetTouch(0),b=Input.GetTouch(1);
                Vector2 aPrev=a.position-a.deltaPosition,bPrev=b.position-b.deltaPosition;
                float before=Vector2.Distance(aPrev,bPrev),after=Vector2.Distance(a.position,b.position);
                cam.orthographicSize-=((after-before)/Mathf.Max(1,Screen.dpi))*zoomSensitivity*18f;
                return;
            }
            if(Input.touchCount!=1){dragging=false;return;}
            Touch t=Input.GetTouch(0);
            if(t.phase==TouchPhase.Began)
            {
                dragging=!IsOverUI(t.fingerId);
                lastPointer=t.position;
            }
            else if(dragging&&(t.phase==TouchPhase.Moved||t.phase==TouchPhase.Stationary))
            {
                Pan(t.position-lastPointer);
                lastPointer=t.position;
            }
            else if(t.phase==TouchPhase.Ended||t.phase==TouchPhase.Canceled)dragging=false;
        }

        void HandleMouse()
        {
            if(Input.touchCount>0)return;
            if(Input.GetMouseButtonDown(1)||Input.GetMouseButtonDown(2))
            {
                dragging=!IsOverUI(-1);
                lastPointer=Input.mousePosition;
            }
            if(dragging&&(Input.GetMouseButton(1)||Input.GetMouseButton(2)))
            {
                Vector2 now=Input.mousePosition;
                Pan(now-lastPointer);lastPointer=now;
            }
            if(Input.GetMouseButtonUp(1)||Input.GetMouseButtonUp(2))dragging=false;
            float wheel=Input.mouseScrollDelta.y;
            if(Mathf.Abs(wheel)>.01f&&!IsOverUI(-1))cam.orthographicSize-=wheel*zoomSensitivity;
        }

        bool IsOverUI(int pointerId)
        {
            return EventSystem.current!=null&&EventSystem.current.IsPointerOverGameObject(pointerId);
        }

        void Pan(Vector2 delta)
        {
            Vector3 right=transform.right;right.y=0;right.Normalize();
            Vector3 forward=transform.forward;forward.y=0;forward.Normalize();
            transform.position+=(-right*delta.x-forward*delta.y)*dragSensitivity;
        }
    }
}