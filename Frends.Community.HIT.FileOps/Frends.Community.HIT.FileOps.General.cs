using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using Renci.SshNet;


#pragma warning disable 1591
#pragma warning disable 8602

namespace Frends.Community.HIT.FileOps
{
    public class General
    {
        private static List<string> GetMatchingFiles(string SMBUrl, string path, string pattern)
        {
            var filesInFolder = SMB.ListFiles(new SMBListInput(SMBUrl));
            
            List<string> regexMatches = new List<string>(
                filesInFolder.ResultData.Where(
                    x => Regex.IsMatch(x, pattern)
                )
            );

            return regexMatches;
        }
        private static List<string> GetMatchingFiles(SFTPServerSettings server, string path, string pattern)
        {
            var filesInFolder = SFTP.ListFiles(new SFTPListInput(path), server);

            List<string> regexMatches = new List<string>(
                filesInFolder.ResultData.Where(
                    x => Regex.IsMatch(x, pattern)
                )
            );

            return regexMatches;
        }

        private static string GenerateDestinationFilename(string sourceFile, string destPattern)
        {
            if (destPattern == @"{source_filename}") {
                return sourceFile;
            }

            if (destPattern.Contains(@"{date}")) {
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                destPattern = destPattern.Replace(@"{date}", date);
            }

            if (destPattern.Contains(@"{time}")) {
                var datetime = DateTime.Now.ToString("hh-mm-ss");
                destPattern = destPattern.Replace(@"{time}", datetime);
            }

            if (destPattern.Contains(@"{guid}")) {
                var guid = Guid.NewGuid().ToString();
                destPattern = destPattern.Replace(@"{guid}", guid);
            }

            // if (destPattern.Contains(@"{sequential}")) {
            // }
            return destPattern;
        }

        private static Dictionary<string, ServerConfiguration> LoadServerConfiguration(string configJson)
        {
            var config = JsonConvert.DeserializeObject<Dictionary<string, ServerConfiguration>>(configJson);
            return config;
        }

        private static List<MoveObject> LoadMoveObjects(string moveObjectsJson)
        {
            var moveObjects = JsonConvert.DeserializeObject<List<MoveObject>>(moveObjectsJson);
            return moveObjects;
        }

        public static CopyResult CopyFiles(string moveObjectJSON, string serverConfigJson)
        {
            var moveObjects = JsonConvert.DeserializeObject<List<MoveObject>>(moveObjectJSON);
            var serverConfigurations = JsonConvert.DeserializeObject<Dictionary<string, ServerConfiguration>>(serverConfigJson);

            var copiedFiles = new Dictionary<string, string>();
            int countFiles = 0;

            foreach (var moveObject in moveObjects)
            {
                var sourceServer = serverConfigurations[moveObject.SourceServer];
                var destinationServer = serverConfigurations[moveObject.DestinationServer];

                string logSrcUrl = $"{sourceServer.ServerType}://{sourceServer.Username}@{sourceServer.Server}/{moveObject.SourcePath}/";
                string logDstUrl = $"{destinationServer.ServerType}://{destinationServer.Username}@{destinationServer.Server}/{moveObject.DestinationPath}/";

                var sourceContent = new Dictionary<string, string>();

                if (sourceServer.ServerType == ServerTypes.SMB) {
                    string SourceSMBURL = SMB.GetSMBConnectionString(
                        sourceServer.Server,
                        sourceServer.Username,
                        sourceServer.Password,
                        sourceServer.Domain,
                        moveObject.SourcePath
                    );
                    var files = new List<string>();
                    try {
                        files = GetMatchingFiles(
                            SourceSMBURL, 
                            moveObject.SourcePath, 
                            moveObject.SourcePattern
                        );
                    }
                    catch (Exception ex) {
                        string errorSMBURL = SMB.GetSMBConnectionString(
                            sourceServer.Server,
                            sourceServer.Username,
                            "[redacted-password]",
                            sourceServer.Domain,
                            moveObject.SourcePath
                        );
                        
                        throw new Exception(
                            "ERROR: Could not connect to server " + errorSMBURL,
                            ex
                        );
                    }

                    foreach (var file in files) {
                        var fileContent = SMB.ReadFile(
                            new SMBReadInput(
                                SourceSMBURL + file
                            )
                        );

                        sourceContent.Add(file, fileContent.ResultData);
                    }
                }
                else {
                    var SFTPServer = new SFTPServerSettings(
                        sourceServer.Server,
                        sourceServer.Username,
                        sourceServer.Password,
                        sourceServer.PrivateKey,
                        sourceServer.Fingerprint
                    );

                    var files = new List<string>();

                    files = GetMatchingFiles(
                    SFTPServer,
                    moveObject.SourcePath,
                    moveObject.SourcePattern
                    );

                    foreach (var file in files) {
                        var path = moveObject.SourcePath;
                        if (path.EndsWith("/") == false) {
                            path += "/";
                        }
                        path += file;

                        var fileContent = SFTP.ReadFile(
                            new SFTPReadInput(
                               path
                            ),
                            SFTPServer
                        );

                        sourceContent.Add(file, fileContent.ResultData);
                    }
                }

                if (destinationServer.ServerType == ServerTypes.SMB) {
                    var destinationSMBURL = SMB.GetSMBConnectionString(
                            destinationServer.Server,
                            destinationServer.Username,
                            destinationServer.Password,
                            destinationServer.Domain,
                            moveObject.DestinationPath
                    );

                    if (destinationSMBURL.EndsWith("/") == false) {
                        destinationSMBURL += "/";
                    }

                    foreach (KeyValuePair<string, string> entry in sourceContent) {
                        var destinationFilename = GenerateDestinationFilename(entry.Key, moveObject.DestinationFilename);
                                                
                        SMB.WriteFile(
                            new SMBWriteInput(
                                destinationSMBURL + destinationFilename,
                                entry.Value,
                                moveObject.Overwrite
                            )
                        );
                        copiedFiles.Add(logSrcUrl + entry.Key, logDstUrl + destinationFilename);
                        countFiles += 1;
                    }
                }
                else {
                    var destinationSFTPServer = new SFTPServerSettings(
                        destinationServer.Server,
                        destinationServer.Username,
                        destinationServer.Password,
                        destinationServer.PrivateKey,
                        destinationServer.Fingerprint
                    );

                    foreach (KeyValuePair<string, string> entry in sourceContent) {
                        var destinationFilename = GenerateDestinationFilename(entry.Key, moveObject.DestinationFilename);
                        var destinationPath = moveObject.DestinationPath;
                        if (destinationPath.EndsWith("/") == false) {
                            destinationPath += "/";
                        }
                        SFTP.WriteFile(
                            new SFTPWriteInput(
                                destinationPath + destinationFilename,
                                entry.Value,
                                moveObject.Overwrite
                            ),
                            destinationSFTPServer
                        );
                        copiedFiles.Add(logSrcUrl + entry.Key, logDstUrl + destinationFilename);
                        countFiles += 1;
                    }
                }
            }
            return new CopyResult(true, countFiles, copiedFiles);
        }
    }
}