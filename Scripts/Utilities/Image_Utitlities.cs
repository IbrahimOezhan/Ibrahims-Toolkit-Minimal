using TemplateTools;
using UnityEngine;

public class Image_Utitlities
{
    public static byte[] ImageToByteArray(Sprite sprite)
    {
        return sprite.texture.EncodeToPNG();
    }

    public static byte[] ImageToByteArray(Texture2D texture)
    {
        return texture.EncodeToPNG();
    }

    public Sprite ByteArrayToSprite(byte[] bytes, Vector2Int size)
    {
        Texture2D texture = new(size.x, size.y, TextureFormat.RGBA32, false);
        texture.LoadRawTextureData(bytes);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    public Texture ByteArrayToTexture(byte[] bytes, Vector2Int size)
    {
        Texture2D texture = new(size.x, size.y, TextureFormat.RGBA32, false);
        texture.LoadRawTextureData(bytes);
        texture.Apply();

        return texture;
    }

    public static Sprite GrayscaleSprite(Sprite _sprite)
    {
        Texture2D texture2D = _sprite.texture;

        Color[] pixels = texture2D.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            float lumincance = Color_Utilities.ColorLuminance(pixels[i]);

            float range = Mathf.Lerp(0, 1, lumincance);

            pixels[i] = new(range, range, range, pixels[i].a);
        }

        Texture2D tex = new Texture2D(texture2D.width, texture2D.height, texture2D.format, false);

        tex.SetPixels(pixels);
        tex.Apply();

        Rect rect = new(0, 0, tex.width, tex.height);
        return Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
    }
}
