using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MowerController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile cutGrassTile;
    [SerializeField] Tile longGrassTile;
    [SerializeField] GameObject grassProjectilePrefab;
    [SerializeField] Sprite mowerSideSprite;
    [SerializeField] Sprite mowerFrontSprite;
    [SerializeField] Transform turretTopPos;
    [SerializeField] Transform turretBottomPos;
    [SerializeField] Transform turretLeftPos;
    [SerializeField] Transform turretRightPos;
    [SerializeField] GameObject moles;
    [SerializeField] GameObject finishMowingButton;
    [SerializeField] GameObject gameoverui;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject bulletHolder;
    Vector2Int dir = new Vector2Int(0, -1);
    float moveDelay = .2f;
    SpriteRenderer sr;
    Transform turret;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        turret = transform.GetChild(0);
        StartCoroutine(MoveMower());
    }

    IEnumerator MoveMower()
    {
        transform.Translate(new Vector3(dir.x, dir.y));
        Vector3Int tilePos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

        if (tilemap.GetTile(tilePos) != cutGrassTile)
        {
            if (tilemap.GetTile(tilePos) != longGrassTile)
            {
                FinishMowing();    
            }
            else
            {
                tilemap.SetTile(tilePos, cutGrassTile);
                FireGrass();
            }
        }

        yield return new WaitForSeconds(moveDelay);

        StartCoroutine(MoveMower());
    }
    
    public void FinishMowing()
    {
        Camera.main.orthographicSize = 10;
        Camera.main.transform.position = Vector3.zero - new Vector3(0, 0, 10);
        Destroy(moles);
        gameoverui.gameObject.SetActive(true);
        Destroy(finishMowingButton);
        Destroy(enemySpawner);
        Destroy(gameObject);
    }

    void Update()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 tdir = Input.mousePosition - Camera.main.WorldToScreenPoint(turret.transform.position);
        float angle = Mathf.Atan2(tdir.y, tdir.x) * Mathf.Rad2Deg;
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), Time.deltaTime * 20f);

        if (input.x > 0)
        {
            dir.x = 1;
            dir.y = 0;
            sr.flipY = false;
            sr.flipX = true;
            sr.sprite = mowerSideSprite;
            turret.position = turretRightPos.position;
        }
        else if (input.x < 0)
        {
            dir.x = -1;
            dir.y = 0;
            sr.flipX = false;
            sr.flipY = false;
            sr.sprite = mowerSideSprite;
            turret.position = turretLeftPos.position;
        }

        if (input.y > 0)
        {
            dir.y = 1;
            dir.x = 0;
            sr.flipY = true;
            sr.flipX = false;
            sr.sprite = mowerFrontSprite;
            turret.position = turretTopPos.position;
        }
        else if (input.y < 0)
        {
            dir.y = -1;
            dir.x = 0;
            sr.flipY = false;
            sr.flipX = false;
            sr.sprite = mowerFrontSprite;
            turret.position = turretBottomPos.position;
        }

    }

    void FireGrass()
    {
        var bullet = Instantiate(grassProjectilePrefab, turret.transform.position, Quaternion.identity);
        var brb = bullet.GetComponent<Rigidbody2D>();
        brb.AddForce((Input.mousePosition - Camera.main.WorldToScreenPoint(turret.transform.position)).normalized * 350);
    }
}
