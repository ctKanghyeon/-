using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // 유니티에서 제공하는 AI 네비게이션

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public Transform target;
    public bool isChase;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        // Enemy > Bone_Body > Boby
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2.0f);
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if(isChase) nav.SetDestination(target.position); // 도착할 목표 위치 지정 함수
    }

    private void OnTriggerEnter(Collider other)
    {
        if (curHealth <= 0) return;

        if(other.tag == "Melee")
        {
            Weapon wepon = other.GetComponent<Weapon>();
            curHealth -= wepon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;

        StartCoroutine(OnDamage(reactVec, true));
    }



    IEnumerator OnDamage(Vector3 vec, bool isGrenade)
    {
        mat.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        mat.color = Color.white;

        if(curHealth <= 0)
        {
            mat.color = Color.gray;
            gameObject.layer = 12;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            if(isGrenade)
            {
                vec = vec.normalized;
                vec += Vector3.up * 3;

                rigid.freezeRotation = false;
                rigid.AddForce(vec * 5, ForceMode.Impulse);
                rigid.AddTorque(vec * 15, ForceMode.Impulse); //회전을 주는 함수
            }
            else
            {
                vec = vec.normalized;
                vec += Vector3.up;
                rigid.AddForce(vec * 5, ForceMode.Impulse);
            }
            Destroy(gameObject, 4);
        }

    }

}
