using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffect : MonoBehaviour {
	[SerializeField]
	private GameObject _prefab;

	private void OnDestroy()
	{
		Instantiate(_prefab, transform.position, transform.rotation);

	}
}
