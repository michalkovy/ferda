//#define ZIP_MAIN

using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace Ferda.Modules.MetabaseLayer
{
	public static class GZip
	{
		private static int ReadAllBytesFromStream(Stream stream)
		{
			// Use this method is used to read all bytes from a stream.
			int bufferSize = 1024;
			byte[] buffer = new byte[bufferSize];
			int totalCount = 0;
			while (true)
			{
				int bytesRead = stream.Read(buffer, 0, bufferSize);
				if (bytesRead == 0)
				{
					break;
				}
				totalCount += bytesRead;
			}
			return totalCount;
		}

		public static void Compress(string inputFilePath, string outputFilePath)
		{
			FileStream infile;
			infile = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			byte[] buffer = new byte[infile.Length];
			if (infile.Read(buffer, 0, buffer.Length) != buffer.Length)
				throw new Exception("Unable to read data from filePath");
			infile.Close();
			MemoryStream memoryStream = new MemoryStream();
			GZipStream compressedzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
			compressedzipStream.Write(buffer, 0, buffer.Length);
			compressedzipStream.Close();
			memoryStream.Position = 0;
			FileStream outputFileStream = System.IO.File.Create(outputFilePath, (int)memoryStream.Length);
			memoryStream.WriteTo(outputFileStream);
			outputFileStream.Close();
			memoryStream.Close();
		}
		public static void Decompress(string inputFilePath, string outputFilePath)
		{
			//"E:\\Saves\\Projekt\\svn\\bin\\MetabaseLayer\\DB\\LISpMinerMetabaseEmpty.mdb.zip"
			FileStream inputFileStream = System.IO.File.Open(inputFilePath, FileMode.Open);
			GZipStream compressedGZipStreamForGetLength = new GZipStream(inputFileStream, CompressionMode.Decompress, true);
			byte[] decompressedBuffer = new byte[GZip.ReadAllBytesFromStream(compressedGZipStreamForGetLength)];
			compressedGZipStreamForGetLength.Close();
			inputFileStream.Position = 0;
			GZipStream compressedGZipStream = new GZipStream(inputFileStream, CompressionMode.Decompress, false);
			compressedGZipStream.Read(decompressedBuffer, 0, decompressedBuffer.Length);
			FileStream outputFileStream = System.IO.File.Create(outputFilePath, decompressedBuffer.Length);
			outputFileStream.Write(decompressedBuffer, 0, decompressedBuffer.Length);
			outputFileStream.Close();
			compressedGZipStream.Close();
		}

#if ZIP_MAIN
		public static void Main(string[] args)
		{
			GZip.Compress(
				Path.Combine(Metabase.BaseDir, Metabase.UncompressedFileName),
				Path.Combine(Metabase.BaseDir, Metabase.CompressedFileName));
			GZip.Decompress(
				Path.Combine(Metabase.BaseDir, Metabase.CompressedFileName),
				Path.Combine(Metabase.BaseDir, System.Guid.NewGuid().ToString() + Metabase.UncompressedFileName));
		}
#endif
	}
}