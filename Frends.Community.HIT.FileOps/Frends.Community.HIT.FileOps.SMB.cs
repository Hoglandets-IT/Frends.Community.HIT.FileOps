using System.Text;
using Newtonsoft.Json;
using SharpCifs.Smb;

#pragma warning disable 1591
#pragma warning disable 8602

namespace Frends.Community.HIT.FileOps.SMB
{
    public class SMB
    {
        public static ReadResult ReadFile(SMBReadInput input)
        {
            var file = new SmbFile(input.Path);
            String resultContent = "";
            Boolean resultSuccess = false;

            if (file.Exists())
            {
                var readStream = file.GetInputStream();
                var memStream = new MemoryStream();
                Encoding encType = Encoding.GetEncoding(
                    Enum.GetName(
                        typeof(FileEncodings), input.OutputEncoding
                    ).Replace(
                        "_", "-"
                    )
                );

                ((Stream)readStream).CopyTo(memStream);
                readStream.Dispose();

                resultContent = encType.GetString(memStream.ToArray());
                resultSuccess = true;

            }
            
            return new ReadResult(resultSuccess, resultContent);
        }
    }

    public class SMBOld
    {
        public static WriteResult WriteFile(WriteInput input)
        {

            var smbString = "smb://";
            if (!string.IsNullOrEmpty(input.Domain))
            {
                smbString += input.Domain + ";";
            }
            smbString += input.Username + ":" + input.Password + "@" + input.Path;

            var file = new SmbFile(smbString);

            if (!file.Exists())
            {
                file.CreateNewFile();
            }
            else 
            {
                if (input.Overwrite)
                {
                    file.Delete();
                    file.CreateNewFile();
                }
                else {
                    throw new Exception("File already exists and overwrite is not enabled");
                }
            }

            var writeStream = file.GetOutputStream();

            writeStream.Write(Encoding.UTF8.GetBytes(input.Content));

            writeStream.Dispose();

            return new WriteResult(true);

        }

        public static CopyResult CopyFile(CopyInput input)
        {
            var readOpts = new ReadInput();
            readOpts.Path = input.SourcePath;
            readOpts.Domain = input.SourceDomain;
            readOpts.Username = input.SourceUsername;
            readOpts.Password = input.SourcePassword;

            var writeOpts = new WriteInput();
            writeOpts.Path = input.DestPath;
            writeOpts.Domain = input.DestinationDomain;
            writeOpts.Username = input.DestinationUsername;
            writeOpts.Password = input.DestinationPassword;
            writeOpts.Overwrite = input.Overwrite;

            var file = ReadFile(readOpts);
            writeOpts.Content = file.ResultData;

            var result = WriteFile(writeOpts);

            return new CopyResult(result.Success);
        }

        public static JsonMoveResult JsonMove(JsonMoveInput input)
        {
            List<CopyInput> files = JsonConvert.DeserializeObject<List<CopyInput>>(input.SettingJson);

            foreach (CopyInput copyOpts in files) {
                CopyFile(copyOpts);
            }

            return new JsonMoveResult(true);
        }
    }
}