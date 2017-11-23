##Importend Git Commands

List the files you've changed and those you still need to add or commit:
```
git status
```

Add one or more files to staging (index):
```
git add <filename>
```

Commit 	Commit changes to head (but not yet to the remote repository):
```
git commit -m "Commit message"
```

Fetch and merge changes on the remote server to your working directory:
```
git pull
```

Push the branch to your remote repository, so others can use it:
```
git push origin <branchname>
```

Create a new branch and switch to it:
```
git checkout -b <branchname>
```

Switch from one branch to another:
```
git checkout <branchname>
```

List all the branches in your repo, and also tell you what branch you're currently in:
```
git branch
```