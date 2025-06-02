using UnityEngine;

namespace IbrahKit
{
    public static class Image_Utitlities
    {
        public static byte[] ImageToByteArray(Sprite sprite)
        {
            if(sprite == null)
            {
                Debug.LogWarning("Sprite is null");
                return new byte[0];
            }

            return ImageToByteArray(sprite.texture);
        }

        public static byte[] ImageToByteArray(Texture2D texture)
        {
            if (texture == null)
            {
                Debug.LogWarning("Texture is null");
                return new byte[0];
            }

            return texture.EncodeToPNG();
        }

        public static Sprite ByteArrayToSprite(byte[] bytes, Vector2Int size)
        {
            if(bytes == null)
            {
                Debug.LogWarning("Bytes array is null");
                return Sprite.Create(new(0,0),new(0,0,0,0),new(0,0));
            }

            if (bytes.Length == 0)
            {
                Debug.LogWarning("Bytes array is empty");
                return Sprite.Create(new(0, 0), new(0, 0, 0, 0), new(0, 0));
            }

            Texture2D texture = ByteArrayToTexture(bytes, size);

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            return sprite;
        }

        public static Texture2D ByteArrayToTexture(byte[] bytes, Vector2Int size)
        {
            if (bytes == null)
            {
                Debug.LogWarning("Bytes array is null");
                return new(0,0);
            }

            if (bytes.Length == 0)
            {
                Debug.LogWarning("Bytes array is empty");
                return new(0, 0);
            }

            Texture2D texture = new(size.x, size.y, TextureFormat.RGBA32, false);
            texture.LoadRawTextureData(bytes);
            texture.Apply();

            return texture;
        }

        public static Sprite GrayscaleSprite(Sprite _sprite)
        {
            if(_sprite == null)
            {
                Debug.LogWarning("Sprite is empty");
                return Sprite.Create(new(0, 0), new(0, 0, 0, 0), new(0, 0));
            }

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

}