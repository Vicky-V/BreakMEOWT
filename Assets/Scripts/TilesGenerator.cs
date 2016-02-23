using UnityEngine;
using System.Collections;


public class TilesGenerator : MonoBehaviour 
{
    [SerializeField]
    private GameObject m_TilePrefab;

    [SerializeField]
    private Sprite m_SecretTileSprite;

    [SerializeField]
    private Sprite m_DefaultTileSprite;

    [SerializeField]
    private GameObject m_CharacterPrefab;
    
    [Header("Borders")]

    [SerializeField]
    private Transform m_TopLeftCorner;

    [SerializeField]
    private Transform m_BottomRightCorner;

    
    [Header("Top Row Tiles")]

    [SerializeField]
    private int m_TilesPerTopRow;

    [SerializeField]
    private Transform m_TopRowLeftBorder;

    [SerializeField]
    private Transform m_TopRowRightBorder;

    private int m_CharcaterSpawnTile;

    [Header ("Main Tiles")]

    [SerializeField]
    private int m_TilesPerRow;

    [SerializeField]
    private int m_NumberOfRows;

    
    private float m_TileWidth;
    private float m_TileHeight;

    private GameObject[] m_Tiles;
    private GameObject[] m_TopTiles;

    float m_TilesLeft;

	void Start () 
	{
        GameManager.Instance.Tiles = this;

        SetDefaultSprite();

        m_Tiles = new GameObject[m_TilesPerRow * m_NumberOfRows];
        m_TopTiles = new GameObject[m_TilesPerTopRow];

        m_CharcaterSpawnTile = UnityEngine.Random.Range(1, m_TilesPerTopRow);
		
		//GenerateTiles();
    }
	
    public void GenerateTiles()
    {
        //Top Row Generation
        float spacingHorizontal = (Mathf.Abs(m_TopRowLeftBorder.position.x - m_TopRowRightBorder.position.x) - m_TileWidth * m_TilesPerTopRow) / (m_TilesPerTopRow - 1);
        
        float newTileX = 0;
        float newTileY = m_TopRowLeftBorder.position.y;

        for (int x = 0; x < m_TilesPerTopRow; x++)
        {
            newTileX = m_TopRowLeftBorder.position.x + (spacingHorizontal + m_TileWidth) * x;

            m_TopTiles[x] = Instantiate(m_TilePrefab, new Vector3(newTileX, newTileY, 0), Quaternion.identity) as GameObject;
        }

        //Instantiate the character
        float charPosX = m_TopRowLeftBorder.position.x + (spacingHorizontal + m_TileWidth) * m_CharcaterSpawnTile;
        float charPosY =  newTileY + m_TileHeight + m_CharacterPrefab.GetComponent<Character>().DropOffset;
        GameManager.Instance.Kitty = Instantiate(m_CharacterPrefab, new Vector3(charPosX, charPosY, 0), Quaternion.identity) as GameObject;

        //Main Tiles Generation
		newTileX = 0;
		newTileY = 0;

		float areaWidth = Mathf.Abs (m_TopLeftCorner.position.x - m_BottomRightCorner.position.x);
		float areaHeight = Mathf.Abs (m_TopLeftCorner.position.y - m_BottomRightCorner.position.y);

		spacingHorizontal = (areaWidth - m_TileWidth * m_TilesPerRow) / (m_TilesPerRow - 1);
		float spacingVertical = (areaHeight - m_TileHeight * m_NumberOfRows) / (m_NumberOfRows - 1);

        for (int y = 0; y < m_NumberOfRows; y++)
        {
			for(int x = 0; x<m_TilesPerRow; x++)
			{
				newTileX = m_TopLeftCorner.position.x + (spacingHorizontal + m_TileWidth) * x;
				newTileY = m_TopLeftCorner.position.y + (spacingVertical + m_TileHeight) * y;

				m_Tiles[y*m_TilesPerRow + x] = Instantiate(m_TilePrefab, new Vector3(newTileX, newTileY, 0), Quaternion.identity) as GameObject;
			}

        }

        m_TilesLeft = m_TilesPerRow * m_NumberOfRows + m_TilesPerTopRow;
    }

    public void RemoveTile(GameObject tile)
    {
        for (int i = 0; i < m_Tiles.Length; i++)
        {
            if(m_Tiles[i]==tile)
            {
                m_Tiles[i] = null;
                m_TilesLeft--;
                return;
            }
        }

        for (int i = 0; i < m_TopTiles.Length; i++)
        {
            if (m_TopTiles[i] == tile)
            {
                m_TopTiles[i] = null;
                m_TilesLeft--;
                return;
            }
        }
    }

	public void ResetTiles()
    {
        for (int i = 0; i < m_TopTiles.Length; i++)
        {
            Destroy(m_TopTiles[i]);
        }

        for (int i = 0; i < m_Tiles.Length; i++)
        {
            Destroy(m_Tiles[i]);
        }
        

        GenerateTiles();
    }

    public void UnlockTheSuperDuperSecretSprite()
    {
        m_TilePrefab.GetComponent<SpriteRenderer>().sprite = m_SecretTileSprite;
        m_TileHeight = m_TilePrefab.GetComponent<SpriteRenderer>().sprite.border.y;
        m_TileWidth = m_TilePrefab.GetComponent<SpriteRenderer>().sprite.border.x;
    }

    public void SetDefaultSprite()
    {
        m_TilePrefab.GetComponent<SpriteRenderer>().sprite = m_DefaultTileSprite;
        m_TileHeight = m_TilePrefab.GetComponent<SpriteRenderer>().sprite.border.y;
        m_TileWidth = m_TilePrefab.GetComponent<SpriteRenderer>().sprite.border.x;
    }
}
