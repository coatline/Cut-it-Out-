using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] GameObject moles;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile barrowedHTile;
    [SerializeField] Tile barrowedVTile;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        var tilePos = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
        tilePos += new Vector3(0, .25f, 0);

        var e = Instantiate(enemyPrefab, tilePos + new Vector3(0, 5, 0), Quaternion.identity, moles.transform);
        var s = Instantiate(shadowPrefab, tilePos, Quaternion.identity, moles.transform);

        var m = e.GetComponent<Mole>();
        m.shadow = s;
        m.tilemap = tilemap;
        m.barrowedTileHorizontal = barrowedHTile;
        m.barrowedTileVertical = barrowedVTile;
        m.targetDropPos = tilePos;

        yield return new WaitForSeconds(Random.Range(7.5f, 15));
        StartCoroutine(Spawn());
    }

    void Update()
    {

    }
}
