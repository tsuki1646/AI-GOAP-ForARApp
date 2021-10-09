using UnityEngine;

public class Spawn : MonoBehaviour {

    // Grab our prefab
    public GameObject customerPrefab;
    private GameObject childGO;
    // Number of patients to spawn
    public int numCustomers;
    // A bool to control if you want spawning or not
    public bool keepSpawning = false;

    void Awake()
    {
        GameObject childGO = customerPrefab.transform.GetChild(4).gameObject;
    }

    void Start() {

        for (int i = 0; i < numCustomers; ++i) {

            // Instantiate numCustomers at the spawner
            Instantiate(customerPrefab, this.transform.position, Quaternion.identity);
            //Renderer rend = childGO.GetComponent<Renderer>();
            //rend.material.color = Color.black;
        }
        // Call the SpawnCustomer method for the first time
        if (keepSpawning) {

            Invoke("SpawnCustomer", 10.0f);
        }
    }

    void SpawnCustomer() {

        // Instantiate numCustomers at the spawner
        Instantiate(customerPrefab, this.transform.position, Quaternion.identity);
        // Invoke this method at random intervals
        Invoke("SpawnCustomer", Random.Range(15.0f, 25.0f));
    }

    // Update is called once per frame
    void Update() {

    }
}
