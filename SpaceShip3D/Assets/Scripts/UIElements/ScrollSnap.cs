using System;
using UnityEngine;
using UnityEngine.UI;
public class ScrollSnap : MonoBehaviour
{
	private event Action OnNeedSnap = delegate { };

	[SerializeField] private GameObject[] Items;
	[SerializeField] private GameObject[] instItems;
	[SerializeField] private int itemOffset;
	//[SerializeField] private float scaleOffset;
	[SerializeField] private float snapSpeed;
	[SerializeField] private float scaleTime = 6f;

	[SerializeField] private Button nextButton;
	[SerializeField] private Button previousButton; 


	private Vector2[] itemPos;
	//private Vector2[] itemScale;

	private int snapIndex;
	private int SnapIndex
	{
		get { return snapIndex; }
		set
		{
			snapIndex = value;
			Debug.Log("Changed to index : " + snapIndex);
		}
	}

	private RectTransform contentRect;
	private int nearestItemID;
	private bool isScrolling;
	private Vector2 contentVector;

	private void Awake()
	{
		previousButton.onClick.AddListener(delegate { HandlerOnClickMovePrevious(); });
		nextButton.onClick.AddListener(delegate { HandlerOnClickMoveNext(); });
	}

	private void Start()
	{
		ConstructItemsOnContent();
	}

	private void Update()
	{
		OnNeedSnap?.Invoke();
	}

	private void ConstructItemsOnContent()
	{
		contentRect = GetComponent<RectTransform>();
		instItems = new GameObject[Items.Length];
		itemPos = new Vector2[Items.Length];
		//itemScale = new Vector2[Items.Length];

		for (int i = 0; i < Items.Length; i++)
		{
			instItems[i] = Instantiate(Items[i], transform, false);
			if (i > 0)
				instItems[i].transform.localPosition = new Vector2(
					instItems[i - 1].transform.localPosition.x + Items[i].GetComponent<RectTransform>().sizeDelta.x + itemOffset,
					instItems[i].transform.localPosition.y);

			itemPos[i] = -instItems[i].transform.localPosition;
		}
	}

	private void HandlerCheckNearest()
	{
		float nearestPos = float.MaxValue;
		for (int i = 0; i < Items.Length; i++)
		{
			float distance = Mathf.Abs(contentRect.anchoredPosition.x - itemPos[i].x);
			if (distance < nearestPos)
			{
				nearestPos = distance;
				nearestItemID = i;
			}
		}
	}

	private void HandlerSmoothMove()
	{
		contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, itemPos[nearestItemID].x, snapSpeed * Time.deltaTime);
		contentRect.anchoredPosition = contentVector;

		if (Mathf.Abs(contentRect.anchoredPosition.x - itemPos[nearestItemID].x) < 0.8f)
		{
			snapIndex = nearestItemID;
			OnNeedSnap = null;
		}
	}

	public void OnScrollgBegin ()
	{
		OnNeedSnap = HandlerCheckNearest;
	}

	public void OnScrollEnded()
	{
		OnNeedSnap += HandlerSmoothMove;
	}

	public void HandlerOnClickMovePrevious()
	{
		OnNeedSnap = MovePrevious;
	}

	public void HandlerOnClickMoveNext()
	{
		OnNeedSnap = MoveNext;
	}

	private void MovePrevious()
	{
		if (snapIndex == 0)
		{
			OnNeedSnap = null;
		}
		else
		{
			contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, itemPos[snapIndex - 1].x, snapSpeed * Time.deltaTime);
			contentRect.anchoredPosition = contentVector;

			if (Mathf.Abs(contentRect.anchoredPosition.x - itemPos[snapIndex - 1].x) < 0.8f)
			{
				snapIndex = snapIndex - 1;
				OnNeedSnap = null;
			}
		}
	}

	private void MoveNext()
	{
		if (snapIndex == itemPos.Length - 1)
		{
			OnNeedSnap = null;
		}
		else
		{
			contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, itemPos[snapIndex + 1].x, snapSpeed * Time.deltaTime);
			contentRect.anchoredPosition = contentVector;

			if (Mathf.Abs(contentRect.anchoredPosition.x - itemPos[snapIndex + 1].x) < 0.8f)
			{
				snapIndex = snapIndex + 1;
				OnNeedSnap = null;
			}
		}

	}
}
