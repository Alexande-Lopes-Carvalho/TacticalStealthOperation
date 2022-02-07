using System;
using TMPro;
using UnityEngine;

public class TestWeaponNear : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void FixedUpdate()
    {
        // guard condition
        if (player.GetComponent<Character>().IsDead())
        {
            return;
        }
        
        bool near = false;
        foreach (GameObject o in PoolLinker.Instance.GetDestroyer("WeaponPool").Pool)
        {
            if ((o.transform.position - player.transform.position).sqrMagnitude < 1)
            {
                near = true;
            }
        }
        text.SetActive(near);
    }
}