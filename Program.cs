using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPConnect
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
			
				string carpetaArchivoLocal = @"D:\yanki.mp3";

				string PathFTP = "rutaftp";
				string NombreArchvio = "file.jpg";


				// Get the object used to communicate with the server.
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://"+PathFTP+"/"+ NombreArchvio + "");
				request.Method = WebRequestMethods.Ftp.UploadFile;

				// This example assumes the FTP site uses anonymous logon.
				request.Credentials = new NetworkCredential("mayala", "mayala2019");

				// Copy the contents of the file to the request stream.
				StreamReader sourceStream = new StreamReader(carpetaArchivoLocal);
				byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
				sourceStream.Close();
				request.ContentLength = fileContents.Length;

				Stream requestStream = request.GetRequestStream();
				requestStream.Write(fileContents, 0, fileContents.Length);
				requestStream.Close();

				FtpWebResponse response = (FtpWebResponse)request.GetResponse();

				Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

				response.Close();

			}
			catch (Exception exception)
			{
				throw new Exception(exception.Message);
			
			}
		}
		
	}
}
