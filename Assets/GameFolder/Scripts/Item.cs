using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Ammo, Coin, Grenade, Heart, Weapon };
    public ItemType itemType;
    public int value;

    Rigidbody rigid;
    SphereCollider sphereCollider;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>(); // 현재 콜라이더가 2개인데, 1개만 가져온다고 선언했으므로
                                                         // 맨위에 콜라이더만 가져옴
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);   
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
