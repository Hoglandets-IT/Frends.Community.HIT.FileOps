# Frends.Community.HIT.FileOps
Frends integration platform task for moving files to/from SMB and SFTP shares/servers.

## Installation
Download the nupkg-package to the right and add into your Frends installation

## Frends.Community.HIT.FileOps.General.CopyFiles
### moveObjectJSON
```json
[
    {
        "objectID": 1,
        "sourceServer": "SERVER01",
        "sourcePath": "/some/path",
        "sourcePattern": "file[0-9]*\\.txt",
        "destinationServer": "SERVER02",
        "destinationPath": "/home/",
        "destinationFilename": "{date}-{time}-{guid}.txt",
        "overwrite": true
    }
]
```
|Attribute|Type|Description|
|---|---|---|
|objectID|INT32|The identifier of the moveObject (will be used for the {sequential}-tag in the future)|
|sourceServer|string|The key of the source server in serverConfigJSON|
|sourcePath|string|The path to the directory where the files should be retrieved|
|sourcePattern|string|The regex pattern for determining which files to copy|
|destinationServer|string|The key of the destination server in serverConfigJSON|
|destinationPath|string|The path to the directory where the files will be copied|
|destinationFilename|string|The filename to set for the destination. {date}, {time} and {guid} can be used to substitute parts of the name with values, {sequential} will be added in the near future|
|overwrite|bool|If the destination file should be overwritten in case it exists|

### serverConfigJSON
```json
{
    "SMBSERVER01": {
        "serverType": "SMB",
        "server": "SMBSERVER01",
        "username": "domainuser",
        "password": "password",
        "domain": "MYDOMAIN"
    },
    "SMBSERVER02": {
        "serverType": "SMB",
        "server": "SMBSERVER02",
        "username": "localuser",
        "password": "password",
        "domain": ""
    },
    "FTPSERVER01": {
        "serverType": "SFTP",
        "server": "ftp.company.com",
        "username": "myuser",
        "password": "mypassword",
        "fingerprint": "SHA256 .....",
        "private_key": ""
    },
    "FTPSERVER02": {
        "serverType": "SFTP",
        "server": "ftp.othercompany.com",
        "username": "theiruser",
        "password": "private-key-password",
        "private_key": "base64-encoded-private-key",
        "fingerprint": ""
    }
}
```
|Attribute|Type|Description|
|---|---|---|
|serverType|string|SFTP or SMB for now|
|server|string|The address to the server, abc.domain.com for SMB and ftp.domain.com or ftp.domain.com:22 for SFTP|
|username|string|The username for authentication|
|password|string|The password for the user, or when a keyfile is provided, the password for the keyfile|
|private_key|string|The private keyfile for the user|
|fingerprint|string|Verify the fingerprint of the server (not yet implemented)|

## Frends.Community.HIT.FileOps.SFTP.ListFiles

## Frends.Community.HIT.FileOps.SFTP.ReadFile

## Frends.Community.HIT.FileOps.SFTP.WriteFile

## Frends.Community.HIT.FileOps.SMB.ListFiles

## Frends.Community.HIT.FileOps.SMB.ReadFile

## Frends.Community.HIT.FileOps.SMB.WriteFile