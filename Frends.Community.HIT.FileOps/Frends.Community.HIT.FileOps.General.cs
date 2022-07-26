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
        public static MoveObject CreateMoveObject(MoveObjectInput moveObject)
        {
            return new MoveObject(
                objectGuid: moveObject.ObjectGuid,
                sourceServerEnvironment: moveObject.SourceServerEnvironment,
                sourcePath: moveObject.SourcePath,
                sourcePattern: moveObject.SourcePattern,
                destinationServerEnvironment: moveObject.DestinationServerEnvironment,
                destinationPath: moveObject.DestinationPath,
                destinationFilename: moveObject.DestinationFilename,
                overwrite: moveObject.Overwrite
            );
        }

        public static MoveObject[] LoadJsonMoveObjects(string json)
        {
            return JsonConvert.DeserializeObject<MoveObject[]>(json);
        }

        public static CopyResult CopyFiles(dynamic[] ObjectsToMove)
        {
            var actualObjects = new List<MoveObject>();
            foreach (var item in ObjectsToMove) {
                if (item.GetType() == typeof(MoveObject)) {
                    actualObjects.Add(item);
                }
            }

            return HelperFunctions.DoFileCopy(actualObjects.ToArray());
        }

        public static CopyResult CopyFile(MoveObject ObjectToMove)
        {
            return CopyFiles(new dynamic[] { ObjectToMove });
        }
    }
}