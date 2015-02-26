using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/*
 * Class for holding turtle drawer state for use with stack
 * Composed of an angle and a position, getters and setters for both
 */
public class TurtleState {
	private int angle;
	private Vector3 position;

	public TurtleState(int angle, Vector3 pos) {
		this.angle = angle;
		this.position = pos;
	}

	public void setAngle(int angle) {
		this.angle = angle;
	}
	public void setPosition (Vector3 pos) {
		this.position = pos;
	}
	public int getAngle() {
		return angle;
	}
	public Vector3 getPosition() {
		return position;
	}
}

/*
 * The main L system creator class
 */
public class l_system : MonoBehaviour {
	public string axiom;
	public string[] rules;
	public int iterations;
	public int angle; //Allows user to set angle delta for making turns
	public int stochasticAngle; //Allows user to specify randomness present in angle
	private int curAngle; //Angle the turtle drawer is currently pointing in
	private Stack<TurtleState> turtleStack; //stack for turtle drawing movements

	// Use this for initialization
	void Start () {
		this.turtleStack = new Stack<TurtleState>();
		for (int i = 0; i < iterations; i++) {
			axiom = applyRules (axiom, rules);
		}
		axiomInterpreter (axiom);
	}

	/*
	 * Generate the final string from the rules and starting axiom we've been given
	 */
	string applyRules(string axi, string[] rules) {
		char[] ax = axi.ToCharArray ();
		string[] results = new string[ax.Length];
		for (int i = 0; i < rules.Length; i++) {
			string[] arrResults = rules[i].Split(':');
			string transformer = arrResults[1]; //The rules for the given symbol
			string toTransform = arrResults[0]; //The given symbol
			for (int j = 0; j < ax.Length; j++) {
				if (ax[j].ToString().Equals(toTransform)) {
					results[j] = transformer;   //Apply the rules to the symbol
				}
			}
		}
		return string.Concat (results);
	}
	//create the physical representation
	void axiomInterpreter(string axi) {
		Stopwatch timer = new Stopwatch ();
		timer.Start ();

		Vector3 curLoc = new Vector3 (0, 0, 0);
		for (int i = 0; i < axi.Length; i++) {
			if (axi.ToCharArray ()[i].ToString().Equals("-")) {
				int rand = Random.Range(-this.stochasticAngle, this.stochasticAngle);
				this.curAngle -= this.angle + rand;
			}
			else if (axi.ToCharArray()[i].ToString().Equals ("+")) {
				int rand = Random.Range(-this.stochasticAngle, this.stochasticAngle);
				this.curAngle += this.angle + rand;
			}
			else if (axi.ToCharArray()[i].ToString().Equals ("(")) {
				this.turtleStack.Push (new TurtleState(this.curAngle, curLoc));
			}
			else if (axi.ToCharArray ()[i].ToString ().Equals(")")) {
				TurtleState temp = this.turtleStack.Pop();
				this.curAngle = temp.getAngle();
				curLoc = temp.getPosition ();
			}
			else if (axi.ToCharArray ()[i].ToString ().Equals ("X")) {

			}
			else {
				for (int j = 0; j < 5; j++) {
					GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					sphere.transform.localScale = new Vector3(1f, 1f, 1f);
					sphere.transform.Rotate(0, this.curAngle, 0);
					sphere.transform.position = curLoc;
					Color matColor = colorGen (curLoc.x, curLoc.y, this.curAngle);
					sphere.renderer.material.color = matColor;
					Transform temp = sphere.transform;
					temp.Translate(0, 0, 1);
					curLoc = temp.position; //TODO: don't reallocate memory every time
				}
			}
			timer.Stop();
			UnityEngine.Debug.Log (timer.ElapsedTicks + ": time taken :" + timer.ElapsedMilliseconds);
		}
	}

	public Color colorGen(float r, float g, float b) {
		return new Color((Mathf.Abs(g) % 255)/255f, (Mathf.Abs(g) % 255)/255f, ((b) % 255)/255f);
	}
}
				                  