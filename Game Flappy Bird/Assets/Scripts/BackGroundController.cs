using UnityEngine;


public class BackGroundController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField]Sprite[] img;
   
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = img[Random.Range(0, 2)];
    }

    // Update is called once per frame
    void Update()
    {
        float heightCamera = 2F * Camera.main.orthographicSize;
        float widthCamera = heightCamera * Camera.main.aspect;
        float heightImg = spriteRenderer.sprite.bounds.size.y;
        float widthImg = spriteRenderer.sprite.bounds.size.x;

        transform.localScale = new Vector3(widthCamera / widthImg,heightCamera /heightImg, 1f);
        transform.position = Vector3.zero;
    }
}
