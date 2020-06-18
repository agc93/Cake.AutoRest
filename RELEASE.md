# Release Process

**TL;DR:**

1. `git clone git@github.com:agc93/Cake.AutoRest.git`
2. `git checkout -b release/<version-here> master`
3. `git rebase develop`
4. Update `ReleaseNotes.md` if necessary
5. `git checkout master` then `git merge --no-ff release/<version-here>`
6. Push and verify that CI ran successfully.
7. `git tag v<version-here>` and `git push --tags`
8. Rebase develop to master and push.
