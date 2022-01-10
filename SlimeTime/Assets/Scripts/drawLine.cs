using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawLine : MonoBehaviour {
   
    [SerializeField] private GameObject stepPrefab;
    [SerializeField] private GameObject playerGame;
    [SerializeField] private Transform parentYellow;
    [SerializeField] private Transform parentGreen;
    [SerializeField] private Transform parentCyan;
    [SerializeField] private Transform parentMagenta;
    [SerializeField] private Transform parentRed;
    [SerializeField] private Transform parentBlue;
    [SerializeField] private Animator animator;

    private GameObject step;
    private Vector3 oldPos;
    private Vector3 newPos;
    private Transform parent;
    private string nameCollider;
    private string goal; 
    private Color color;
    private List<Transform> completedLines;
    private bool colliderHit;
    private List<Vector3> positions; 
    private List<string> startColliders;

    // Start is called before the first frame update
    void Start() {
        goal = "goal";
        nameCollider = "start";
        startColliders = new List<string>{"YellowStart", "BlueStart", "MagentaStart", "GreenStart", "RedStart", "CyanStart"};
        completedLines = new List<Transform>();
        positions = new List<Vector3>();  
    }

    /*
        Description: when the player enter a trigger set the nameCollider to 
                        the collider that has been triggered
        Parameters: Collider: the triggered collider
    */
    void OnTriggerEnter(Collider collider) {
        //Output the Collider's GameObject's name
        nameCollider = collider.name;
        // Debug.Log("Collider " + nameCollider);
        destroyNotCompleted();

        if(startColliders.Contains(nameCollider) || nameCollider.Contains("Step")) colliderHit = true; else colliderHit = false;
        setColor();
        setGoalToReach();
        newPos = oldPos = collider.transform.position;
        // Debug.Log("hit " + colliderHit);
        // Debug.Log("start " + nameCollider);
        // Debug.Log("goal " + goal);
        setReachedGoal();
    }

    // Update is called once per frame
    void Update() {   
        if(completedLines.Count == 1) {
            Debug.Log(animator.parameters[0]); 
            animator.SetTrigger("Open");}

        if(Input.GetKeyDown(KeyCode.Backspace)) resetUncompleted();

        if(Input.GetKeyDown(KeyCode.C)) destroyCurrent();

        if(Input.GetKeyDown(KeyCode.Space) && colliderHit){
        	newPos = playerGame.transform.position;
			CreatePositions(newPos, oldPos);      
            SpawnStep();
        	oldPos = newPos;
        }
        
    }

    /*
        Description: set the color to spawn based on the starting point
        Parameters: nothing
        Return value: nothing
    */
    void setColor(){
        if(nameCollider == "YellowStart") color = Color.yellow;
        else if(nameCollider == "BlueStart") color = Color.blue;
        else if(nameCollider == "MagentaStart") color = Color.magenta;
        else if(nameCollider == "RedStart") color = Color.red;
        else if(nameCollider == "GreenStart") color = Color.green;
        else if(nameCollider == "CyanStart") color = Color.cyan;
    }

    /*
        Description: set goaldReached to true and add the line to the list of 
                        reached goal when the goal is reached with the right color
        Parameters: nothing
        Return value: nothing
    */
    void setReachedGoal() {
        if (nameCollider == goal) {
            Debug.Log("goal reached");
            colliderHit = false;
            newPos = playerGame.transform.position;

			CreatePositions(newPos, oldPos);     
            SpawnStep();

            color = Color.clear;

            if(positions != null) positions.Clear();

            if(nameCollider == "YellowGoal") completedLines.Add(parentYellow); 
            else if(nameCollider == "BlueGoal") completedLines.Add(parentBlue);
            else if(nameCollider == "MagentaGoal") completedLines.Add(parentMagenta);
            else if(nameCollider == "RedGoal") completedLines.Add(parentRed);
            else if(nameCollider == "GreenGoal") completedLines.Add(parentGreen);
            else if(nameCollider == "CyanGoal") completedLines.Add(parentCyan);
        } else {
            // Debug.Log("destroy");
            destroyNotCompleted();
        }
    }



    /*
        Description: set which goal the player needs to reach based on 
                    the starting point
        Parameters: nothing
        Return value: nothing
    */
    void setGoalToReach() {
        if (nameCollider == "YellowStart") goal = "YellowGoal";
        else if (nameCollider == "GreenStart") goal = "GreenGoal";
        else if (nameCollider == "MagentaStart") goal = "MagentaGoal";
        else if (nameCollider == "BlueStart") goal = "BlueGoal";
        else if (nameCollider == "RedStart") goal = "RedGoal";
        else if (nameCollider == "CyanStart") goal = "CyanGoal";
    }

    /*
        Description: create a list of positions depending on where the player passed
                    using linear interpolation
        Parameters: Vector 3: x, y and z of the previous position of the player 
                    Vector 3: x, y and z of the new position of the player
        Return value: nothing
    */
    void CreatePositions(Vector3 newPosition, Vector3 oldPosition) {
        float distance = Vector3.Distance(oldPosition, newPosition);
        float stepSize = stepPrefab.GetComponent<ParticleSystem>().shape.radius / distance;
        float percentage = stepSize;
        
        while(percentage < 1) {
            //TODO: change based on slime Y position
            Vector3 pos = Vector3.Lerp(oldPosition, newPosition, percentage);
            pos.y -= 1;
            positions.Add(pos);
            percentage += stepSize;
        }
    }

    /*
        Description: spawn colored particles on the current position, depending on the current color
        Paremeters: List: containing Vector3
        Return value: nothing
    */
    void SpawnStep() {
        // Debug.Log("spawn");
        if(color == Color.yellow) parent = parentYellow;
        else if(color == Color.cyan) parent = parentCyan;
        else if(color == Color.green) parent = parentGreen;
        else if(color == Color.magenta) parent = parentMagenta;
        else if(color == Color.red) parent = parentRed;

        foreach(Vector3 pos in positions) {
            step = Instantiate(stepPrefab, parent) as GameObject;
            step.transform.position = pos;
            // step.transform.rotation = rotation;
            Renderer rend = step.GetComponent<Renderer>();
            rend.material.SetColor("_Color", color);
        }

    }

    /*
        Description: destroy the line and all the children of that line
        Parameters: Transform: the line to destroy
        Return value: nothing
    */
    void destroyLine(Transform line) {
        for (int i = 0; i < line.transform.childCount; i++){
            Destroy(line.transform.GetChild(i).gameObject);
        } 
    }

    /*
        Description: when clicking "backspace" destroy all the lines that 
                        hasn't been completed yet
        Parameters: nothing
        Return value: nothing
    */
    void resetUncompleted() {
        if(completedLines.Count > 0) {
            if (!(completedLines.Contains(parentYellow))) destroyLine(parentYellow);
            if (!(completedLines.Contains(parentBlue))) destroyLine(parentBlue);
            if (!(completedLines.Contains(parentGreen))) destroyLine(parentGreen);
            if (!(completedLines.Contains(parentCyan))) destroyLine(parentCyan);
            if (!(completedLines.Contains(parentRed))) destroyLine(parentRed);
            if (!(completedLines.Contains(parentMagenta))) destroyLine(parentMagenta);

            goal = "goal";
            nameCollider = "start";
            positions.Clear();
        }  
    }

    /*
        Description: destroy the line that is being drew
        Paremeters: nothing
        Return value: nothing
    */ 
    void destroyCurrent() {
        if (nameCollider == "YellowStart") destroyLine(parentYellow); 
        else if (nameCollider == "CyanStart") destroyLine(parentCyan);
        else if (nameCollider == "MagentaStart") destroyLine(parentMagenta);
        else if (nameCollider == "BlueStart") destroyLine(parentBlue);
        else if (nameCollider == "RedStart") destroyLine(parentRed);
    }

    /*
    TODO: check this function, shouldn't work this way
        Description: destroy the lines that hasn't been completed when passing over another starting point
        Paremeters: nothing
        Return value: nothing
    */ 
    void destroyNotCompleted() {
        if (nameCollider == "YellowStart" && color != Color.yellow && !(completedLines.Contains(parentYellow))) destroyLine(parentYellow);
        if (nameCollider == "CyanStar" && color != Color.cyan && !(completedLines.Contains(parentCyan))) destroyLine(parentCyan);
        if (nameCollider == "MagentaStart" && color != Color.magenta && !(completedLines.Contains(parentMagenta))) destroyLine(parentMagenta);
        if (nameCollider == "BlueStart" && color != Color.blue && !(completedLines.Contains(parentBlue))) destroyLine(parentBlue);
        if (nameCollider == "RedStart" && color != Color.red && !(completedLines.Contains(parentRed))) destroyLine(parentRed);
    }
}
