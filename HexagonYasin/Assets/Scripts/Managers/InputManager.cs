using UnityEngine;
using System.Collections;

namespace YasinYuce.HexagonYasin
{
    public class InputManager : MonoBehaviour
    {
        public bool IsReadyForInput { get; set; }
        private bool isTurned;

        private SelectorManager mSelectorManager;
        private Coroutine mSwipeControlRoutine;

        public void Initialize(SelectorManager selectorManager)
        {
            mSelectorManager = selectorManager;
        }

        public void Update()
        {
            if (IsReadyForInput)
            {
                ReadInput();
            }
        }

        public void ReadInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnTouchBegin();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (isTurned) { isTurned = false; return; }
                OnTouchEnd();
            }
        }

        public void OnTouchBegin()
        {
            isTurned = false;
            StartSwipeControl();
        }

        public void OnTouchEnd()
        {
            StopSwipeControl();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D rayHit2d = Physics2D.Raycast(mousePos, Vector2.zero);
            if (rayHit2d.collider != null)
            {
                rayHit2d.collider.GetComponent<GridPiece>().SelectGroup(mousePos);
            }
            else
            {
                mSelectorManager.ResetSelectAction();
            }
        }

        public void StartSwipeControl()
        {
            if (mSelectorManager.CurrentSelectorObject == null)
            {
                return;
            }

            StopSwipeControl();
            mSwipeControlRoutine = StartCoroutine(WaitForSwipe());
        }

        public void StopSwipeControl()
        {
            if (mSwipeControlRoutine != null)
            {
                StopCoroutine(mSwipeControlRoutine);
                mSwipeControlRoutine = null;
            }
        }

        public IEnumerator WaitForSwipe()
        {
            float firstAngle = mSelectorManager.GiveAngleToCurrentSelector();
            yield return new WaitUntil(() => firstAngle.GetDifferenceBetweenTwoAngle(mSelectorManager.GiveAngleToCurrentSelector()) > 15f);
            float secondAngle = mSelectorManager.CurrentSelectorObject.transform.GetAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            isTurned = true;
            bool isAngleFlipped = Mathf.Abs(firstAngle - secondAngle) > 270f ? true : false;

            DefaultSelector selector = mSelectorManager.CurrentSelectorObject.GetComponent<DefaultSelector>();
            if (isAngleFlipped)
            {
                if ((secondAngle > firstAngle))
                {
                    selector.StartTurnRoutine(-1);
                }
                else
                {
                    selector.StartTurnRoutine(+1);
                }
            }
            else
            {
                if ((secondAngle < firstAngle))
                {
                    selector.StartTurnRoutine(-1);
                }
                else
                {
                    selector.StartTurnRoutine(+1);
                }
            }
        }
    }
}