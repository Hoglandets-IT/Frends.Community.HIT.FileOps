KEY=$1
VER=$2

rm -f ./Frends.Community.HIT.FileOps.*.nupkg && \
dotnet build --configuration Release /p:Version=$VER && \
dotnet pack --configuration Release /p:Version=$VER --no-build --include-source --output . && \
dotnet nuget push Frends.Community.HIT.FileOps.$VER.nupkg --source https://proget.intern.hoglandet.se/nuget/Frends/ --api-key $KEY