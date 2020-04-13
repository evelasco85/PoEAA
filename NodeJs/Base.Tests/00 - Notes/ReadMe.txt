[Install Mocha]
cmd: npm install mocha -g

[Run Specific Test File]
prereq.: Naviage test folder
cmd:    mocha <File> --unhandled-rejections=none

[Run Project Tests]
prereq.: Naviage test folder's parent
cmd: mocha <Folder>  --unhandled-rejections=none

[Run Project Tests - 2]
prereq.: Naviage test folder's parent
cmd: mocha ../<Folder>  --unhandled-rejections=none

[Class]
*Equivalent to 'struct' in C# where all members are public