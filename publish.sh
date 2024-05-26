set -e

version=$1
key=$2

if [ -z "$2" ]; then
  echo "error params"
  exit 1
fi

sed -i -r "s/<Version>.*<\/Version>/<Version>$version<\/Version>/" "src\DryIocPatcher.csproj"
sed -i -r "s/<AssemblyVersion>.*<\/AssemblyVersion>/<AssemblyVersion>$version<\/AssemblyVersion>/" "src\DryIocPatcher.csproj"

dotnet build -c Release

dotnet nuget push "src\bin\Release\DryIocPatcher.$version.nupkg" --api-key $key --source https://api.nuget.org/v3/index.json

git add -A
git commit -m "chore v$version"
git tag "v$version"
