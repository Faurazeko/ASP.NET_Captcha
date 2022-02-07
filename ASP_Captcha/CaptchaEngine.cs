using System.Drawing;
using System.Text;

//i dont give a shit that all this staff works only on windows xd lmao
#pragma warning disable CA1416

namespace ASP_Captcha
{
    public class CaptchaEngine
    {
        const string FontFamilyName = "Hack";
        const int FontSize = 100;
        const FontStyle FontStyle1337 = FontStyle.Bold;
        static readonly Font Font = new Font(FontFamilyName, FontSize, FontStyle1337);
        static readonly Brush TextBrush = new SolidBrush(Color.Black);
        const int TextAngleRotation = 10;

        static readonly Color BackgroundColor = Color.White;

        static readonly Random Random = new Random();

        const int CaptchaLength = 5;


        static public Image GenerateCaptcha(out string captchaContent) 
        {
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            captchaContent = GenerateCaptchaText(CaptchaLength);
            SizeF textSize = drawing.MeasureString(captchaContent, Font);
            int symbolWidth = (int)(textSize.Width / CaptchaLength);

            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)Math.Ceiling(textSize.Width), (int)Math.Ceiling(textSize.Height));
            drawing = Graphics.FromImage(img);

            drawing.Clear(BackgroundColor);
            //AddCoolBackground(drawing, textSize);

            //drawing.DrawString(text, Font, TextBrush, 0, 0);
            DrawMessedText(drawing, captchaContent, symbolWidth, true);

            AddLines(drawing, textSize);
            AddNoise(drawing, textSize, img/*, true*/);

            drawing.Save();
            //img.Save(path);
            drawing.Dispose();
            //img.Dispose();
            return img;
        }

        static private void AddNoise(Graphics drawing, SizeF textSize, Image img, bool randomizeColor = false)
        {
            var color = Color.White;
            var bmp = img as Bitmap;
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if(Random.NextDouble() < 0.5)
                    {
                        bmp.SetPixel(i, j, color);

                        if (randomizeColor)
                            color = GenerateRandomColor();
                    }  
                }
            }
        }

        static private void DrawMessedText(Graphics drawing, string text, int symbolWidth, bool randomizeColor = false)
        {
            int widthOffset = 0;
            Brush brush = TextBrush;
            for (int i = 0; i < text.Length; i++)
            {
                if (randomizeColor)
                    brush = new SolidBrush(GenerateRandomColor());

                var angle = Random.Next(-TextAngleRotation, TextAngleRotation + 1);
                drawing.RotateTransform(angle);
                drawing.DrawString(text[i].ToString(), Font, brush, widthOffset, 0);
                widthOffset += symbolWidth;
                drawing.RotateTransform(-angle);
            }
        }

        static private void AddCoolBackground(Graphics drawing, SizeF textSize)
        {
            int magicNumber = ((int)Math.Ceiling((textSize.Width * textSize.Height) / 1024)) / 2;
            int lineCount = magicNumber;
            for (int i = 0; i < lineCount; i++)
            {
                int[] points = GenerateRandomXYPoints(textSize, 4);

                var color = GenerateRandomColor();
                var lineThicc = Random.Next(magicNumber/2, magicNumber);
                Pen pen = new Pen(color, lineThicc);

                drawing.DrawLine(pen, points[0], points[1], points[2], points[3]);
            }
        }

        static private void AddLines(Graphics drawing, SizeF textSize)
        {
            int magicNumber = (int)Math.Ceiling((textSize.Width * textSize.Height) / 1024);
            int lineCount = magicNumber;
            for (int i = 0; i < lineCount; i++)
            {
                int[] points = GenerateRandomXYPoints(textSize, 4);

                var color = GenerateRandomColor();
                var lineThicc = Random.Next(1, 5);
                Pen pen = new Pen(color, lineThicc);

                drawing.DrawLine(pen, points[0], points[1], points[2], points[3]);

            }
        }

        static private string GenerateCaptchaText(int symbolsCount)
        {
            if (symbolsCount <= 0)
                throw new ArgumentException("The number of characters must be greater than 0");

            var captchaText = new StringBuilder();

            for (int i = 0; i < symbolsCount; i++)
                //captchaText.Append((char)Random.Next(33, 126)); // special symbols, numbers, capital and lower letters
                captchaText.Append((char)Random.Next(65, 90)); // only capital letters

            return captchaText.ToString();
        }

        /// <summary>
        /// every first point is x and every second is y you know
        /// </summary>
        /// <param name="textSize"> size of text which is SizeF and can be measured by graphics.MeasureString()</param>
        /// <param name="count">must be a multiple of 2 because we creating x and y points you know</param>
        /// <returns></returns>
        static private int[] GenerateRandomXYPoints(SizeF textSize, int count)
        {
            int[] points = new int[count];

            int height = (int)Math.Ceiling(textSize.Height) + 1;
            int width = (int)Math.Ceiling(textSize.Width) + 1;

            for (int i = 1; i <= count; i++)
            {
                if(i % 2 == 0)
                    points[i-1] = Random.Next(0, height);
                else
                    points[i-1] = Random.Next(0, width);
            }

            return points;
        }

        static private Color GenerateRandomColor() => Color.FromArgb(Random.Next(0, 256), Random.Next(0, 256), Random.Next(0, 256));
    }
}
