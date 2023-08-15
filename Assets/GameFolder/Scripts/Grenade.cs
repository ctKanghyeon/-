using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;

    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3.0f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero; // 속도를 없애는 작업
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 
            15, Vector3.up, 
            0f, LayerMask.GetMask("Enemy")); // 수류탄이 맞은 적들의 정보를 다 가져옴
        // 원형태의 rayCast이고 터지는 위치(자신), 범위 15, 방향은 위쪽(없어도된다해서)
        // 쏘는길이는 0 (길이를 주게되면 설정한 방향으로 쏘아올려서 위쪽으로 터지는거임)
        // Enemy의 layer를 가진것들만 가져올거임

        foreach(RaycastHit hits in rayHits)
        {
            hits.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5);
    }
}
