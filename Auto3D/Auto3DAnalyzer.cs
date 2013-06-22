using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.ProcessPlugins.Auto3D
{
    internal class Auto3DAnalyzer
    {
        public static void MaxScale(ref double[] projection, double max)
        {
            var minValue = double.MaxValue;
            var maxValue = double.MinValue;

            for (var i = 0; i < projection.Length; i++)
            {
                if (projection[i] > 0)
                    projection[i] = projection[i] / max;

                if (projection[i] < minValue)
                    minValue = projection[i];

                if (projection[i] > maxValue)
                    maxValue = projection[i];
            }

            if (maxValue == 0)
                return;

            for (var i = 0; i < projection.Length; i++)
            {
                if (maxValue == 255)
                    projection[i] = 1;
                else
                    projection[i] = (projection[i] - minValue) / (maxValue - minValue);
            }
        }

        public static double CalulatePrjSim(double[] source, double[] compare)
        {
            var frequencies = new Dictionary<double, int>();

            for (var i = 0; i < source.Length; i++)
            {
                var difference = source[i] - compare[i];

                difference = Math.Round(difference, 2);
                difference = Math.Abs(difference);

                if (frequencies.ContainsKey(difference))
                    frequencies[difference] = frequencies[difference] + 1;
                else
                    frequencies.Add(difference, 1);
            }

            var deviation = frequencies.Sum(value => (value.Key * value.Value));

            deviation /= source.Length;
            deviation = (0.5 - deviation) * 2;

            return deviation;
        }

        public static double CheckFor3DFormat(System.Drawing.Imaging.BitmapData bmpData, int width, int height, bool bSideBySide)
        {
            var hp1 = new double[width];
            var vp1 = new double[height];

            var hp2 = new double[width];
            var vp2 = new double[height];

            double totalBrightness = 0;

            unsafe
            {
                byte* ptr = null;

                // iterate over all pixels of the image and calculate the brightness for each pixel
                // then store the values for the horizontal and vertical projection of the pixel brightness
                // values for each part of the image (SBS/TAB)

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        ptr = (byte*)bmpData.Scan0 + (bmpData.Stride * y) + x * 3;

                        byte r1 = *ptr++;
                        byte g1 = *ptr++;
                        byte b1 = *ptr++;

                        int brightness = (byte)(((0.2126 * r1) + (0.7152 * g1)) + (0.0722 * b1)); // default formula for brightness 

                        hp1[x] += brightness;
                        vp1[y] += brightness;

                        totalBrightness += brightness;

                        if (bSideBySide)
                            ptr = (byte*)bmpData.Scan0 + (bmpData.Stride * y) + (bmpData.Stride / 2) + x * 3;
                        else
                            ptr = (byte*)bmpData.Scan0 + (bmpData.Stride * (y + height)) + x * 3;

                        byte r2 = *ptr++;
                        byte g2 = *ptr++;
                        byte b2 = *ptr++;

                        brightness = (byte)(((0.2126 * r2) + (0.7152 * g2)) + (0.0722 * b2));

                        hp2[x] += brightness;
                        vp2[y] += brightness;

                        totalBrightness += brightness;
                    }
                }
            }

            totalBrightness /= (width * height); // get average brightness of all pixels

            if (totalBrightness < 10) // if image is to dark ignore it
                return -1;

            // scale value to maximum for better results

            MaxScale(ref hp1, width);
            MaxScale(ref vp1, height);

            MaxScale(ref hp2, width);
            MaxScale(ref vp2, height);

            // calculate horizontal and vertical projection of brightness values

            double hSim = CalulatePrjSim(hp1, hp2);
            double vSim = CalulatePrjSim(vp1, vp2);

            //System.Diagnostics.Debug.WriteLine("B:" + totalBrightness + " - H:" + hSim + " - V:" + vSim);

            double delta = Math.Abs(hSim - vSim);

            // if delta is small return max projection values

            if (delta < 0.25)
                return Math.Max(hSim, vSim);

            // if delta is too big, we return the average of both projection values

            return (hSim + vSim) / 2;
        }
    }
}
