using System.IO;
using Microsoft.Extensions.FileProviders;

namespace MockResponse.Core.Logging
{
	public class Logger : ILogger
    {
		private IFileProvider _fileProvider;

		public Logger(IFileProvider fileProvider){
			_fileProvider = fileProvider;	
		}

        public void Trace(string message)
        {
			File.WriteAllLines(_fileProvider.GetFileInfo("App_Data/log.log").PhysicalPath, new[] { message });
        }
    }
}
