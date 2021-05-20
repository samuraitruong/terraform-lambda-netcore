dotnet publish  src/DigitalSignatureApi/DigitalSignatureApi.csproj --output ./dist
zip function.zip ./dist/*.*
mv function.zip tf/module/s3/function.zip
