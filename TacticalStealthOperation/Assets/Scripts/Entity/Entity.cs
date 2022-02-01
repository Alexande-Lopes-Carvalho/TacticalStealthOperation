using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour {
    [SerializeField] private int maxHealth;
    [SerializeField] [Min(-1)] private int currentHealth = -1;
    private Transform earTransform;
    public Transform EarTransform{get => earTransform; set => earTransform = value;}
    public virtual void Awake(){
        earTransform = transform;
    }

    // Start is called before the first frame update
    public virtual void Start() {
        if(currentHealth == -1){
            currentHealth = maxHealth;
        }
        if(IsAlive()){
            EarLinker.Register(this);
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
        if(currentHealth <= 0){
            Kill();
        }
    }

    public virtual void Damage(int damage){
        AddHealth(-damage);
        
    }

    public virtual void Damage(int damage, Transform origin){
        Damage(damage);
    }

    public virtual void Heal(int heal){
        AddHealth(heal);
    }

    public virtual void Heal(int heal, Transform origin){
        Heal(heal);
    }

    public virtual void Kill(){
        Destroy(gameObject);
    }

    public virtual void OnDestroy(){
        EarLinker.Remove(this);
    }

    public virtual void Ear(Transform t){

    }

    public virtual bool IsDead(){
        return currentHealth <= 0;
    }

    public virtual bool IsAlive(){
        return currentHealth > 0;
    }
}
