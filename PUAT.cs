using UnityEngine;
using System.Collections;

public class PUAT : MonoBehaviour
{
    [SerializeField] private float _distance, dropForwardForce, dropUpwardForce;
    [SerializeField] private Transform holdPos;
    public Transform _parent;
    public Vector3 halfExtents = new Vector3(0.3f, 2f, 0.3f);
    public float speed, rspeed;
    public float radius;
    public float wallDistance;
    GameObject currentWeapon;
    GameObject checkIfWeapon;
    private int maskbitWeapon = 1 << 3;
    private int maskbitWall = 1 << 6;
    bool isPicked;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) StartCoroutine(PickUp( ));
        if (Input.GetKeyDown(KeyCode.Q) && currentWeapon != null && Vector3.Distance(currentWeapon.transform.position, holdPos.transform.position) < 0.5f) 
        {
            Drop();
            StopAllCoroutines();
        }
    }
    private IEnumerator PickUp()
    {   
        RaycastHit[] hits = new RaycastHit[1];
        hits = Physics.SphereCastAll(_parent.position, radius, _parent.forward, _distance, maskbitWeapon, QueryTriggerInteraction.Ignore);
        if (hits.Length != 0) 
        {
            foreach (RaycastHit hit in hits) checkIfWeapon = hit.transform.gameObject;
            if (!Physics.Linecast(_parent.position, checkIfWeapon.transform.position, maskbitWall)) 
            {
                if (isPicked) Drop();
                foreach (RaycastHit hit in hits) currentWeapon = hit.transform.gameObject;
                {
                Debug.Log("Picked Up");
                while (Vector3.Distance(currentWeapon.transform.position, holdPos.position) > 0.001f)
                    {   
                        currentWeapon.transform.parent = _parent;
                        currentWeapon.GetComponent<BoxCollider>().enabled = false;
                        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
                        currentWeapon.transform.rotation = Quaternion.RotateTowards(currentWeapon.transform.rotation, holdPos.rotation, rspeed * Time.deltaTime);
                        currentWeapon.transform.position = Vector3.Lerp(currentWeapon.transform.position, holdPos.position, speed * Time.deltaTime);
                        isPicked = true;
                        currentWeapon.layer = 0;
                        yield return null;
                    } 
                }
            }  
            else { Debug.Log("Can't PickUp. There's a wall"); }
        }
    }
        private void Drop()
    {
        if (!Physics.Raycast(_parent.position, _parent.forward, wallDistance, maskbitWall))
        {
            Debug.Log("Dropped");
            isPicked = false;
            currentWeapon.GetComponent<BoxCollider>().enabled = true;
            Vector3 forceToAdd = _parent.forward * dropForwardForce + _parent.up * dropUpwardForce;
            currentWeapon.transform.parent = null;
            Rigidbody currentWeaponRig = currentWeapon.GetComponent<Rigidbody>();
            currentWeaponRig.isKinematic = false;
            currentWeaponRig.AddForce(forceToAdd, ForceMode.Impulse);
            float RNum = UnityEngine.Random.Range(-10f, 10f);
            currentWeaponRig.AddTorque(new Vector3(RNum, -30f, RNum));
            currentWeapon.layer = 3;
            currentWeapon = null;
        }
        else { Debug.Log("Can't Drop. There's a wall"); }
    }
    // ванёк чтобы визуализировать область в которой чел может подбирать оружие, расскоментируй строки ниже (выделить -> ctrl + /)
    // private void OnDrawGizmos() {
    //     if (checkIfWeapon != null) {
    //     Vector3 fwd = _parent.transform.TransformDirection(Vector3.forward);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(_parent.position, checkIfWeapon.transform.position);
    //     Gizmos.DrawWireCube(_parent.position + _parent.forward * _distance, halfExtents);
    //     }
    // }
}