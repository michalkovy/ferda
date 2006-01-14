SectionGroup "$(Gens)" FerdaGens
             Section "4ft Gen" 4ftGen
             SetOutPath "$INSTDIR\Server\lispMinerGens"
             File ..\bin\Server\lispMinerGens\4ftGen.exe
             SectionEnd
             
             Section "KL Gen" KLGen
             SetOutPath "$INSTDIR\Server\lispMinerGens"
             File ..\bin\Server\lispMinerGens\KLGen.exe
             SectionEnd
             
             Section "CF Gen" CFGen
             SetOutPath "$INSTDIR\Server\lispMinerGens"
             File ..\bin\Server\lispMinerGens\CFGen.exe
             SectionEnd
             
             Section "SD4ft Gen" SD4ftGen
             SetOutPath "$INSTDIR\Server\lispMinerGens"
             File ..\bin\Server\lispMinerGens\SD4ftGen.exe
             SectionEnd
             
             Section "SDKL Gen" SD4KLGen
             SetOutPath "$INSTDIR\Server\lispMinerGens"
             File ..\bin\Server\lispMinerGens\SDKLGen.exe
             SectionEnd
             
             Section "SDCF Gen" SDCFGen
             SetOutPath "$INSTDIR\Server\lispMinerGens"
             File ..\bin\Server\lispMinerGens\SDCFGen.exe
             SectionEnd
SectionGroupEnd