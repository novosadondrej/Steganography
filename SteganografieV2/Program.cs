using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;


namespace Steganografie
{
    class Program
    {
        public enum State
        {
            Hide,
            Zeros
        }

        static void Main(string[] args)
        {
            int red = 0, green = 0, blue = 0, charIndex = 0,
                  charValue = 0, zeros = 0;
            long pixelElementIndex = 0;

            if (args[1] == "--hide" && args[2].Length != 0 &&
                args[3].Contains(".png"))
            {
                State state = State.Hide;
                Bitmap picture = new Bitmap("obr.png");
                string text = string.Empty;

                for (int i = 0; i < picture.Height; i++)
                {
                    for (int j = 0; j < picture.Width; j++)
                    {
                        Color pixel = picture.GetPixel(j, i);

                        red = pixel.R - pixel.R % 2;
                        green = pixel.G - pixel.G % 2;
                        blue = pixel.B - pixel.B % 2;

                        for (int y = 0; y < 3; y++)
                        {
                            if (pixelElementIndex % 8 == 0)
                            {
                                if (state == State.Zeros && zeros == 8)
                                {
                                    if ((pixelElementIndex - 1) % 3 < 2)
                                    {
                                        picture.SetPixel(j, i, Color.FromArgb
                                            (red, green, blue));
                                    }
                                    picture.Save(args[3]);
                                }
                                if (charIndex >= text.Length)
                                {
                                    state = State.Zeros;
                                }
                                else
                                {
                                    charValue = text[charIndex++];
                                }
                            }
                            switch (pixelElementIndex % 3)
                            {
                                case 0:
                                    {
                                        if (state == State.Hide)
                                        {
                                            red += charValue % 2;
                                            charValue /= 2;
                                        }
                                    }
                                    break;
                                case 1:
                                    {
                                        if (state == State.Hide)
                                        {
                                            green += charValue % 2;
                                            charValue /= 2;
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        if (state == State.Hide)
                                        {
                                            blue += charValue % 2;
                                            charValue /= 2;
                                        }
                                        picture.SetPixel(j, i, Color.FromArgb
                                            (red, green, blue));
                                    }
                                    break;
                            }
                            pixelElementIndex++;

                            if (state == State.Zeros)
                            {
                                zeros++;
                            }
                        }
                    }
                }
            }

            else if (args[1] == "--show" && args[2].Contains(".png"))
            {
                Bitmap pictureEncrypted = new Bitmap(args[2]);
                Color lastPixel = pictureEncrypted.GetPixel(pictureEncrypted.Width - 1,
                    pictureEncrypted.Height - 1);
                Color pixel;
                int text = lastPixel.B;
                string vysledek = string.Empty;

                for (int i = 0; i < pictureEncrypted.Width; i++)
                {
                    for (int j = 0; j < pictureEncrypted.Height; j++)
                    {
                        pixel = pictureEncrypted.GetPixel(i, j);
                        if (i < 1 && j < text)
                        {
                            int value = pixel.B;
                            char c = Convert.ToChar(value);
                            string pismeno = Encoding.ASCII.GetString(new byte[] 
                            {Convert.ToByte(c)});
                            vysledek += pismeno;
                        }
                    }
                }
                Console.WriteLine(vysledek);
                Console.ReadKey();
            }
        }
    }
}
