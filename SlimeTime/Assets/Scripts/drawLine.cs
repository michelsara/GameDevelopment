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
    private string tagCollider;
    private string goal; 
    private Color color;
    private bool startHit;
    private List<Vector3> positions; 
    private Transform parentCollider;
    private bool completedYellow;
    private bool completedCyan;
    private bool completedBlue;
    private bool completedMagenta;
    private bool completedRed;
    private bool completedGreen;

    private bool isWalking;

    private Camera[] gameCamera;

    // Start is called before the first frame update
    void Start() {
        gameCamera = Camera.allCameras;
        gameCamera[0].enabled = false;
        animator.ResetTrigger("Open");
        goal = "goal";
        nameCollider = "start";
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
        tagCollider = collider.tag;
        nameCollider = collider.name;
        parentCollider = collider.transform.parent;

        setColor();
        setParent();
      
        if (tagCollider == "Start") {
            if(!startHit) startHit = true; else startHit = false;
        } 
        
        setGoalToReach();
        if(nameCollider == goal) setReachedGoal(collider.transform.position);
        newPos = oldPos = collider.transform.position;
        destroyCurrentHit();

    }

    // Update is called once per frame
    void Update() {   

        Debug.Log("Complete magenta: " + completedMagenta); 
        Debug.Log("Complete blue:" + completedBlue); 
        Debug.Log("Complete cyan:" + completedCyan); 
        Debug.Log("Complete red:" + completedRed); 
        Debug.Log("Complete yellow:" + completedYellow); 
        Debug.Log("Complete green:" + completedGreen); 
        if(completedBlue && completedCyan && completedGreen && completedMagenta && completedRed && completedYellow) animator.SetTrigger("Open");
        // if(completedMagenta) animator.SetTrigger("Open");

        if(Input.GetKeyDown(KeyCode.T)) {
            
            if(gameCamera[0].enabled) {
                gameCamera[0].enabled = false;
                gameCamera[1].enabled = true;
            } else {
                gameCamera[0].enabled = true;
                gameCamera[1].enabled = false;
            }
        }
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            if(Input.GetKeyDown(KeyCode.Backspace)) destroyAll();

            if(Input.GetKeyDown(KeyCode.C)) {
                destroyLine(parentCyan);
                completedCyan = false;
            }
            if(Input.GetKeyDown(KeyCode.B)) {
                destroyLine(parentBlue);
                completedBlue = false;
            }
            if(Input.GetKeyDown(KeyCode.R)) {
                destroyLine(parentRed);
                completedRed = false;
            }
            if(Input.GetKeyDown(KeyCode.G)) {
                destroyLine(parentGreen);
                completedGreen = false;
            }
            if(Input.GetKeyDown(KeyCode.Y)) {
                destroyLine(parentYellow);
                completedYellow = false;
            }
            if(Input.GetKeyDown(KeyCode.M)) {
                destroyLine(parentMagenta);
                completedMagenta = false;
            }
            if(Input.GetKeyDown(KeyCode.U)) destroyUncompleted();
        }

        if(startHit && isMoving() && Input.GetKey(KeyCode.Mouse1)){
            newPos = playerGame.transform.position;
            Debug.Log(newPos);
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
            if(nameCollider == "YellowStart") color = Color.yellow;
            else if(nameCollider == "BlueStart") color = Color.blue;
            else if(nameCollider == "MagentaStart") color = Color.magenta;
            else if(nameCollider == "RedStart") color = Color.red;
            else if(nameCollider == "GreenStart") color = Color.green;
            else if(nameCollider == "CyanStart") color = Color.cyan;
        }
    }

    /*
        Description: set the parent of each step depending on color
        Parameters: nothing
        Return value: nothing
    */
    void setParent() {
        if(color == Color.yellow) parent = parentYellow;
        else if(color == Color.cyan) parent = parentCyan;
        else if(color == Color.green) parent = parentGreen;
        else if(color == Color.magenta) parent = parentMagenta;
        else if(color == Color.red) parent = parentRed;
        else if(color == Color.blue) parent = parentBlue;
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

        if(nameCollider == "YellowGoal" && !completedYellow) completedYellow = true; 
        else if(nameCollider == "BlueGoal" && !completedBlue) completedBlue = true;
        else if(nameCollider == "MagentaGoal" && !completedMagenta) completedMagenta = true;
        else if(nameCollider == "RedGoal" && !completedRed) completedRed = true;
        else if(nameCollider == "GreenGoal" && !completedGreen) completedGreen = true;
        else if(nameCollider == "CyanGoal" && !completedCyan) completedCyan = true;
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
        if (!completedBlue) destroyLine(parentBlue);
        if (!completedGreen) destroyLine(parentGreen);
        if (!completedCyan) destroyLine(parentCyan);
        if (!completedRed) destroyLine(parentRed);
        if (!completedMagenta) destroyLine(parentMagenta);
    }

    /*
        Description: destroy all the lines, even completed
        Paremeters: nothing
        Return value: nothing
    */ 
    void destroyAll() {
        destroyLine(parentYellow); 
        destroyLine(parentCyan);
        destroyLine(parentMagenta);
        destroyLine(parentBlue);
        destroyLine(parentRed);
        destroyLine(parentGreen); 
        completedMagenta = completedBlue = completedCyan = completedRed = completedYellow = completedGreen = false;
    }

    /*
        Description: destroy the line if something diffirent from the particle itself, goal or start of the current color is hit.
        Paremeters: nothing
        Return value: nothing
    */ 
    void destroyCurrentHit() {
        if (color == Color.cyan && ((nameCollider.Contains("Step") && parentCollider != parentCyan) || (tagCollider == "Goal" && nameCollider != goal) || (tagCollider == "Start" && nameCollider != "CyanStart"))) destroyLine(parentCyan);
        if (color == Color.blue && ((nameCollider.Contains("Step") && parentCollider != parentBlue) || (tagCollider == "Goal" && nameCollider != goal) || (tagCollider == "Start" && nameCollider != "BlueStart"))) destroyLine(parentBlue);
        if (color == Color.red && ((nameCollider.Contains("Step") && parentCollider != parentRed) || (tagCollider == "Goal" && nameCollider != goal) || (tagCollider == "Start" && nameCollider != "RedStart"))) destroyLine(parentRed);
        if (color == Color.magenta && ((nameCollider.Contains("Step") && parentCollider != parentMagenta) || (tagCollider == "Goal" && nameCollider != goal) || (tagCollider == "Start" && nameCollider != "MagentaStart"))) destroyLine(parentMagenta);
        if (color == Color.green && ((nameCollider.Contains("Step") && parentCollider != parentGreen) || (tagCollider == "Goal" && nameCollider != goal) || (tagCollider == "Start" && nameCollider != "GreenStart"))) destroyLine(parentGreen);
        if (color == Color.yellow && ((nameCollider.Contains("Step") && parentCollider != parentYellow) || (tagCollider == "Goal" && nameCollider != goal) || (tagCollider == "Start" && nameCollider != "YellowStart"))) destroyLine(parentYellow);
    }
}
