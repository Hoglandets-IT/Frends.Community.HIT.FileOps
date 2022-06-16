using System.Text;
using FluentFTP;

#pragma warning disable 1591
#pragma warning disable 8602
 
namespace Frends.Community.HIT.FileOps
{
    public class FTP
    {
        /// <summary>
        /// List files in directory on FTP server
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        /// <param name="input">Path information</param>
        /// <param name="server">Server settings</param>
        public static ListResult ListFiles(FTPListInput input, FTPServerSettings server)
        {
            var files = new List<string>();
            int port = 21;
            var split = server.Server.Split(':');
            var host = split[0];

            if (split.Length > 1)
            {
                port = int.Parse(split[1]);
            }


            using (FtpClient client = new FtpClient(host, port, server.Username, server.Password))
            {
                client.AutoConnect();

                var result = client.GetListing(input.Path, FtpListOption.Modify);
                
                foreach (FtpListItem item in result) {
                    if (item.Type == FtpFileSystemObjectType.File) {
                        files.Add(item.Name);
                    }
                }

            }
            return new ListResult(true, files);
        }

        /// <summary>
        /// Read file from FTP Server
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        /// <param name="input">Path and encoding information</param>
        /// <param name="server">Server settings</param>
        public static ReadResult ReadFile(FTPReadInput input, FTPServerSettings server)
        {
            int port = 21;
            var split = server.Server.Split(':');
            var host = split[0];

            if (split.Length > 1)
            {
                port = int.Parse(split[1]);
            }

            Encoding encType = HelperFunctions.TranslateEncoding.GetEncoding(input.Encoding);
            
            string resultContent = "";

            using (FtpClient client = new FtpClient(host, port, server.Username, server.Password))
            {
                client.AutoConnect();

                var memStream = new MemoryStream();
                var file = client.Download(memStream, input.Path);

                resultContent = encType.GetString(memStream.ToArray());
            }

            return new ReadResult(true, resultContent, encType);
        }

        /// <summary>
        /// Write file to FTP Server
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        /// <param name="input">Path and encoding information</param>
        /// <param name="server">Server settings</param>
        public static WriteResult WriteFile(FTPWriteInput input, FTPServerSettings server)
        {
            int port = 21;
            var split = server.Server.Split(':');
            var host = split[0];

            if (split.Length > 1)
            {
                port = int.Parse(split[1]);
            }

            Encoding encType = HelperFunctions.TranslateEncoding.GetEncoding(input.Encoding);
            
            using (FtpClient client = new FtpClient(host, port, server.Username, server.Password))
            {
                client.AutoConnect();

                FtpRemoteExists overwrite = FtpRemoteExists.Skip;
                if (input.Overwrite) {
                    overwrite = FtpRemoteExists.Overwrite;
                }

                var memStream = new MemoryStream(encType.GetBytes(input.Content));
                var file = client.Upload(memStream, input.Path, overwrite, false);
            }

            return new WriteResult(true, encType);
        }
    }
}