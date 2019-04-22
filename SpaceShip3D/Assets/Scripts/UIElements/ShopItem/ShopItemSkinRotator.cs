using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemSkinRotator : MonoBehaviour
{
	private Transform t;
	[SerializeField] private float rotateSpeed = 36f;
	// Start is called before the first frame update
	void Awake()
    {
		t = GetComponent<Transform>();
		FindObjectOfType<ItemsManager>().SkinRotator += SkinRotate;
	}

    // Update is called once per frame
    void SkinRotate()
    {
		t.RotateAround(t.position,Vector2.up, Time.deltaTime * rotateSpeed);
    }
}
