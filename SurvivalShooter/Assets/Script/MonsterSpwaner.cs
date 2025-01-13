using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public MonsterMove[] monsterPrefabs; // 몬스터 프리팹 배열 (0, 1, 2 각각의 몬스터 프리팹)
    public MonsterData[] datas; // 몬스터 데이터 배열 (0, 1, 2 각각의 데이터)
    public Transform[] spawnPoints; // 몬스터 스폰 포인트 배열

    private List<MonsterMove> monsters = new List<MonsterMove>(); // 현재 생성된 몬스터 목록
    public float spawnInterval = 5f; // 몬스터 스폰 간격 (초 단위)

    private GameManager gm; // 게임 매니저 참조
    private float lastSpawnTime; // 마지막으로 몬스터가 스폰된 시간

    private void Start()
    {
        // GameManager를 GameController 태그를 통해 찾음
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        lastSpawnTime = Time.time; // 시작 시 현재 시간을 마지막 스폰 시간으로 설정
    }

    private void Update()
    {
        // 게임 오버 상태라면 몬스터 스폰 중단
        if (gm != null && gm.IsGameOver)
        {
            return;
        }

        // 일정 시간 간격마다 몬스터 스폰
        if (Time.time >= lastSpawnTime + spawnInterval)
        {
            SpawnMonster();
            lastSpawnTime = Time.time; // 스폰 시간을 갱신
        }
    }

    private void SpawnMonster()
    {
        // 몬스터와 데이터의 연결: 배열의 동일한 인덱스를 사용
        int index = Random.Range(0, monsterPrefabs.Length); // 랜덤으로 몬스터 인덱스 선택
        var prefab = monsterPrefabs[index]; // 해당 인덱스의 몬스터 프리팹 선택
        var data = datas[index]; // 해당 인덱스의 몬스터 데이터 선택

        // 랜덤 스폰 포인트 선택
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 몬스터 생성
        var monster = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        monster.Setup(data); // 몬스터 데이터 설정
        monsters.Add(monster); // 생성된 몬스터를 목록에 추가

        //// 몬스터 사망 이벤트 연결
        //monster.onDeath += () => monsters.Remove(monster); // 몬스터가 죽으면 목록에서 제거
        //monster.onDeath += () => gm.AddScore(100); // 점수 추가
        //monster.onDeath += () => Destroy(monster.gameObject, 5f); // 죽은 몬스터를 5초 후 파괴

        // onDeath 이벤트 등록
        monster.onDeath += () =>
        {
            if (monster != null)
            {
                monsters.Remove(monster); // 리스트에서 제거
            }
        };

        monster.onDeath += () =>
        {
            if (gm != null)
            {
                gm.AddScore(100); // 점수 추가
            }
        };

        monster.onDeath += () =>
        {
            if (monster != null)
            {
                Destroy(monster.gameObject, 5f); // 마지막에 오브젝트 삭제
            }
        };
    }
}
