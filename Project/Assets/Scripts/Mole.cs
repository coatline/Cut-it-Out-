using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mole : MonoBehaviour
{
    public Tile barrowedTileVertical;
    public Tile barrowedTileHorizontal;
    public Tilemap tilemap;
    public Vector3 targetDropPos;
    public GameObject shadow;
    bool dropped = false;
    SpriteRenderer sr;
    BoxCollider2D bc;
    int numMoves;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void ChoosePos()
    {
        numMoves = Random.Range(2, 10);
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        if (numMoves > 0)
        {
            int x = (int)Random.Range(-1, 2);
            int y = (int)Random.Range(-1, 2);

            if (x != 0)
            {
                y = 0;
            }

            var dir = new Vector2(x, y);

            transform.Translate(dir);

            Vector3Int tilePos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if (dir.y != 0)
            {
                tilemap.SetTile(tilePos, barrowedTileVertical);
            }
            else
            {
                tilemap.SetTile(tilePos, barrowedTileHorizontal);
            }

            numMoves--;
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            ChoosePos();
        }

        yield return new WaitForSeconds(.5f);
        StartCoroutine(Move());
    }

    void Update()
    {
        if (!dropped)
        {
            if (Vector3.Distance(transform.position, targetDropPos) > .25f)
            {
                transform.position = Vector3.MoveTowards(transform.position, shadow.transform.position, Time.deltaTime);
            }
            else
            {
                sr.enabled = false;
                bc.enabled = false;
                Destroy(shadow);
                ChoosePos();
                dropped = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            Destroy(shadow);
            Destroy(gameObject);
        }
    }
}
