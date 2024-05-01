using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    // CSV ������ ��ġ�� ���
    public TextAsset csvFilePath; // TextAsset Ÿ������ ����

    // �ʿ� ���� Ÿ�� �����յ�
    public GameObject[] tilePrefabs;

    // Ÿ�� ������ ����
    public Vector3 tileSize = new Vector3(1f, 1f, 1f);

    // CSV ���Ϸκ��� �о�� �� �����͸� ������ ����Ʈ
    private List<List<int>> mapData = new List<List<int>>();

    // ���� �����ϴ� �Լ�
    public void GenerateMap()
    {
        // �� ������ ����Ʈ�� �ʱ�ȭ
        mapData.Clear();

        // CSV ���Ͽ��� �� �����͸� �о�ͼ� ����Ʈ�� ����
        ReadMapFromCSV();

        // �о�� �� �����͸� ������� ���� ���� ����
        CreateMap();
    }

    // CSV ���Ͽ��� �� �����͸� �о���� �Լ�
    private void ReadMapFromCSV()
    {
        // CSV ������ �������� �ʾ��� ��� ���� �޽��� ��� �� �Լ� ����
        if (csvFilePath == null)
        {
            Debug.LogError("CSV ������ �������� �ʾҽ��ϴ�.");
            return;
        }

        // CSV ������ ������ ���ڿ��� ��ȯ
        string csvText = csvFilePath.text;

        // �ٹٲ� ���ڸ� �������� �ؽ�Ʈ�� �����Ͽ� �� �����ͷ� ����
        string[] lines = csvText.Split('\n');

        // �� ���� ��ȸ�ϸ� �� ������ ����Ʈ�� �߰�
        foreach (string line in lines)
        {
            // ��ǥ�� �������� ���� �����Ͽ� ������ ��ȯ�Ͽ� ����Ʈ�� �߰�
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

            // �� ������ ����Ʈ�� �� ������ �߰�
            mapData.Add(rowData);
        }
    }

    // ���� ���� �����ϴ� �Լ�
    private void CreateMap()
    {
        // �� ������ ����Ʈ�� ��ȸ�ϸ鼭 Ÿ���� ����
        for (int rowIndex = 0; rowIndex < mapData.Count; rowIndex++)
        {
            List<int> rowData = mapData[rowIndex];

            for (int columnIndex = 0; columnIndex < rowData.Count; columnIndex++)
            {
                int tileIndex = rowData[columnIndex];

                // ����ġ ���� ����Ͽ� Ÿ�� Ÿ�Կ� ���� ó��
                switch (tileIndex)
                {
                    case 0:
                        // Ÿ�� Ÿ���� 0�� ��� �ƹ� ó���� ���� ����
                        break;
                    case 1:
                        // Ÿ�� Ÿ���� 1�� ��� ù ��° �������� �����ϰ� ��ġ ����
                        CreateTile(0, columnIndex, rowIndex);
                        break;
                    case 2:
                        // Ÿ�� Ÿ���� 2�� ��� �� ��° �������� �����ϰ� ��ġ ����
                        CreateTile(1, columnIndex, rowIndex);
                        break;
                    default:
                        // �� ���� Ÿ�� Ÿ�Կ� ���� ó�� (����� ó������ ����)
                        break;
                }
            }
        }
    }

    // Ÿ���� �����ϰ� ��ġ �����ϴ� �Լ�
    private void CreateTile(int prefabIndex, int x, int z)
    {
        // ������ Ÿ�� �������� ������
        GameObject tilePrefab = tilePrefabs[prefabIndex];

        // Ÿ���� ��ġ ��� (y ���� 1�� ����)
        Vector3 tilePosition = new Vector3(x * tileSize.x - 25f, 0.5f, z * tileSize.z - 25f);

        // Ÿ���� �����ϰ� ��ġ ����
        GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
        tile.transform.parent = transform; // �θ� ����
    }

    // ������ ���۵� �� ȣ��Ǵ� �Լ�
    private void Start()
    {
        // �� ���� �Լ� ȣ��
        GenerateMap();
    }
}
