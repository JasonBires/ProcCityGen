using UnityEngine;
using System.Collections;

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