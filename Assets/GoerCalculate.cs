using UnityEngine;
using UnityEngine.AI;

public class GoerCalculate : MonoBehaviour
{
    public NavMeshAgent Goer;
    public LineRenderer line;

    //public static GoerCalculate

    public static GoerCalculate me;

    public NavMeshPath path;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        me = this;
        Goer.updateRotation = false;
        Goer.updateUpAxis = false;
        Goer.updatePosition = false;
        Goer.enabled = false;
        
    }

    public void CalcPath(){
        Goer.enabled = true;
        //Goer.SetDestination(TemplatesRooms.me.ExitLocation.position);
        path = new NavMeshPath();
        Goer.CalculatePath(TemplatesRooms.me.ExitLocation.position, path);
        line.positionCount = path.corners.Length;
        for(int i = 0; i < path.corners.Length; i++){
            Vector3 Rounded = new Vector3(Mathf.Round(path.corners[i].x / 10f) * 10f, Mathf.Round(path.corners[i].y / 10f) * 10f, Mathf.Round(path.corners[i].z / 10f) * 10f);
            line.SetPosition(i, Rounded);
        }
        if(path.status == NavMeshPathStatus.PathPartial){
            //if the level isnt beatable, try again
            PlayerStats.me.Level--;
            TemplatesRooms.me.RemakeRooms();

        }
        //Goer.enabled = false;
    }

    public void Reset(){
        line.positionCount = 0;
    }
}
