7z.exe a -r ExchposerSetup.7z ..\Debug\*
copy /b 7zS.sfx + config.txt + ExchposerSetup.7z ExchposerSetup.exe
del ExchposerSetup.7z
