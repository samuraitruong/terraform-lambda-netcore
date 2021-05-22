set -e
rm -rf ./dist/*.*

dotnet publish  src/DigitalSignatureApi/DigitalSignatureApi.csproj -c Release --output ./dist 
cd dist
echo {}>appsettings.json
zip -r function.zip .
mv function.zip ../tf/modules/s3/function.zip

cd ../tf

terraform apply
