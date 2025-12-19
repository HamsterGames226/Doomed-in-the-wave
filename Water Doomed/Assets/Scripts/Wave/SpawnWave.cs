using UnityEngine;

public class SpawnWave : MonoBehaviour
{
    public GameObject wavePrefab;

    public GameObject[] spawnPoints;

    public float timeToSpawn = 0.5f;

    private float currectTime = 0;
    void Update()
    {
        currectTime += Time.deltaTime;

        if(currectTime >= timeToSpawn && DieScript.life)
        {
            currectTime = 0;
            GameObject newWave = Instantiate(wavePrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position,Quaternion.identity);
            WaveMove waveMove = newWave.GetComponent<WaveMove>();
            waveMove.speedWave = Random.Range(2,5);
        }
    }
}
