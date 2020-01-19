using UnityEngine;
using System.Collections;

public class DungeonCamera : MonoBehaviour 
{
	public Transform target;
	
	public float zoomSpeed = 10.0f;
	public float distanceLerpSpeed = 2.0f;
	public float currentDistance = 8.0f;
	public float distance = 8.0f;
	public float minDistance = 8.0f;
	public float maxDistance = 13.0f;
	public float minAngleX = 20;
	public float maxAngleX = 45;
	public float xAngle = 45;
	public float yAngle = 45;
	
	public float noMoveRadius = 0.5f;
	public float dampSpeed = 5.5f;
	
	public float focusHeightOffset = 0.0f;
	
	private Vector3 lookingPosition = Vector3.zero; 
	private Vector3 dampLookingPosition = Vector3.zero;
	
	
	#if UNITY_IPHONE || UNITY_ANDROID	
	private Vector2 oldTouch0Pos,oldTouch1Pos;
	private bool isMoved = false;
	
	#endif
	

	void Update() 
	{
		if (target == null)
		{
			return;
		}
		
		Vector3 moveDir = lookingPosition - target.position;
		if (moveDir.magnitude > noMoveRadius * 5)
		{
			lookingPosition = target.position;
			dampLookingPosition = lookingPosition;
		}
		else if (moveDir.magnitude > noMoveRadius * 2)
		{
			lookingPosition = target.position + moveDir.normalized * noMoveRadius;
			
			dampLookingPosition = Vector3.Lerp(dampLookingPosition, lookingPosition, Mathf.Pow(dampSpeed, moveDir.magnitude/noMoveRadius)  * Time.deltaTime);
		}
		else if (moveDir.magnitude > noMoveRadius)
		{
			lookingPosition = target.position + moveDir.normalized * noMoveRadius;
			dampLookingPosition = Vector3.Lerp(dampLookingPosition, lookingPosition, dampSpeed * Time.deltaTime);
		}
		else
		{
			dampLookingPosition = Vector3.Lerp(dampLookingPosition, lookingPosition, dampSpeed * Time.deltaTime);
		}

		float zoomDelta = GetZoomDelta();
		distance = Mathf.Clamp(distance + zoomDelta, minDistance, maxDistance);
	}
	
	void LateUpdate() 
	{
		if (target == null)
		{
			return;
		}
		
		currentDistance = Mathf.Lerp(currentDistance, distance, distanceLerpSpeed*Time.deltaTime);

		float factor = 0;
		if (maxDistance == minDistance)
		{
			factor = 100;
		}
		else
		{
			factor = (currentDistance - minDistance) / (maxDistance - minDistance);
		}
		xAngle = Mathf.Lerp(minAngleX, maxAngleX, factor);
		
		float xzDistance = currentDistance * Mathf.Cos(xAngle*Mathf.Deg2Rad);
		float zDistance = xzDistance * Mathf.Cos(yAngle*Mathf.Deg2Rad);
		float xDistance = xzDistance * Mathf.Sin(yAngle*Mathf.Deg2Rad);
		float yDistance = currentDistance * Mathf.Sin(xAngle*Mathf.Deg2Rad) + focusHeightOffset;
		
		Vector3 offset = new Vector3(xDistance,yDistance,zDistance);
		
		transform.position = dampLookingPosition + offset;
		
		Vector3 lookAt = dampLookingPosition;
		lookAt.y += focusHeightOffset;
		transform.LookAt (lookAt);
	}
	
	float GetZoomDelta() 
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		float zoomDelta = 0.0f;
		if(Input.touchCount>1){
			Vector2 touch0Pos = Input.GetTouch(0).position;
			Vector2 touch1Pos = Input.GetTouch(1).position;			
			if(Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began){
				oldTouch0Pos = touch0Pos;
				oldTouch1Pos = touch1Pos;
				isMoved = true;
			}
			if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended){
				isMoved = false;
			}
			if(isMoved && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)){
				if(isEnlarge(oldTouch0Pos,oldTouch1Pos,touch0Pos,touch1Pos)){
					zoomDelta = 0.1f;
				}else{
					zoomDelta = -0.1f;
				}
				oldTouch0Pos = touch0Pos;
				oldTouch1Pos = touch1Pos;
			}					
		}
		return zoomDelta;
		#else
		return Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
		#endif		
	}
	
	private bool isEnlarge(Vector2 op1,Vector2 op2,Vector2 p1,Vector2 p2)
	{
		float ods = Mathf.Abs(Vector2.Distance(op1,op2));
		float ds = Mathf.Abs(Vector2.Distance(p1,p2));
		if (ds > ods)
		{
			return true;
		}
		return false;
	}
	
	void SetCameraTarget(Transform target) 
	{
		this.lookingPosition = target.position;
		this.dampLookingPosition = lookingPosition;
		this.target = target;
	}
}
