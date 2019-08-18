# Releasing

We use [Jeff Campbell's Unity Package Tools](https://github.com/jeffcampbellmakesgames/unity-package-tools) to help with building releases. Install these tools from the release package on GitHub (we don't install via package manager as we don't want them to be installed as a dependency of our projects).

[Full usage instructions](https://github.com/jeffcampbellmakesgames/unity-package-tools/blob/master/usage.md) are available, the below is a summary for convenience.

  1. Update the Package Version Number
  2. Review all other settings
  3. Click Export Package
  4. In bash, in the export directory run the following commands:
  
  ```bash
  git init
  git remote add origin git@github.com:3dtbd/Common.git
  git checkout --orphan release/stable
  git add .
  git commit -m "Release vx.y.z"
  git push origin release/stable
  ```

