using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace imgTI
{
	class imgTI
	{
		private static readonly Dictionary<string, Color> Colors = new Dictionary<string, Color>
		{
			{ "BLEU", Color.FromArgb(0, 0, 255) },
			{ "ROUGE", Color.FromArgb(255, 0, 0) },
			{ "NOIR", Color.FromArgb(0, 0, 0) },
			{ "MAGENTA", Color.FromArgb(255, 0, 255) },
			{ "VERT", Color.FromArgb(0, 255, 0) },
			{ "MARRON", Color.FromArgb(128, 0, 0) },
			{ "ORANGE", Color.FromArgb(255, 165, 0) },
			{ "BLEU MRN", Color.FromArgb(0, 128, 255) },
			{ "BLEU CLR", Color.FromArgb(0, 191, 255) },
			{ "JAUNE", Color.FromArgb(255, 255, 0) },
			{ "BLANC", Color.FromArgb(255, 255, 255) },
			{ "GRIS CLR", Color.FromArgb(211, 211, 211) },
			{ "GRIS MOY", Color.FromArgb(169, 169, 169) },
			{ "GRIS", Color.FromArgb(128, 128, 128) },
			{ "GRIS FON", Color.FromArgb(105, 105, 105) }
		};

		public static string GetClosestColorName(int r, int g, int b)
		{
			Color inputColor = Color.FromArgb(r, g, b);
			string closestColorName = null;
			double closestDistance = double.MaxValue;

			foreach (var colorEntry in Colors)
			{
				double distance = ColorDistance(inputColor, colorEntry.Value);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestColorName = colorEntry.Key;
				}
			}

			return closestColorName;
		}

		private static double ColorDistance(Color color1, Color color2)
		{
			// Calculate Euclidean distance between two colors
			return Math.Sqrt(Math.Pow(color1.R - color2.R, 2) +
							 Math.Pow(color1.G - color2.G, 2) +
							 Math.Pow(color1.B - color2.B, 2));
		}
		public static Bitmap ScaleBitmap(Bitmap original, int width, int height)
		{
			// Create a new bitmap with the specified size
			Bitmap scaledBitmap = new Bitmap(width, height);
			
			// Use a Graphics object to draw the original bitmap onto the new bitmap
			using (Graphics g = Graphics.FromImage(scaledBitmap))
			{
				// Set the interpolation mode for better quality
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

				// Draw the original bitmap onto the new bitmap
				g.DrawImage(original, 0, 0, width, height);
			}

			return scaledBitmap;
		}
		
		
		public static void Main(string[] args)
		{
			Console.WriteLine("4re5 group - imgTI a basic image to TI basic script");
			if(args.Length >= 1) {
				string input = args[0];
				if(!File.Exists(input)) {
					Console.WriteLine("ERROR: input file does not exists");
					return;
				}
				
				var myBitmap = new Bitmap(input); 
				int pixelWidth = 2;
				myBitmap = ScaleBitmap(myBitmap, 132/2, 100/2);
				int r, g, b, outY;
				
				int Xmax = 132*2;
				int Ymax = 100/2;
				
				
				string output = @"AxesNAff
EffDess
0→Xmin
"+Xmax.ToString()+@"→Xmax
1→Xgrad
0→Ymin
"+Ymax.ToString()+@"→Ymax
2→Ygrad
1→Xrés
0.25→X
1→PasTrace";

				for (int y = 0; y < myBitmap.Height; y++) {
					for (int x = 0; x < myBitmap.Width; x++)
					{                    
						Color pixelColor = myBitmap.GetPixel(x, y);

						r = pixelColor.R;
						g = pixelColor.G;
						b = pixelColor.B;
						outY = Ymax-y;
						output += "\r\nPt-Aff("+x+","+outY+","+pixelWidth.ToString()+","+GetClosestColorName(r, g, b)+")";
					}
				}
				
				File.WriteAllText("ouput_script.txt", output);
				
				Console.WriteLine("Finished >> ouput_script.txt");
				
			} else {
				Console.WriteLine("ERROR: usage imgTI.exe inputimage");
				return;
			}
			
			
		}
	}
}