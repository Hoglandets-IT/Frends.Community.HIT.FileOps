using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Renci.SshNet;

#pragma warning disable 1591

namespace Frends.Community.HIT.FileOps
{
    public class SFTPServerSettings
    {
        /// <summary>
        /// Server address : port
        /// </summary>
        [DefaultValue(@"sftp.example.com:22")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Server { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [DefaultValue(@"username")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Username { get; set; }

        /// <summary>
        /// Base64-encoded PrivateKey to use for authentication
        /// </summary>
        [PasswordPropertyText]
        public string PrivateKey { get; set; } = "";

        /// <summary>
        /// Password for the user or keyfile
        /// </summary>
        [PasswordPropertyText]
        public string Password { get; set; }

        /// <summary>
        /// Fingerprint to verify
        /// </summary>
        [DefaultValue(@"")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Fingerprint { get; set; }

        private static String SaveTemporaryKey(String key)
        {
            var decodedKey = Convert.FromBase64String(key);
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Encoding.UTF8.GetString(decodedKey));
            return tempFile;
        }

        public ConnectionInfo GetConnectionInfo()
        {
            PrivateKeyFile pkey;
            Int32 port = 22;
            string[] serverAndPort = Server.Split(':');
            
            string server = serverAndPort[0];
            if (serverAndPort.Length > 1) {
                port = Int32.Parse(serverAndPort[1]);
            }

            if (String.IsNullOrEmpty(PrivateKey) == false && String.IsNullOrWhiteSpace(PrivateKey) == false) {
                var tempKey = SaveTemporaryKey(PrivateKey);
                
                if (String.IsNullOrEmpty(PrivateKey) == false && String.IsNullOrWhiteSpace(PrivateKey) == false) {
                    pkey = new PrivateKeyFile(tempKey, Password);
                }
                else {
                    pkey = new PrivateKeyFile(tempKey);
                }

                return new ConnectionInfo(
                    host: server,
                    port: port,
                    username: Username,
                    authenticationMethods: new PrivateKeyAuthenticationMethod(Username, pkey)
                );
            }
            
            return new ConnectionInfo(
                host: server,
                port: port,
                username: Username,
                authenticationMethods: new PasswordAuthenticationMethod(Username, Password)
            );
        }

        public SFTPServerSettings(
            string server,
            string username,
            string password = "",
            string privateKey = "",
            string fingerprint = ""
        )
        {
            Server = server;
            Username = username;
            Password = password;
            PrivateKey = privateKey;
            Fingerprint = fingerprint;
        }
    }

    public class SFTPListInput
    {
        /// <summary>
        /// The path to the folder
        /// </summary>
        [DefaultValue(@"/")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        public SFTPListInput(
            string path
        )
        {
            Path = path;
        }
    }

    public class SFTPReadInput
    {
        /// <summary>
        /// The full path to the file
        /// </summary>
        [DefaultValue(@"/folderA/folderB/test.txt")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        /// <summary>
        /// The output encoding
        /// </summary>
        [DefaultValue(FileEncodings.UTF_8)]
        public FileEncodings Encoding { get; set; }

        public SFTPReadInput(
            string path,
            FileEncodings encoding = FileEncodings.UTF_8
        )
        {
            Path = path;
            Encoding = encoding;
        }

    }

    public class SFTPWriteInput
    {
        /// <summary>
        /// The full path to the file
        /// </summary>
        [DefaultValue(@"/folderA/folderB/test.txt")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Path { get; set; }

        /// <summary>
        /// Overwrite the file if exists
        /// </summary>
        [DefaultValue(false)]
        public bool Overwrite { get; set; }     
        
        /// <summary>
        /// The file contents to write
        /// </summary>
        [DefaultValue(@"")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Content { get; set; }

        /// <summary>
        /// The output encoding
        /// </summary>
        [DefaultValue(FileEncodings.UTF_8)]
        public FileEncodings Encoding { get; set; }

        public SFTPWriteInput(
            string path,
            string content,
            bool overwrite = false,
            FileEncodings encoding = FileEncodings.UTF_8
        )
        {
            Path = path;
            Content = content;
            Encoding = encoding;
            Overwrite = overwrite;
        }
            
    }
}