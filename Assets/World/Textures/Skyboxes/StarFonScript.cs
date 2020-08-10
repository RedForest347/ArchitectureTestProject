using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StarLayer
{
    [HideInInspector] public string ShaderTextureName;
    public float Speed;
    //public float Approximation;
    [HideInInspector] public Vector2 originalTextureSize;

    public StarLayer(string name)
    {
        ShaderTextureName = name;
    }
}

public class StarFonScript : MonoBehaviour
{
    public Material StarsMaterial;
    public Camera cam;
    Vector3 originalStarFonSize;

    public StarLayer Background = new StarLayer("_Background");
    public StarLayer StarDust = new StarLayer("_SpaceDust");
    public StarLayer SmallStars = new StarLayer("_SmallStars");
    public StarLayer MediumStars = new StarLayer("_MediumStars");
    public StarLayer BigStars = new StarLayer("_BigStars");

    void GetOriginalTextureSize(StarLayer StarLayer)
    {
        StarLayer.originalTextureSize = StarsMaterial.GetTextureScale(StarLayer.ShaderTextureName);
    }

    void Start()
    {
        originalStarFonSize = transform.localScale;
        GetOriginalTextureSize(Background);
        GetOriginalTextureSize(StarDust);
        GetOriginalTextureSize(SmallStars);
        GetOriginalTextureSize(MediumStars);
        GetOriginalTextureSize(BigStars);
    }

    void StarFonUpdate(StarLayer StarLayer)
    {
        Vector2 Offset = new Vector2(0, 0);

        /*float Tiling = 20 / StarLayer.Approximation + (1 / cam.orthographicSize * StarLayer.Approximation);
        Offset += new Vector2(StarsMaterial.GetTextureScale(StarLayer.ShaderTextureName).x * -0.5f + 0.5f ,
                              StarsMaterial.GetTextureScale(StarLayer.ShaderTextureName).y * -0.5f + 0.5f );*/

        Offset += new Vector2(cam.transform.position.x / (originalStarFonSize.x * (StarLayer.Speed / 10) / StarLayer.originalTextureSize.x),
                              cam.transform.position.y / (originalStarFonSize .y* (StarLayer.Speed / 10) / StarLayer.originalTextureSize.y));

        StarsMaterial.SetTextureOffset(StarLayer.ShaderTextureName, Offset);
        StarsMaterial.SetTextureOffset(StarLayer.ShaderTextureName + "_Mask", Offset);
    }

    void StarFonUpdateInTheEnd(StarLayer StarLayer)
    {
        StarsMaterial.SetTextureOffset(StarLayer.ShaderTextureName, new Vector2(0, 0));
        StarsMaterial.SetTextureOffset(StarLayer.ShaderTextureName + "_Mask", new Vector2(0, 0));
    }

    void Update()
    {
        StarFonUpdate(Background);
        StarFonUpdate(StarDust);
        StarFonUpdate(SmallStars);
        StarFonUpdate(MediumStars);
        StarFonUpdate(BigStars);
        gameObject.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, gameObject.transform.position.z);
    }

    void OnApplicationQuit()
    {
        StarFonUpdateInTheEnd(Background);
        StarFonUpdateInTheEnd(StarDust);
        StarFonUpdateInTheEnd(SmallStars);
        StarFonUpdateInTheEnd(MediumStars);
        StarFonUpdateInTheEnd(BigStars);
    }

}

