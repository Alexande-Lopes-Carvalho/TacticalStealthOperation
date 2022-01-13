using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Path))]
public class PathEditor : Editor {
    private Vector3 offset;
    private int handleDrag = -1;
    private bool leftMouseDrag = false;
    public void OnSceneGUI(){
        Path path = target as Path;
        if(path.PathStates.Count == 0){
            return;
        }
        List<Vector3> list = new List<Vector3>();
        List<Quaternion> listQuat = new List<Quaternion>();
        float defaultUp = 1f, samePlaceUp = 1.35f, sqrRadius = 0.25f*0.25f;
        list.Add(path.PathStates[0].Destination+new Vector3(0, defaultUp, 0));
        for(int i = 1; i < path.PathStates.Count; ++i){
            bool foundNear = false;
            float y = 0;
            for(int j = 0; j < i; ++j){
                if(j != i && (path.PathStates[i].Destination-path.PathStates[j].Destination).sqrMagnitude < sqrRadius){
                    foundNear = true;
                    y = list[j].y+samePlaceUp;
                }
            }
            if(!foundNear){
                y = path.PathStates[i].Destination.y+defaultUp;
            }
            list.Add(new Vector3(path.PathStates[i].Destination.x, y, path.PathStates[i].Destination.z));
        }

        


        // Draw Path
        if(path.Type == Path.PathType.DoOnce || path.Type == Path.PathType.BackAndForth){
            for(int i = 0; i < path.PathStates.Count-1; ++i){
                Handles.color = Color.HSVToRGB((float)i/(path.PathStates.Count-1), 1f, 0.5f);
                Handles.DrawLine(list[i], list[i+1], 3);
            }
            if(path.Type == Path.PathType.BackAndForth){
                Vector3 k = new Vector3(0, -0.25f, 0);
                for(int i = 0; i < path.PathStates.Count-1; ++i){
                    Handles.color = Color.black;
                    Handles.DrawLine(list[i]+k, list[i+1]+k, 3);
                }
            }
        } else if(path.Type == Path.PathType.Loop){
            for(int i = 0; i < path.PathStates.Count; ++i){
                Handles.color = Color.HSVToRGB((float)i/(path.PathStates.Count), 1f, 0.5f);
                Handles.DrawLine(list[i], list[(i+1)%list.Count], 3);
            }
        }

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.grey;
        Debug.Log(Time.time + " startLoop");
        for(int i = 0 ; i < path.PathStates.Count; ++i){
            
            
            Quaternion q = Quaternion.identity;
            if(!path.PathStates[i].NoRotation || path.PathStates.Count <= 1){
                q = Quaternion.Euler(path.PathStates[i].FacingRotation);
            } else {
                int k = (i == 0)? i+1 : i;
                bool done = false;
                for(int j = k; j > 0; --j){
                    if((path.PathStates[j].Destination-path.PathStates[j-1].Destination).sqrMagnitude > sqrRadius){
                        q.SetLookRotation(path.PathStates[j].Destination-path.PathStates[j-1].Destination);
                        done = true;
                        break;
                    } else if(!path.PathStates[j-1].NoRotation){
                        q = Quaternion.Euler(path.PathStates[j-1].FacingRotation);
                        done = true;
                        break;
                    }
                }
                if(!done){
                    q = Quaternion.Euler(path.PathStates[i].FacingRotation);
                }
            }

            listQuat.Add(q);
            bool temp = GUI.changed;
            GUI.changed = false;
            leftMouseDrag = (Event.current.type == EventType.MouseDown && Event.current.button == 0)? true : (Event.current.type == EventType.MouseUp && Event.current.button == 0)? false : leftMouseDrag;

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(list[i],Quaternion.identity/*q*/);
            if(handleDrag == -1 || handleDrag == i){
                if(GUI.changed){
                    if(handleDrag == -1){
                        offset = path.PathStates[i].destination-list[i];
                        //Debug.Log(Time.time + " save offset");
                    }
                    handleDrag = i;
                } else if(!GUI.changed && !leftMouseDrag){
                    handleDrag = -1;
                }
            }

            GUI.changed |= temp;

            if(EditorGUI.EndChangeCheck() && handleDrag == i){
                Debug.Log(Time.time + " move " + i);
                Undo.RecordObject(path, "Move PathState n°" + i);
                Vector3 computedPos = newPosition+offset;
                //Debug.Log(Time.time + " " + computedPos + " " + newPosition + " " + (path.PathStates[i].destination-list[i]) + " " + (path.PathStates[i].destination-offset));
                path.PathStates[i].destination =  new Vector3(SnapValue(computedPos.x, 0.01f), SnapValue(computedPos.y, 0.01f), SnapValue(computedPos.z, 0.01f));
            }
            Handles.color = Color.black;
            if(!path.PathStates[i].NoRotation){
                EditorGUI.BeginChangeCheck();
                Quaternion rot = Handles.Disc(Quaternion.Euler(path.PathStates[i].facingRotation), list[i]+q*new Vector3(0, 0.5f, 0), q*new Vector3(0, 1f, 0), 0.5f, false, 1);
                if(EditorGUI.EndChangeCheck()){
                    Undo.RecordObject(path, "Rotate PathState n°" + i);
                    path.PathStates[i].facingRotation = new Vector3(SnapValue(rot.eulerAngles.x, 1f), SnapValue(rot.eulerAngles.y, 1f), SnapValue(rot.eulerAngles.z, 1f));
                }
            }

            EditorGUI.BeginChangeCheck();
            float newTime = Handles.ScaleSlider(Mathf.Max(path.PathStates[i].timeToWait, 0.25f), list[i]+q*new Vector3(0, 0.75f, 0), q*new Vector3(0, 0, 1) , Quaternion.identity, 0.5f, 1f);
            if(EditorGUI.EndChangeCheck()){
                Undo.RecordObject(path, "Modify time PathState n°" + i);
                path.PathStates[i].timeToWait = Mathf.Max(SnapValue(newTime, 0.01f), 0.0f);
            }

            Handles.Label(list[i]+new Vector3(0, 0.75f, 0), path.PathStates[i].timeToWait + " s", style);


            if(Handles.Button(list[i]+q*new Vector3(0, 0.75f, -0.25f), q*Quaternion.AngleAxis(90, Vector3.up), 0.07f, 0.07f, Handles.RectangleHandleCap)){
                Undo.RecordObject(path, "Update NoRotation");
                path.PathStates[i].noRotation = !path.PathStates[i].noRotation;
            }
        }       

        for(int i = 0 ; i < path.PathStates.Count; ++i){
            Quaternion q = listQuat[i];
            Handles.color = Color.HSVToRGB(0.6f, 1f, 0.5f);

            if(Handles.Button(list[i]+q*new Vector3(0, 1.15f, 0.25f), q*Quaternion.AngleAxis(90, Vector3.up), 0.07f, 0.07f, Handles.RectangleHandleCap)){
                Undo.RecordObject(path, "Add PathState");
                Path.PathState res = new Path.PathState();
                res.destination = path.PathStates[i].destination;
                res.noRotation = path.PathStates[i].noRotation;
                res.facingRotation = path.PathStates[i].facingRotation;
                res.timeToWait = path.PathStates[i].timeToWait;
                path.PathStates.Insert(i, res);
                list.Insert(i, new Vector3(0, 0, 0));
                listQuat.Insert(i, new Quaternion(0, 0, 0, 0));
                ++i;
            }

            Handles.color = Color.HSVToRGB(0.0f, 1f, 0.5f);

            if(Handles.Button(list[i]+q*new Vector3(0, 1.15f, -0.25f), q*Quaternion.AngleAxis(90, Vector3.up), 0.07f, 0.07f, Handles.RectangleHandleCap)){
                Undo.RecordObject(path, "Remove PathState");
                path.PathStates.RemoveAt(i);
                list.RemoveAt(i);
                listQuat.RemoveAt(i);
                i--;
            }
        }
    }

    private float SnapValue(float value, float p){
        return ((int)(value/p))*p;
    }
    
}