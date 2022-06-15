using System.Text;
using Renci.SshNet;


#pragma warning disable 1591
#pragma warning disable 8602

namespace Frends.Community.HIT.FileOps
{
    public class SFTP
    {
        /// <summary>
        /// List files in directory on SFTP server
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        /// <param name="input">Path information</param>
        /// <param name="server">Server settings</param>
        public static ListResult ListFiles(SFTPListInput input, SFTPServerSettings server)
        {
            using (var client = new SftpClient(server.GetConnectionInfo())) {
                client.Connect();
                var result = client.ListDirectory(input.Path);
                client.Disconnect();
                return new ListResult(true, new List<string>(result.Select(x => x.Name).ToList()));
            }
        }

        /// <summary>
        /// Read file from SFTP Server
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        /// <param name="input">Path and encoding information</param>
        /// <param name="server">Server settings</param>
        public static ReadResult ReadFile(SFTPReadInput input, SFTPServerSettings server)
        {
            Encoding encType = TranslateEncoding.GetEncoding(input.Encoding);
            
            using (var client = new SftpClient(server.GetConnectionInfo()))
            {
                client.Connect();
                var file = client.ReadAllText(input.Path, encType);
                client.Disconnect();

                return new ReadResult(true, file.ToString(), encType);
            }
        }

        /// <summary>
        /// Write file to SFTP Server
        /// Documentation: https://github.com/Hoglandets-IT/Frends.Community.HIT.FileOps
        /// </summary>
        /// <param name="input">Path and encoding information</param>
        /// <param name="server">Server settings</param>
        public static WriteResult WriteFile(SFTPWriteInput input, SFTPServerSettings server)
         {
            Encoding encType = TranslateEncoding.GetEncoding(input.Encoding);

            using (var client = new SftpClient(server.GetConnectionInfo()))
            {
                client.Connect();
                client.WriteAllText(input.Path, input.Content, encType);
                client.Disconnect();

                return new WriteResult(true, encType);
            }
        }
    }
}