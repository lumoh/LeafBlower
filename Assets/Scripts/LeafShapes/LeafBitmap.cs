using System.Collections.Generic;
using UnityEngine;

public class LeafBitmap : LeafPile
{
    public Texture2D Bitmap;
    public bool Transparency;
    public bool UseColors;

    public override void SetColors()
    {
        if (!UseColors || Bitmap == null)
        {
            base.SetColors();
        }
        else
        {
            if (Bitmap != null)
            {
                Color[] pixels = Bitmap.GetPixels();
                List<Color> pixelList = new List<Color>();
                foreach(var pixel in pixels)
                {
                    if(Transparency && pixel.r == 0 && pixel.g == 0 && pixel.b == 0)
                    {
                        continue;
                    }

                    pixelList.Add(pixel);
                }

                for (int i = 0; i < _leaves.Count; i++)
                {
                    Leaf leaf = _leaves[i];
                    Color color = pixelList[i];

                    leaf.SetColor(color);
                }
            }
        }
    }

    public void CreateLeavesFromBitmap(bool transparency)
    {
        Transparency = transparency;

        _leaves = new List<Leaf>();
        if (Bitmap != null)
        {
            Color[] pixels = Bitmap.GetPixels();

            Debug.Log(pixels.Length);
            int x = 0;
            int y = 0;
            int width = Bitmap.width;
            int height = Bitmap.height;

            foreach (var pixel in pixels)
            {
                if (Transparency && pixel.r == 0 && pixel.g == 0 && pixel.b == 0)
                {
                    x++;
                    x = x % width;
                    if (x == 0)
                    {
                        y++;
                        y = y % height;
                    }
                }
                else
                {
                    Leaf newLeaf = spawnLeaf();
                    _leaves.Add(newLeaf);
                    NumLeaves++;

                    newLeaf.transform.localPosition = new Vector3(x * Leaf.WIDTH, Leaf.WIDTH / 2f, y * Leaf.WIDTH);

                    x++;
                    x = x % width;
                    if (x == 0)
                    {
                        y++;
                        y = y % height;
                    }
                }
            }
        }
    }
}
