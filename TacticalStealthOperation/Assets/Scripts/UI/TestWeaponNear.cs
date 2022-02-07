using UnityEngine;

public class TestWeaponNear : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private GameObject player;

    public void FixedUpdate()
    {
        Character character = HumanLinker.Instance.Characters[0];
        
        // guard condition
        if (character.IsDead())
        {
            return;
        }
        player = character.gameObject;
        
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