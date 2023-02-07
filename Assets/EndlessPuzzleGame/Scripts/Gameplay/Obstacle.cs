using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Rigidbody2D obstacleRigidbody2D;

    float obstacleSpeed;
    bool createdNew = false;
    bool createdNewLine = false;
    float screenWidth, screenHeight, minGap, maxGap, verticalGap;
    float height, width;
    GameObject nextObstacle;

    //player has landed on plaform -> enable another jump
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.inAir = false;
    }

    public void InitOBstacle(Vector2 _position,float _obstacleSpeed, Color color, float _screenWidth, float _screenHeight, float _minHorizontalGap, float _maxHorizontalGap, float _verticalGap, float _height, float _width, bool _newLine)
    {
        transform.position = _position;
        obstacleSpeed = _obstacleSpeed;
        GetComponent<SpriteRenderer>().color = color;
        screenWidth = _screenWidth;
        screenHeight = _screenHeight;
        minGap = _minHorizontalGap;
        maxGap = _maxHorizontalGap;
        verticalGap = _verticalGap;
        height = _height;
        width = _width;
        createdNewLine = _newLine;
        transform.localScale = new Vector2(width, height);
    }

    void Update()
    {
        obstacleRigidbody2D.velocity = new Vector2(obstacleSpeed,0);

        if (!createdNewLine && transform.position.y < GameManager.Instance.camObject.transform.position.y + screenHeight)
        {
            NewLine();
        }

        if (obstacleSpeed < 0)
        {
            //if not created next obstacle create new when conditions go through
            if (!createdNew && transform.position.x < screenWidth)
            {
                NewObstacle();
            }

            //check when obstacle is over the screen to destroy it
            if (transform.position.x + width/2 < -screenWidth)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //if not created next obstacle create new when conditions go through
            if (!createdNew && transform.position.x > -screenWidth)
            {
                NewObstacle();
            }

            //check when obstacle is over the screen to destroy it
            if (transform.position.x - width / 2 > screenWidth)
            {
                Destroy(gameObject);
            }
        }

        if (transform.position.y + height / 2 < GameManager.Instance.camObject.transform.position.y - screenHeight) //destroy game object, when its bellow the bottom screen border
        {
            Destroy(gameObject);
        }
    }

    //create new obstacle outside of screen
    void NewObstacle()
    {
        nextObstacle = Instantiate(GameManager.Instance.obstaclePrefab);
        float tempWidth = Random.Range(GameManager.Instance.minObstacleWidth, GameManager.Instance.maxObstacleWidth);

        if (obstacleSpeed < 0)
            nextObstacle.GetComponent<Obstacle>().InitOBstacle(new Vector2(transform.position.x + Random.Range(minGap, maxGap) + width / 2 + tempWidth / 2, transform.position.y),obstacleSpeed,GameManager.Instance.GetRandomColor(),screenWidth, screenHeight, minGap, maxGap, verticalGap, height, tempWidth, createdNewLine);
        else
            nextObstacle.GetComponent<Obstacle>().InitOBstacle(new Vector2(transform.position.x - Random.Range(minGap, maxGap) - width / 2 - tempWidth / 2, transform.position.y), obstacleSpeed, GameManager.Instance.GetRandomColor(), screenWidth, screenHeight, minGap, maxGap, verticalGap, height, tempWidth, createdNewLine);

        createdNew = true;
    }

    //create new line of obstacles on top of current
    void NewLine()
    {
        createdNewLine = true;

        //let only one obstacle to create new line
        if (nextObstacle != null)
            return;

        GameObject tempObstacle = Instantiate(GameManager.Instance.obstaclePrefab);
        float tempWidth = Random.Range(GameManager.Instance.minObstacleWidth, GameManager.Instance.maxObstacleWidth);

        if (GameManager.Instance.lineNumber % 2 == 0)
            tempObstacle.GetComponent<Obstacle>().InitOBstacle(new Vector2(0, transform.position.y + GameManager.Instance.obstacleHeight + verticalGap), Random.Range(GameManager.Instance.minHorizontalSpeed, GameManager.Instance.maxHorizontalSpeed), GameManager.Instance.GetRandomColor(), screenWidth, screenHeight, minGap, maxGap, verticalGap, height, tempWidth, false);
        else
            tempObstacle.GetComponent<Obstacle>().InitOBstacle(new Vector2(0, transform.position.y + GameManager.Instance.obstacleHeight + verticalGap), Random.Range(-GameManager.Instance.maxHorizontalSpeed, -GameManager.Instance.minHorizontalSpeed), GameManager.Instance.GetRandomColor(), screenWidth, screenHeight, minGap, maxGap, verticalGap, height, tempWidth, false);

        GameManager.Instance.lineNumber++;
    }
}
