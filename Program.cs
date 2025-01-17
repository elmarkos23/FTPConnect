﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace FTPConnect
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
			
				string carpetaArchivoLocal = @"D:\";

				string PathFTP = "ftp.grupodifare.com";
				string NombreArchvio = "yanki.mp3";
				string Usuario = "confiamed";
				string Clave = "C0nf1@m3d*";




				// Setup session options
				SessionOptions sessionOptions = new SessionOptions
				{
					Protocol = Protocol.Sftp,
					FtpSecure = FtpSecure.Explicit,
					HostName = PathFTP,
					UserName =Usuario,
					Password = Clave,
					PortNumber=22
//					SshHostKeyFingerprint = "ssh-rsa 2048 xxxxxxxxxxx...="
				};

				using (Session session = new Session())
				{
					// Connect
					session.Open(sessionOptions);

					// Upload files
					session.PutFiles(carpetaArchivoLocal, NombreArchvio).Check();
				}



				// Get the object used to communicate with the server.
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://"+PathFTP+"/"+ NombreArchvio + "");
				request.Method = WebRequestMethods.Ftp.UploadFile;

				// This example assumes the FTP site uses anonymous logon.
				request.Credentials = new NetworkCredential(Usuario, Clave);

				// Copy the contents of the file to the request stream.
				StreamReader sourceStream = new StreamReader(carpetaArchivoLocal);
				byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
				sourceStream.Close();
				request.ContentLength = fileContents.Length;
				request.KeepAlive = false;
				request.UseBinary = true;
				request.Proxy = null;
				request.UsePassive = false;
				request.ClientCertificates = new X509CertificateCollection();
				

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
