# Frends.Community.HIT.FileOps
Frends integration platform task for moving files to/from SMB and SFTP shares/servers.

## Installation
Download the nupkg-package to the right and add into your Frends installation, or connect Frends to https://proget.hoglan.dev/nuget/Frends/ to receive update notifications.

## Server Environments
The configuration for the module is stored in the Environment Variables section of the Frends Panel Settings, but can also be read from a file on the agent host(upcoming).
The basic configuration structure is:
```jsonp
{
    "SMBSERV01": {
        // This can be either SMB, SFTP, FTP or HTTP
        "serverType": "SMB",

        // The server name or IP address
        "server": "someserver.internal.net",

        // The username to use for authentication (no domain part)
        "username": "automation-user",

        // The password to use to access the share
        "password": "automation-password",

        // The domain to use for the username
        "domain": "INTERNAL"
    },
    "SFTPSERV01": {
        "serverType": "SFTP",

        // The server name or IP address and port if not the default port (21)
        "server": "ftp.somehost.com",
        "username": "some-user",
        "password": "some-password",
        "fingerprint": "optional-server-fingerprint-for-verification",
        "private_key": "optional-private-key-for-authentication"
    },
    "FTPSERV01": {
        "serverType": "FTP",

        // The server name or IP address and port if not the default port (21)
        "server": "ftp.somehost.com",
        "username": "some-user",
        "password": "some-password"
    },
    "HTTPSERV01": {
        "serverType": "HTTP",
        "server": "https://somehost.com/",
        
        // BASIC username and password, if applicable
        "username": "some-user",
        "password": "some-password",

        // Certificate authentication, if applicable
        // Either via path on the agent host, or directly from a string of certificate data
        "certificate": "optional-certificate-data-for-authentication",
        "certificate_path": "optional-path-to-certificate-on-agent-host",
        "certificate_password": "optional-password-for-certificate",
    }
}

```

When configured in the Environment part of the Frends interface, create a new environment variable of the type "group".
You can then add each server as an item, with the contents set to the server configuration (example below)

```json
{
        "serverType": "SMB",
        "server": "someserver.internal.net",
        "username": "automation-user",
        "password": "automation-password",
        "domain": "INTERNAL"
}
```

## General Methods
### Load Configuration
Loads the server configuration from a file on the agent host, alternatively on SMB, SFTP, FTP or HTTP/s.

### Create Move Object
A move object is the configuration for a move operation. It's created by using the Create Move Object card, and can be used to add either a single or multiple move objects in parallel and feed them into the Move File/Move Files card.
The following options are available:
|Field Name|Description|Example Value|
|---|---|---|
|Object Guid|Unique identifier for the move object, can be used to name files and identify the object in troubleshooting.|64f7001c-c378-44c0-9435-3d63d1c6c6a5|
|Source Server|The source server to use for the move operation, either from env or from Load Configuration|#env.ServerConfiguration.SMBSERVER01 or #result[LoadConfig].SMBSERVER01|
|Source Path|The folder from which to get files, without the last /|/some/folder <br> /share/folder <br> /share$/folder|
|Source Pattern|The pattern to use for finding files, can apply to multiple files at once. Supports regex.|*.txt <br> someFile[a-zA-Z]{5}\.json|
|Destination Server|The destination server to use for the move operation, either from en vor Load Configuration|#env.ServerConfiguration.SFTPSERVER01 <br> #result[LoadConfig].SFTPSERVER01|
|Destination Path|The path on the destination host to move the files to, without the last /|/some/folder <br> /share/folder <br> /share$/folder|
|Destination Filename|The filename to use for the moved files. If not set, the original filename will be retained. You can also use one of the placeholders below to modify the filename|{originalFilename}|
|Overwrite|If a file with the same destination filename exists at the target location, overwrite it|

To use the placeholders, enter the placeholder name complete with {} in the Destination filename box, together with any other text you want to use.

|Placeholder|Description|Input File|Destination Filename|Result|
|---|---|---|---|---|
|{source_filename}|If the Destination Filename field only contains {source_filename}, the complete original filename with extension will be retained.|test.txt|{source_filename}|test.txt|
|{source_filename}|If the Destination Filename field contains other text apart from the placeholder, the original filename is retained without the extension.|test.txt|{source_filename}123.txt|test123.txt|
|{date}|The current date in the format YYYY-MM-DD|test.txt|file-copied-{date}.txt|file-copied-2024-01-01.txt|
|{time}|The current time in the format hh-mm-ss|test.txt|file-copied-{time}.txt|file-copied-12-08-55.txt|
|{guid}|The GUID of the move object|test.txt|test-{guid}.txt|test-64f7001c-c378-44c0-9435-3d63d1c6c6a5.txt|
|{sequential}|The sequential number (amount of times a file has been moved in this current move object with this guid) (Requires further configuration, see below)|test.txt|test-{sequential}.txt|test-13.txt|



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