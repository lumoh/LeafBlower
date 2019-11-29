using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class LeafBitmap : LeafPile
{
    public Texture2D Bitmap;

    public override void SetColors()
    {
        _leaves = new List<Leaf>();
        if (Bitmap != null)
        {
            Color[] pixels = Bitmap.GetPixels();
            for (int i = 0; i < _leaves.Count; i++)
            {
                Leaf leaf = _leaves[i];
                Color color = pixels[i];

                leaf.SetColor(color);
            }
        }
    }

    public void CreateLeavesFromBitmap()
    {
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
