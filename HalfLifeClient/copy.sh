#!/bin/sh
rm /run/media/mryabloko/f3320f1a-5ddf-4209-8cd2-87ed67db9cbd/SteamLibrary/steamapps/common/Half-Life/valve/cl_dlls/client_amd64.so

echo "Deleted original"

dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishAot=true

echo "Copying"

cp /home/mryabloko/RiderProjects/HalfLifeServer/HalfLifeClient/bin/Release/net9.0/linux-x64/publish/HalfLifeClient.so /run/media/mryabloko/f3320f1a-5ddf-4209-8cd2-87ed67db9cbd/SteamLibrary/steamapps/common/Half-Life/valve/cl_dlls/client_amd64.so

echo "Copied"
