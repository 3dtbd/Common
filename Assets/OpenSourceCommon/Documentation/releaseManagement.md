# Releasing

We use [Jeff Campbell's Unity Package Tools](https://github.com/jeffcampbellmakesgames/unity-package-tools) to help with building releases. Install these tools from the release package on GitHub (we don't install via package manager as we don't want them to be installed as a dependency of our projects).

[Full usage instructions](https://github.com/jeffcampbellmakesgames/unity-package-tools/blob/master/usage.md) are available, the below is a summary for convenience. It describes the release process for all 3Dtbd assets.

  1. Update the Package Version Number in the `PackageManifestConfig`
  2. Review all other settings in the `PackageManifestConfig`
  3. Click Export Package in the inspector for `PackageManifestConfig`
  4. In bash, cd to the export directory and run the following commands (be sure to replace any "[bracketed value]"):
  
  ```bash
  PACKAGE="[PACKAGE NAME]"
  VERSION="[x.y.z]"

  git init
  git remote add origin git@github.com:3dtbd/$PACKAGE.git
  git checkout --orphan release/v$VERSION
  git add .
  git commit -m "Release v$VERSION"
  git push origin release/v$VERSION
  ```

