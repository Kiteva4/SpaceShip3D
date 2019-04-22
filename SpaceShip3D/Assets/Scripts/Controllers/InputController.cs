using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] protected float rotationDamping;

    protected Transform playerT;
    protected static Transform planetT;
    public static InputController Instance;
    protected Vector2 _vector;
    protected Vector2 vector;
    /// <summary>
    /// Первоначальная позиция
    /// </summary>
    protected Vector2 startInputPos;
    /// <summary>
    /// Изменяемая позиция
    /// </summary>
    protected Vector2 currInputPos;
    /// <summary>
    /// Производное неаправление из startPos и currPos
    /// </summary>
    protected Vector2 dirInputMove;
    protected bool mouseIsHold;
    protected float targetAngle;
    protected float currentAngle;

    protected delegate void MoverHandler();
    protected MoverHandler Mover;

    #region MonoBehaviour
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GameManager.ChangeModeEvent += ChangeMode;
    }

    private void Update()
    {
        //Debug.Log(currentAngle);
        Mover?.Invoke();
    }
    #endregion

    protected void ChangeMode(GameManager.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameManager.GameMode.Idle:
                vector = playerT.up;
                dirInputMove = Vector2.zero;
                _vector = dirInputMove + vector;
                targetAngle = Mathf.Atan2(_vector.y, _vector.x) * Mathf.Rad2Deg;
                currentAngle = targetAngle;
                Mover = GameMover;
                break;
            case GameManager.GameMode.Game:
                StartCoroutine(GameInput());
                break;
            case GameManager.GameMode.ChangePlanet:
                Mover = null;
                break;
            case GameManager.GameMode.Death:
                Mover = null;
                break;
            default:
                break;
        }
    }

    void GameMover()
    {
        _vector = dirInputMove + vector;
        targetAngle = Mathf.Atan2(_vector.y, _vector.x) * Mathf.Rad2Deg;
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationDamping);
        playerT.rotation = Quaternion.Euler(0.0f, 0.0f, currentAngle - 90.0f);
        planetT.RotateAround(planetT.position, playerT.right, -GameManager.gameBaseSpeed * Time.deltaTime);
    }

    public static void SetPlanet(Transform t)
    {
        planetT = t;
    }

    IEnumerator GameInput()
    {
        yield return null;
        while (GameManager.Mode == GameManager.GameMode.Game)
        {
            yield return null;

#if UNITY_EDITOR
            #region mouse controll
            if (Input.GetMouseButtonDown(0))
            {
                startInputPos = Input.mousePosition;
                mouseIsHold = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouseIsHold = false;
            }

            if (mouseIsHold)
            {
                currInputPos = Input.mousePosition;
                if (currInputPos == startInputPos) continue;
                dirInputMove = 0.2f * (currInputPos - startInputPos);
            }

            #endregion
#else
            #region touch controll
            if (Input.touchCount == 0)
            {
                //deltaTouchMove = Vector2.zero;
                continue;
            }

            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    startInputPos = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    currInputPos = Input.GetTouch(0).position;
                    if (currInputPos == startInputPos) continue;
                    dirInputMove = 0.2f * (currInputPos - startInputPos);
                }
            }
            #endregion
#endif
        }
    }
}
