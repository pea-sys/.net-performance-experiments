using System;
using System.Linq;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace ProcessBMP
{
	class Program
	{
		private const int tryCount = 10;
		static void Main(string[] args)
		{
			var p = new Program();

			const string bmpFile = @"..\..\..\Misc\Trees.bmp";

			File.Copy(bmpFile, "mmfFile.bmp", true);
            File.Copy(bmpFile, "bmpFile.bmp", true);

            var st1 = new Stopwatch();
            var st2 = new Stopwatch();
			var mmfRecord = new long[tryCount];
			var plainRecord = new long[tryCount];
            GC.Collect();

            for (int x = 0; x < tryCount; x++)
            {
                st1.Restart();
                p.WhiteOutRows("mmfFile.bmp");
                st1.Stop();
				
				mmfRecord[x] = st1.ElapsedMilliseconds;

                
                st2.Restart();
                var image = (Bitmap)Image.FromFile("bmpFile.bmp");
                p.WhiteOutRows(image);
                st2.Stop();
				plainRecord[x] = st2.ElapsedMilliseconds;
				image.Dispose();
                Console.WriteLine($"MMF method    {x.ToString()} : {st1.ElapsedMilliseconds.ToString()}");
                Console.WriteLine($"Bitmap method {x.ToString()} : {st2.ElapsedMilliseconds.ToString()}");
            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"MMF method    [mean] {mmfRecord.Average().ToString()} msec [max] {mmfRecord.Max().ToString()} msec [min] {mmfRecord.Min().ToString()} msec");
            Console.WriteLine($"Bitmap method [mean] {plainRecord.Average().ToString()} msec [max] {plainRecord.Max().ToString()} mesec [min] {plainRecord.Min().ToString()} msec");
            Console.ReadKey();
		}

		public void WhiteOutRows(System.Drawing.Bitmap image)
		{
			int rowStart = image.Size.Height - 200;
			for (var i = 0; i < 200; i += 2)
			{
				for (int x = 0; x < image.Size.Width; x++)
				{
					image.SetPixel(x, rowStart + i, Color.White);
				}
			}
			image.Save("bmpFile2.bmp", ImageFormat.Bmp);
		}

		public void WhiteOutRows(string bmpFilename)
		{
			var headers = ReadHeaders(bmpFilename);
			using (var mmf = MemoryMappedFile.CreateFromFile(bmpFilename, FileMode.Open))
			{
                int rowSize = headers.Item2.RowSize;    // number of byes in a row
                var whiteRow = (from b in Enumerable.Range(0, rowSize) select (byte)255).ToArray();
                for (var row = 0; row < 200; row += 2)
                {
                    using (var view = mmf.CreateViewAccessor(headers.Item1.DataOffset + rowSize * row, rowSize, MemoryMappedFileAccess.Write))
                    {
                        view.WriteArray(0, whiteRow, 0, whiteRow.Length);
                    }
                }
            }
		}

		public Tuple<BmpHeader, DibHeader> ReadHeaders(string filename)
		{
			var bmpHeader = new BmpHeader();
			var dibHeader = new DibHeader();
			using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				using (var br = new BinaryReader(fs))
				{
					bmpHeader.MagicNumber = br.ReadInt16();
					bmpHeader.Filesize = br.ReadInt32();
					bmpHeader.Reserved1 = br.ReadInt16();
					bmpHeader.Reserved2 = br.ReadInt16();
					bmpHeader.DataOffset = br.ReadInt32();

					dibHeader.HeaderSize = br.ReadInt32();
					if (dibHeader.HeaderSize != 40)
					{
						throw new ApplicationException("Only Windows V3 format supported.");
					}
					dibHeader.Width = br.ReadInt32();
					dibHeader.Height = br.ReadInt32();
					dibHeader.ColorPlanes = br.ReadInt16();
					dibHeader.Bpp = br.ReadInt16();
					dibHeader.CompressionMethod = br.ReadInt32();
					dibHeader.ImageDataSize = br.ReadInt32();
					dibHeader.HorizontalResolution = br.ReadInt32();
					dibHeader.VerticalResolution = br.ReadInt32();
					dibHeader.NumberOfColors = br.ReadInt32();
					dibHeader.NumberImportantColors = br.ReadInt32();
				}
			}

			return Tuple.Create(bmpHeader, dibHeader);
		}
	}

	public class BmpHeader
	{
		public short MagicNumber { get; set; }
		public int Filesize { get; set; }
		public short Reserved1 { get; set; }
		public short Reserved2 { get; set; }
		public int DataOffset { get; set; }
	}

	public class DibHeader
	{
		public int HeaderSize { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public short ColorPlanes { get; set; }
		public short Bpp { get; set; }
		public int CompressionMethod { get; set; }
		public int ImageDataSize { get; set; }
		public int HorizontalResolution { get; set; }
		public int VerticalResolution { get; set; }
		public int NumberOfColors { get; set; }
		public int NumberImportantColors { get; set; }
		public int RowSize
		{
			get
			{
				return 4 * ((Bpp * Width) / 32);
			}
		}
	}
}
