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
    private bool colliderHit;
    private List<Vector3> positions; 
    private List<string> startColliders;
    private List<string> goalColliders;
    private Transform parentCollider;
    private bool completedYellow;
    private bool completedCyan;
    private bool completedBlue;
    private bool completedMagenta;
    private bool completedRed;
    private bool completedGreen;



    // Start is called before the first frame update
    void Start() {
        animator.ResetTrigger("Open");
        goal = "goal";
        nameCollider = "start";
        startColliders = new List<string>{"YellowStart", "BlueStart", "MagentaStart", "GreenStart", "RedStart", "CyanStart"};
        goalColliders = new List<string>{"YellowGoal", "BluewGoal", "MagentaGoal", "GreenGoal", "RedGoal", "CyanGoal"};
        positions = new List<Vector3>();  
        completedMagenta = completedBlue = completedCyan = completedRed = completedYellow = completedGreen = false;
        color = Color.clear;
    }

    /*
        Description: when the player enter a trigger set the nameCollider to 
                        the collider that has been triggered
        Parameters: Collider: the triggered collider
    */
    void OnTriggerEnter(Collider collider) {
        nameCollider = collider.name;
        parentCollider = collider.transform.parent;
        Debug.Log(nameCollider);
        Debug.Log(startColliders.Contains(nameCollider));
        if(startColliders.Contains(nameCollider) || nameCollider.Contains("Step")) colliderHit = true; else colliderHit = false;
        setColor();
        setParent();
        setGoalToReach();
        newPos = oldPos = collider.transform.position;
        setReachedGoal();
        // destroyNotCompleted();
        destroyCurrentHit();

    }

    // Update is called once per frame
    void Update() {   
        if(completedBlue && completedCyan && completedGreen && completedMagenta && completedRed && completedYellow) animator.SetTrigger("Open");

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
        if (color == Color.clear) {
            if(nameCollider == "YellowStart") color = Color.yellow;
            else if(nameCollider == "BlueStart") color = Color.blue;
            else if(nameCollider == "MagentaStart") color = Color.magenta;
            else if(nameCollider == "RedStart") color = Color.red;
            else if(nameCollider == "GreenStart") color = Color.green;
            else if(nameCollider == "CyanStart") color = Color.cyan;
        }
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

            if(nameCollider == "YellowGoal" && !completedYellow) completedYellow = true; 
            else if(nameCollider == "BlueGoal" && !completedBlue) completedBlue = true;
            else if(nameCollider == "MagentaGoal" && !completedMagenta) completedMagenta = true;
            else if(nameCollider == "RedGoal" && !completedRed) completedRed = true;
            else if(nameCollider == "GreenGoal" && !completedGreen) completedGreen = true;
            else if(nameCollider == "CyanGoal" && !completedCyan) completedCyan = true;
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

    void setParent() {
          // Debug.Log("spawn");
        if(color == Color.yellow) parent = parentYellow;
        else if(color == Color.cyan) parent = parentCyan;
        else if(color == Color.green) parent = parentGreen;
        else if(color == Color.magenta) parent = parentMagenta;
        else if(color == Color.red) parent = parentRed;
    }
    /*
        Description: spawn colored particles on the current position, depending on the current color
        Paremeters: List: containing Vector3
        Return value: nothing
    */
    void SpawnStep() {
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
        if (!completedYellow) destroyLine(parentYellow);
        if (!completedBlue) destroyLine(parentBlue);
        if (!completedGreen) destroyLine(parentGreen);
        if (!completedCyan) destroyLine(parentCyan);
        if (!completedRed) destroyLine(parentRed);
        if (!completedMagenta) destroyLine(parentMagenta);

        goal = "goal";
        nameCollider = "start";
        positions.Clear();
        
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
        if (nameCollider == "YellowStart" && color != Color.yellow && !completedYellow) destroyLine(parentYellow);
        if (nameCollider == "CyanStar" && color != Color.cyan && !completedCyan) destroyLine(parentCyan);
        if (nameCollider == "MagentaStart" && color != Color.magenta && !completedMagenta) destroyLine(parentMagenta);
        if (nameCollider == "BlueStart" && color != Color.blue && !completedBlue) destroyLine(parentBlue);
        if (nameCollider == "RedStart" && color != Color.red && !completedRed) destroyLine(parentRed);
        if (nameCollider == "GreenStart" && color != Color.green && !completedGreen) destroyLine(parentGreen);
    }

    void destroyCurrentHit() {
        if (color == Color.cyan && ((nameCollider.Contains("Step") && parentCollider != parentCyan) || (goalColliders.Contains(nameCollider) && nameCollider != goal) || (startColliders.Contains(nameCollider) && nameCollider != "CyanStart"))) destroyLine(parentCyan);
        if (color == Color.blue && ((nameCollider.Contains("Step") && parentCollider != parentBlue) || (goalColliders.Contains(nameCollider) && nameCollider != goal) || (startColliders.Contains(nameCollider) && nameCollider != "Bluetart"))) destroyLine(parentBlue);
        if (color == Color.red && ((nameCollider.Contains("Step") && parentCollider != parentRed) || (goalColliders.Contains(nameCollider) && nameCollider != goal) || (startColliders.Contains(nameCollider) && nameCollider != "RedStart"))) destroyLine(parentRed);
        if (color == Color.magenta && ((nameCollider.Contains("Step") && parentCollider != parentMagenta) || (goalColliders.Contains(nameCollider) && nameCollider != goal) || (startColliders.Contains(nameCollider) && nameCollider != "MagentaStart"))) destroyLine(parentMagenta);
        if (color == Color.green && ((nameCollider.Contains("Step") && parentCollider != parentGreen) || (goalColliders.Contains(nameCollider) && nameCollider != goal) || (startColliders.Contains(nameCollider) && nameCollider != "GreenStart"))) destroyLine(parentGreen);
        if (color == Color.yellow && ((nameCollider.Contains("Step") && parentCollider != parentYellow) || (goalColliders.Contains(nameCollider) && nameCollider != goal) || (startColliders.Contains(nameCollider) && nameCollider != "YellowStart"))) destroyLine(parentYellow);

    }
}
