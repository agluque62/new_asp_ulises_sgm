using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace UtilitiesCD40
{
	public class ReplaceInFile
	{
		public static void Replace(string pathFile, string cadena1, string cadena2)
		{
			String line;

			try
			{
				//Pass the file path and file name to the StreamReader constructor
				StreamReader sr = new StreamReader(pathFile);

				//Read the file
				line = sr.ReadToEnd();

				// Replace old string for new string
				line = line.Replace(cadena1, cadena2);

				//close the file
				sr.Close();

				// StreamWriter constructor
				StreamWriter sw = new StreamWriter(pathFile, false);

				// Write the file with the replaced text
				sw.Write(line);

				// Close the file
				sw.Close();
			}
			catch (Exception /*e*/)
			{
			}
			finally
			{
			}
		}
	}
}
