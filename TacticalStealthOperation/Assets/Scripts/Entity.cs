using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour {
    [SerializeField] private int maxHealth;
    [SerializeField] [Min(-1)] private int currentHealth = -1;


    // Start is called before the first frame update
    public virtual void Start() {
        if(currentHealth == -1){
            currentHealth = maxHealth;
        }
        //Debug.Log("Start health " + currentHealth + " "+ name);
    }

    public virtual void FixedUpdate(){

    }

    // Update is called once per frame
    public virtual void Update() {
        
    }
    private void AddHealth(int heal){
        //Debug.Log(Time.time + " " + currentHealth + " " +  heal);
        currentHealth += heal;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        if(currentHealth < 0){
            Kill();
        }
    }

    public virtual void Damage(int damage){
        AddHealth(-damage);
        
    }

    public virtual void Heal(int heal){
        AddHealth(heal);
    }

    public virtual void Kill(){
        Destroy(gameObject);
    }
}
