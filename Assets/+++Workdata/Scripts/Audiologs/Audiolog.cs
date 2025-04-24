using UnityEngine;

public class Audiolog : MonoBehaviour
{
	[SerializeField] private GameObject indicator;

	private Vector3 rotatePosition;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			indicator.SetActive(true);
			
			
		}
	}

	private void Update()
	{
		transform.LookAt(Camera.main.transform.position);
	}
	
	
}
