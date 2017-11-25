using UnityEngine;
using System.Collections;

public class ShipMover : MonoBehaviour {
	[SerializeField]private GameObject _cube;
	[SerializeField]private float _speed = 3.0f;
	private Vector3 _startPos;
	private Vector3 _endPos;
    [SerializeField] private GameObject _cylinder;
    private float _cylinderMeshRadius;
    private Vector3 _cylinderCenterPosition;

    private void Update () {
		Vector3 dir = (_endPos - _cube.transform.position).normalized;
        Debug.DrawRay(_cylinderCenterPosition, Vector3.right * _cylinderMeshRadius);
        Vector3 steeringVector;
        Vector3 forwardVector = dir;
        var coliderPosition = _cube.transform.position + (forwardVector*2) ;
        Debug.DrawRay(_cube.transform.position, coliderPosition);
        // The main goal here is to avoid the red cylinder in the middle of the scene!
        // DO NOT use Unity's physics or navigation meshes.

       
        Vector3 vectorBetweenShipAndCylinderCenter = _cube.transform.position - _cylinderCenterPosition;
       


        if (((_cylinderCenterPosition)-(coliderPosition)).magnitude <= _cylinderMeshRadius)
        {
           
            if (Vector3.Dot(vectorBetweenShipAndCylinderCenter.normalized, forwardVector.normalized) <0){
                steeringVector = Vector3.Cross(dir, Vector3.up);
            }
            else{
                steeringVector = Vector3.Cross(Vector3.up, dir);
            }
            forwardVector += steeringVector;
            Debug.DrawRay(_cube.transform.position, steeringVector*100);
        }
        Debug.DrawRay(_cube.transform.position, ((_cylinderCenterPosition) - (coliderPosition * 50))*100);
        var perpendicular = Vector3.Cross(Vector3.up, forwardVector);
        _cube.transform.rotation = Quaternion.LookRotation(perpendicular);
        _cube.transform.position = _cube.transform.position + forwardVector * Time.deltaTime * _speed;
        // if cube is close to end point, reset
        if (Vector3.Distance(_endPos, _cube.transform.position) <= (dir * Time.deltaTime * _speed).magnitude) {
			ResetCube();
		}
	}

	private void ResetCube() {
		_startPos = new Vector3(4.5f, 0.0f, Random.Range(-4.5f, 4.5f));
		_endPos = _startPos;
		_endPos.x *= -1.0f;
		_endPos.z = Random.Range(-4.5f, 4.5f);
		_cube.transform.position = _startPos;
	}

	private void Start () {
        _cylinderMeshRadius = _cylinder.GetComponent<MeshFilter>().mesh.bounds.extents.x*5;
        
        _cylinderCenterPosition = _cylinder.transform.position;
        
        ResetCube();
    }
}
