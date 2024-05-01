using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    // CSV 파일이 위치한 경로
    public TextAsset csvFilePath; // TextAsset 타입으로 변경

    // 맵에 사용될 타일 프리팹들
    public GameObject[] tilePrefabs;

    // 타일 사이의 간격
    public Vector3 tileSize = new Vector3(1f, 1f, 1f);

    // CSV 파일로부터 읽어온 맵 데이터를 저장할 리스트
    private List<List<int>> mapData = new List<List<int>>();

    // 맵을 생성하는 함수
    public void GenerateMap()
    {
        // 맵 데이터 리스트를 초기화
        mapData.Clear();

        // CSV 파일에서 맵 데이터를 읽어와서 리스트에 저장
        ReadMapFromCSV();

        // 읽어온 맵 데이터를 기반으로 실제 맵을 생성
        CreateMap();
    }

    // CSV 파일에서 맵 데이터를 읽어오는 함수
    private void ReadMapFromCSV()
    {
        // CSV 파일이 지정되지 않았을 경우 에러 메시지 출력 후 함수 종료
        if (csvFilePath == null)
        {
            Debug.LogError("CSV 파일이 지정되지 않았습니다.");
            return;
        }

        // CSV 파일의 내용을 문자열로 변환
        string csvText = csvFilePath.text;

        // 줄바꿈 문자를 기준으로 텍스트를 분할하여 행 데이터로 저장
        string[] lines = csvText.Split('\n');

        // 각 행을 순회하며 맵 데이터 리스트에 추가
        foreach (string line in lines)
        {
            // 쉼표를 기준으로 값을 분할하여 정수로 변환하여 리스트에 추가
            string[] values = line.Split(',');
            List<int> rowData = new List<int>();

            foreach (string value in values)
            {
                int tileType;
                if (int.TryParse(value, out tileType))
                {
                    rowData.Add(tileType);
                }
            }

            // 맵 데이터 리스트에 행 데이터 추가
            mapData.Add(rowData);
        }
    }

    // 실제 맵을 생성하는 함수
    private void CreateMap()
    {
        // 맵 데이터 리스트를 순회하면서 타일을 생성
        for (int rowIndex = 0; rowIndex < mapData.Count; rowIndex++)
        {
            List<int> rowData = mapData[rowIndex];

            for (int columnIndex = 0; columnIndex < rowData.Count; columnIndex++)
            {
                int tileIndex = rowData[columnIndex];

                // 스위치 문을 사용하여 타일 타입에 따라 처리
                switch (tileIndex)
                {
                    case 0:
                        // 타일 타입이 0인 경우 아무 처리도 하지 않음
                        break;
                    case 1:
                        // 타일 타입이 1인 경우 첫 번째 프리팹을 생성하고 위치 설정
                        CreateTile(0, columnIndex, rowIndex);
                        break;
                    case 2:
                        // 타일 타입이 2인 경우 두 번째 프리팹을 생성하고 위치 설정
                        CreateTile(1, columnIndex, rowIndex);
                        break;
                    default:
                        // 그 외의 타일 타입에 대한 처리 (현재는 처리하지 않음)
                        break;
                }
            }
        }
    }

    // 타일을 생성하고 위치 설정하는 함수
    private void CreateTile(int prefabIndex, int x, int z)
    {
        // 선택한 타일 프리팹을 가져옴
        GameObject tilePrefab = tilePrefabs[prefabIndex];

        // 타일의 위치 계산 (y 값에 1을 빼줌)
        Vector3 tilePosition = new Vector3(x * tileSize.x - 25f, 0.5f, z * tileSize.z - 25f);

        // 타일을 생성하고 위치 설정
        GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
        tile.transform.parent = transform; // 부모 설정
    }

    // 게임이 시작될 때 호출되는 함수
    private void Start()
    {
        // 맵 생성 함수 호출
        GenerateMap();
    }
}
