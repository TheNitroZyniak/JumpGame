using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Levels : MonoBehaviour{
    [SerializeField] GameObject obstacle, hole, enemy, saw, staticPlatform, player, rewardedVideo, rotatePlatform, bomb, mace;
    private GameObject[] obstacles = new GameObject[20];
    private GameObject[] holes = new GameObject[10];
    private GameObject[] enemies = new GameObject[10];
    private GameObject[] saws = new GameObject[10];
    private GameObject[] rotatePlatforms = new GameObject[6];
    private GameObject[] bombs = new GameObject[6];
    private GameObject[] maces = new GameObject[6];
    
    public GameObject[] platforms;

    private GameObject[] staticPlatforms = new GameObject[10];

    List<int> levels = new List<int>();

    public GameObject[] sideObstacles;

    private int currentLevel = 1;
    public static int level = 0;

    private TextMesh text;
    public Text levelText;
    float y = 0;

    Rigidbody2D playerRb;
    public Camera _camera;
    //public AdManager adsManager;



    private void Start() {
        playerRb = player.GetComponent<Rigidbody2D>();
        text = GetComponent<TextMesh>();

        for (int i = 0; i < obstacles.Length; i++) obstacles[i] = Instantiate(obstacle, new Vector3(-8, -8, -0.5f), Quaternion.identity);        
        for (int i = 0; i < holes.Length; i++) holes[i] = Instantiate(hole, new Vector3(-10, -10, -0.5f), Quaternion.identity);        
        for (int i = 0; i < enemies.Length; i++) enemies[i] = Instantiate(enemy, new Vector3(-12, -12, -0.5f), Quaternion.identity);        
        for (int i = 0; i < saws.Length; i++) saws[i] = Instantiate(saw, new Vector3(-14, -14, -0.5f), Quaternion.identity);        
        for (int i = 0; i < staticPlatforms.Length; i++) staticPlatforms[i] = Instantiate(staticPlatform, new Vector3(-16, -16, -0.5f), Quaternion.identity);        
        for (int i = 0; i < maces.Length; i++) maces[i] = Instantiate(mace, new Vector3(-18, -18, -0.5f), Quaternion.identity);       

        for (int i = 0; i < rotatePlatforms.Length; i++) {
            rotatePlatforms[i] = Instantiate(rotatePlatform, new Vector3(-20, -20, -0.5f), Quaternion.identity);
            bombs[i] = Instantiate(bomb, new Vector3(-18, -18, -0.5f), Quaternion.identity);
        }

        for (int i = 1; i < 9; i++) {
            levels.Add(i);
        }
    }



    public int CreateLevel(float height) {
        
        if(levels.Count==0)for(int i = 1; i < 9; i++) levels.Add(i);
        level = RandomLevel();
        
        transform.position = new Vector3(0, height, 0);
        y = height + 5;
        float z = -0.5f;
        int levelLength = 0;
        text.text = currentLevel.ToString();
        levelText.text = currentLevel.ToString() + "/10";
        AllSetActive();

        if (level == 1) {
            List<float> pos_x = new List<float>() {0.5f, 1.5f, -2,   -1, 0,    2, 1,    0, -1.5f, 1.5f, 1.5f,  0, -2,   2, -1.25f, 1.25f};
            List<float> pos_y = new List<float>() {0,    0.5f,  3, 3.5f, 4, 6.5f, 7, 7.5f,    10,   10,   14, 18, 22,  22,  22.5f, 22.5f};
            for (int i = 0; i < pos_y.Count; i++) pos_y[i] += y;
            List<float> rot = new List<float>()   {0,       0,  0,    0, 0,    0, 0,    0,     0,    0,    0,  0, 45, -45,     22,   -22};

            SetPosition(obstacles, pos_x, pos_y, rot);

            holes[0].transform.SetPositionAndRotation(new Vector3(0, y + 14, z), Quaternion.identity);
            enemies[0].transform.SetPositionAndRotation(new Vector3(-1.5f, y + 14, z), Quaternion.identity);
            enemies[0].GetComponent<Enemy>().startPosition = enemies[0].transform.position;
            enemies[0].GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
            obstacles[11].GetComponent<Obstacle>().moving = true;

            levelLength = 30;
        }

        

        if(level == 2) {
            float x = -2;
            float y1 = y;
            for (int i = 0; i < 6; i++) {
                saws[i].GetComponent<Saw>().moving = true;
                saws[i].transform.position = new Vector3(x, y1, z);
                if (x > 0) saws[i].GetComponent<Rigidbody2D>().angularVelocity = 120;
                else saws[i].GetComponent<Rigidbody2D>().angularVelocity = -120;
                saws[i].GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
                x = -x;
                y1 += 4;
            }
            levelLength = 30;
        }

        if (level == 3) {
            sideObstacles[0].transform.position = new Vector3(-2.71f, y + 7.5f, sideObstacles[0].transform.position.z);
            sideObstacles[1].transform.position = new Vector3(2.71f, y + 7.5f, sideObstacles[1].transform.position.z);

            sideObstacles[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0.125f, 0);
            sideObstacles[1].GetComponent<Rigidbody2D>().velocity = new Vector2(-0.125f, 0);
            levelLength = 20;
        }
        

        if(level == 4) {
            List<float> pos_x = new List<float>() { -1.5f, 1.5f,  0,  2, -1.5f, 1.5f,  0, -2};
            List<float> pos_y = new List<float>() {     0,    0,  4,  4,     8,    8, 12, 12};
            for (int i = 0; i < pos_y.Count; i++) pos_y[i] += y;
            List<float> rot = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0};

            SetPosition(holes, pos_x, pos_y, rot);

            levelLength = 25;
        }

        
        if(level == 5) {
            float y1 = 10;
            for(int i = 0; i < saws.Length; i++) {
                saws[i].transform.SetPositionAndRotation(new Vector3(Random.Range(-2,2), y + y1, z), Quaternion.identity);
                saws[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2); 
                saws[i].GetComponent<Rigidbody2D>().angularVelocity = 120;          
                levelLength = 30;
                y1 += 5;
            }
        }
        

        if (level == 6) {
            List<float> pos_x = new List<float>() { -2.5f, -1.5f, -0.5f, 0.5f, 2.5f, -2.5f, -0.5f, 0.5f, 1.5f, 2.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f };
            List<float> pos_y = new List<float>() {     0,     0,     0,    0,    0,     5,     5,    5,    5,    5,    10,    10,    10,   10,   10 };
            for (int i = 0; i < pos_y.Count; i++) pos_y[i] += y;
            List<float> rot = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            SetPosition(obstacles, pos_x, pos_y, rot);

            enemies[0].transform.SetPositionAndRotation(new Vector3(1.5f, y, z), Quaternion.identity);
            enemies[1].transform.SetPositionAndRotation(new Vector3(-1.5f, y + 5, z), Quaternion.identity);
            enemies[2].transform.SetPositionAndRotation(new Vector3(2.5f, y + 10, z), Quaternion.identity);

            obstacles[11].GetComponent<Obstacle>().moving = false;
            
            for (int i = 0; i < 3; i++) {
                enemies[i].GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
                enemies[i].GetComponent<Enemy>().startPosition = enemies[i].transform.position;
            }

            levelLength = 25;

        }
        

        if (level == 7) {
            float x = -2.4f, y1 = 0, r = -20;
            for(int i  = 0; i < rotatePlatforms.Length; i++) {
                rotatePlatforms[i].transform.SetPositionAndRotation(new Vector3(x, y + y1, z), Quaternion.Euler(0, 0, r));
                bombs[i].transform.SetPositionAndRotation(new Vector3(x, y + y1 + 1, z), Quaternion.identity);
                x = -x;
                y1 += 4;
                r = -r;
            }


            levelLength = 30;
        }

        if(level == 8) {
            float y1 = 0;
            for(int i = 0; i < maces.Length; i++) {
                maces[i].transform.SetPositionAndRotation(new Vector3(Random.Range(-1.8f, 1.8f), y + y1, z), Quaternion.identity);
                int a = Random.Range(0, 2);

                if(a == 0) maces[i].GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-4f, -3f), 0);
                else maces[i].GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(3f, 4f), 0);

                y1 += 4.5f;
            }
            levelLength = 40;
        }
       
        if(level == 9) {

        }

        /*

        if (level == 9) {
            staticPlatforms[0].transform.SetPositionAndRotation(new Vector3(0, height2, z), Quaternion.identity);

            staticPlatforms[1].transform.SetPositionAndRotation(new Vector3(1.5f, height2 + 1.5f, z), Quaternion.identity);

            staticPlatforms[2].transform.SetPositionAndRotation(new Vector3(-1, height2 + 3, z), Quaternion.identity);

            staticPlatforms[3].transform.SetPositionAndRotation(new Vector3(1, height2 + 4.5f, z), Quaternion.identity);

            staticPlatforms[4].transform.SetPositionAndRotation(new Vector3(2, height2 + 6, z), Quaternion.identity);

            staticPlatforms[5].transform.SetPositionAndRotation(new Vector3(-2, height2 + 7.5f, z), Quaternion.identity);

            staticPlatforms[6].transform.SetPositionAndRotation(new Vector3(0, height2 + 9f, z), Quaternion.identity);

            levelLength = 20;
        }
        */

        currentLevel++;
        return (int)y + levelLength;
    }



    void SetPosition(GameObject[] obj, List<float> pos_x, List<float> pos_y, List<float> rot) {
        for(int i = 0; i < pos_x.Count; i++) {
            obj[i].transform.SetPositionAndRotation(new Vector3(pos_x[i], pos_y[i], -0.5f), Quaternion.Euler(0,0, rot[i]));

        }
    }




    public void RewardVideo() {
        rewardedVideo.SetActive(true);
        StartCoroutine(Slider());
    }

    bool isUp = false;

    private void Update() {
        if (level == 4) {
            if (playerRb.velocity.y > 0 && !isUp) {
                for (int i = 0; i < staticPlatforms.Length; i++) {
                    staticPlatforms[i].GetComponent<Collider2D>().isTrigger = true;
                }
                isUp = true;
            } 
            
            else if(playerRb.velocity.y < 0 && isUp) {
                for (int i = 0; i < staticPlatforms.Length; i++) {
                    staticPlatforms[i].GetComponent<Collider2D>().isTrigger = false;
                }
                isUp = false;

            }
        }

        if(level == 7) {
            for(int i = 0; i < bombs.Length; i++) {
                if (player.transform.position.y + 5 > bombs[i].transform.position.y && player.transform.position.y < bombs[i].transform.position.y) {
                    bombs[i].GetComponent<Rigidbody2D>().gravityScale = 1;
                }
            }
            
        }

    }

    public Transform ShootingToEnemy(Transform player) {
        float len = Vector3.Distance(player.position, enemies[0].transform.position);
        int enemyToShoot = 0;
        for(int i = 1; i < enemies.Length; i++) {
            if (Vector3.Distance(player.position, enemies[i].transform.position) < len) {
                len = Vector3.Distance(player.position, enemies[i].transform.position);
                enemyToShoot = i;

            }
        }

        return enemies[enemyToShoot].transform; 
    }

    int RandomLevel() {
        int temp = Random.Range(0, levels.Count);
        int znach = levels[temp];
        levels.Remove(levels[temp]);
        return znach;
    }

    void AllSetActive() {
        for(int i = 0; i < enemies.Length; i++) {
            enemies[i].SetActive(true);
        }
    }

    IEnumerator Slider() {
        float a = 1f;
        RectTransform rt = rewardedVideo.transform.GetChild(3).gameObject.GetComponent<RectTransform>();
        while (a > 0) {

            rt.sizeDelta = new Vector2((int)(500 * a), 50);
            a -= 0.005f;
            yield return new WaitForFixedUpdate();
        }
        if(Player.death) SceneManager.LoadScene("Menu");
    }

    public void Continue() {
        rewardedVideo.SetActive(false);
        currentLevel--;

        //adsManager.PlayRewardedVideoAd();

        for (int i = 0; i < obstacles.Length; i++) obstacles[i].transform.SetPositionAndRotation(new Vector3(-8, -8, -0.5f), Quaternion.identity);        
        for (int i = 0; i < holes.Length; i++) holes[i].transform.SetPositionAndRotation(new Vector3(-10, -10, -0.5f), Quaternion.identity);       
        for (int i = 0; i < enemies.Length; i++) enemies[i].transform.SetPositionAndRotation(new Vector3(-12, -12, -0.5f), Quaternion.identity);      
        for (int i = 0; i < saws.Length; i++) saws[i].transform.SetPositionAndRotation(new Vector3(-14, -14, -0.5f), Quaternion.identity);      
        for (int i = 0; i < sideObstacles.Length; i++) sideObstacles[i].transform.SetPositionAndRotation(new Vector3(-40, -40, -0.5f), Quaternion.identity);      
        for (int i = 0; i < staticPlatforms.Length; i++) staticPlatforms[i].transform.SetPositionAndRotation(new Vector3(-16, -16, -0.5f), Quaternion.identity);       
        for (int i = 0; i < rotatePlatforms.Length; i++) {
            rotatePlatforms[i].transform.SetPositionAndRotation(new Vector3(-18, -18, -0.5f), Quaternion.identity);
            bombs[i].transform.SetPositionAndRotation(new Vector3(-18, -18, -0.5f), Quaternion.identity);
        }

        GameManager.levelHeight = CreateLevel(_camera.transform.position.y + 3);

        player.transform.SetPositionAndRotation(new Vector3(0, _camera.transform.position.y - 1.4f, -0.5f), Quaternion.identity);
        player.transform.localScale = new Vector3(0.075f, 0.075f, 1);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        player.GetComponent<Collider2D>().isTrigger = false;

        platforms[0].transform.SetPositionAndRotation(new Vector3(0, _camera.transform.position.y - 4, platforms[0].transform.position.z), Quaternion.identity);
        platforms[0].transform.localScale = new Vector3(2, 0.2f, 1);
        platforms[1].transform.SetPositionAndRotation(new Vector3(-1, _camera.transform.position.y - 4, platforms[1].transform.position.z), Quaternion.identity);
        platforms[2].transform.SetPositionAndRotation(new Vector3(1, _camera.transform.position.y - 4, platforms[2].transform.position.z), Quaternion.identity);

        Player.death = false;       
    }
}
