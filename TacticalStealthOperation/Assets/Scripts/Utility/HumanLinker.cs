using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLinker : MonoBehaviour {
    private static HumanLinker instance;
    public static HumanLinker Instance{get => instance;}
    private List<Human> humans;
    public List<Human> Humans{get=> humans;}
    private List<Guard> guards;
    public List<Guard> Guards{get=> guards;}
    private List<Character> characters;
    public List<Character> Characters{get=> characters;}
    //private float activationDist = 30f, activationSqrDist;
    private void Awake(){
        if(instance != null){
            Destroy(instance.gameObject);
        }
        instance = this;
        humans = new List<Human>();
        guards = new List<Guard>();
        characters = new List<Character>();
        Guard.PriorityCount = 50;
        //activationSqrDist = activationDist*activationDist;
    }

    private void Update(){

    }

    public void Register(Human h){
        humans.Add(h);
        if(h is Guard){
            guards.Add((Guard)h);
        }
        if(h is Character){
            characters.Add((Character)h);
        }
    }

    public void Remove(Human h){
        humans.Remove(h);
        if(h is Guard){
            guards.Remove((Guard)h);
        } 
        if(h is Character){
            characters.Remove((Character)h);
        }
    }
}
