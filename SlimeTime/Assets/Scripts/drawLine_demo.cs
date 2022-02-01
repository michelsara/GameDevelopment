using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawLine_demo : MonoBehaviour {

    private static string tagName;
   
    [SerializeField] private GameObject stepPrefab;
    [SerializeField] private GameObject playerGame;
    [SerializeField] private GameObject parentPlayer;
    [SerializeField] private Transform parentYellow;
    [SerializeField] private Transform parentGreen;
    [SerializeField] private Animator animator;

    private static GameObject step;
    private static Vector3 oldPos;
    private static Vector3 newPos;
    private static Transform parent;
    private static string nameCollider;
    private static string tagCollider;
    private static string goal; 
    private static Color color;
    private static bool startHit;
    private static List<Vector3> positions; 
    public static Transform parentCollider;
    public static bool completedYellow;
    public static bool completedGreen;

    private bool isWalking;

    // Start is called before the first frame update
    void Start() {
        animator.ResetTrigger("Open");
        goal = "goal";
        nameCollider = "start";
        positions = new List<Vector3>();  
        completedYellow = completedGreen = false;
        color = Color.clear;
    }

    /*
        Description: when the player enter a trigger set the nameCollider to 
                        the collider that has been triggered
        Parameters: Collider: the triggered collider
    */
    void OnTriggerEnter(Collider collider) {
        tagCollider = collider.tag;
        nameCollider = collider.name;
        parentCollider = collider.transform.parent;

        setColor();
        setParent();
      
        if (tagCollider == "Start") {
            if ((parent == parentGreen && !completedGreen) || (parent == parentYellow && !completedYellow)) {
                if(!startHit) startHit = true; else startHit = false;
            }
        } 
        
        setGoalToReach();
        if(nameCollider == goal) setReachedGoal(collider.transform.position);
        newPos = oldPos = collider.transform.position;

		destroyCurrentHit();
    }

    // Update is called once per frame
    void Update() {   

        if(completedGreen && completedYellow) animator.SetTrigger("Open");

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            if(Input.GetKeyDown(KeyCode.Backspace)) destroyAll();

            if(Input.GetKeyDown(KeyCode.G)) {
                destroyLine(parentGreen);
                completedGreen = false;
            }
            if(Input.GetKeyDown(KeyCode.Y)) {
                destroyLine(parentYellow);
                completedYellow = false;
            }
            if(Input.GetKeyDown(KeyCode.U)) destroyUncompleted();
        }

        if(startHit && isMoving()){
            newPos = playerGame.transform.position;
            CreatePositions(newPos, oldPos);     
            SpawnStep();
            oldPos = newPos;
        }
        
    }


    bool isMoving(){
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
 
        // Checks if player is walking and isGrounded
        // Will allow head bob
        if (targetVelocity.x != 0 || targetVelocity.z != 0) isWalking = true; else isWalking = false;

        return isWalking;
    }

    /*
        Description: set the color to spawn based on the starting point
        Parameters: nothing
        Return value: nothing
    */
    void setColor(){
        if (color == Color.clear) {
            if(nameCollider == "YellowStart") {
                color = Color.yellow;
                tagName = "stepYellow";
                parentPlayer.tag = tagName;
            } else if(nameCollider == "GreenStart") {
                color = Color.green;
                tagName = "stepGreen";
                parentPlayer.tag = tagName;
            }
        }
    }

    /*
        Description: set the parent of each step depending on color
        Parameters: nothing
        Return value: nothing
    */
    void setParent() {
        if(color == Color.yellow) parent = parentYellow;
        else if(color == Color.green) parent = parentGreen;
    }

    /*
        Description: set goaldReached to true and add the line to the list of 
                        reached goal when the goal is reached with the right color
        Parameters: nothing
        Return value: nothing
    */
    void setReachedGoal(Vector3 position) {        
        startHit = false;

        CreatePositions(position, oldPos);     
        SpawnStep();
        if(positions != null) positions.Clear();
        color = Color.clear;
        parent = null;
        parentPlayer.tag = "Untagged";
        if(nameCollider == "YellowGoal" && !completedYellow) completedYellow = true;
        else if(nameCollider == "GreenGoal" && !completedGreen) completedGreen = true;
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
        float stepSize = stepPrefab.GetComponent<ParticleSystem>().shape.radius;
        float percentage = stepSize;
        for(int i = 0; percentage < 1; i += 2) {
            //TODO: change based on slime Y position
            Vector3 pos = Vector3.Lerp(oldPosition, newPosition, percentage);
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
        Vector3 pos = positions[positions.Count - 1];
		step = Instantiate(stepPrefab, parent) as GameObject;
		step.transform.position = pos;
		step.tag = tagName;
		// step.transform.rotation = rotation;
		Renderer rend = step.GetComponent<Renderer>();
		rend.material.SetColor("_Color", color);
    }

    /*
        Description: destroy the line and all the children of that line
        Parameters: Transform: the line to destroy
        Return value: nothing
    */
    public static void destroyLine(Transform line) {
        for (int i = 0; i < line.transform.childCount; i++){
            Destroy(line.transform.GetChild(i).gameObject);
        }
        goal = "goal";
        nameCollider = "start";
        positions.Clear();
        color = Color.clear;
        startHit = false;
    }

    /*
        Description: when clicking "shift+backspace" destroy all the lines that 
                        hasn't been completed yet
        Parameters: nothing
        Return value: nothing
    */
    void destroyUncompleted() {
        if (!completedYellow) destroyLine(parentYellow);
        if (!completedGreen) destroyLine(parentGreen);
    }

    /*
        Description: destroy all the lines, even completed
        Paremeters: nothing
        Return value: nothing
    */ 
    void destroyAll() {
        destroyLine(parentYellow);
        destroyLine(parentGreen); 
        completedYellow = completedGreen = false;
    }

	/*
        Description: destroy the line if something diffirent from the particle itself, goal or start of the current color is hit.
        Paremeters: nothing
        Return value: nothing
    */
	void destroyCurrentHit() {
        if (color == Color.green && ((tagCollider == "Goal" && nameCollider != goal) || (tagCollider == "Start" && nameCollider != "GreenStart"))) destroyLine(parentGreen);
        if (color == Color.yellow && ((tagCollider == "Goal" && nameCollider != goal) || (tagCollider == "Start" && nameCollider != "YellowStart"))) destroyLine(parentYellow);
    }
}
