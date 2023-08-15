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
        sphereCollider = GetComponent<SphereCollider>(); // ���� �ݶ��̴��� 2���ε�, 1���� �����´ٰ� ���������Ƿ�
                                                         // ������ �ݶ��̴��� ������
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
