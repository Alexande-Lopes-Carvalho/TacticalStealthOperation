using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLinker : MonoBehaviour {
    private static List<Human> humans;
    public static List<Human> Humans{get=> humans;}
    private static List<Guard> guards;
    public static List<Guard> Guards{get=> guards;}
    private static List<Character> characters;
    public static List<Character> Characters{get=> characters;}
    private void Awake(){
        humans = new List<Human>();
        guards = new List<Guard>();
        characters = new List<Character>();
        Guard.PriorityCount = 50;
    }

    public static void Register(Human h){
        humans.Add(h);
        if(h is Guard){
            guards.Add((Guard)h);
        }
        if(h is Character){
            characters.Add((Character)h);
        }
    }

    public static void Remove(Human h){
        humans.Remove(h);
        if(h is Guard){
            guards.Remove((Guard)h);
        } 
        if(h is Character){
            characters.Remove((Character)h);
        }
    }
}
