SectionGroup "$(Basefiles)" BaseFiles
	Section "$(FerdaBaseFiles)" FerdaBase
	  
	SectionIn RO ;section is read-only, must be checked
	
	SetOutPath "$INSTDIR"
	File ..\AUTHORS
	File ..\COPYRIGHT
	File ..\INSTALL
	File ..\LICENSE
	File ..\NEWS
	File ..\README
	File ..\UNINSTALL
	  
	SetOutPath "$INSTDIR\FrontEnd"
	File ..\bin\FrontEnd\*.*
	
	SetOutPath "$INSTDIR\FrontEnd\Help"
	File ..\bin\FrontEnd\Help\*.*
	
	SetOutPath "$INSTDIR\FrontEnd\Icons"
	File ..\bin\FrontEnd\Icons\*.*
	
	SetOutPath "$INSTDIR\Server"
	File ..\bin\Server\*.*
	
	SetOutPath "$INSTDIR\Server\BoxModulesServices"
	File /r ..\bin\Server\BoxModulesServices\*.*
	  
	;Store installation folder
	WriteRegStr HKCU "Software\Ferda DataMiner\" "InstallDir" $INSTDIR
	WriteRegStr HKCU "Software\Ferda DataMiner\" "Version" ${FERDA_VERSION}
	
	;Create uninstaller
    WriteUninstaller "$INSTDIR\Uninstall.exe"
	  
	SectionEnd
	
	;Hidden section for installing all of the other necessary files
	Section
	
	CreateDirectory "$INSTDIR\db"
	CreateDirectory "$INSTDIR\db\node"
	CreateDirectory "$INSTDIR\db\registry"

	SectionEnd

;hidden section with task progress addin
Section
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\WaitDialog.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "WaitDialog" "" 
SectionEnd
	
	Section "$(IcePack)" Ice
	SectionIn RO ;section is read-only, must be checked
	;SetOutPath "$INSTDIR\ThirdParty\ice"
	;File /r ..\ThirdParty\ice\*.*
	
	SetOutPath "$INSTDIR\db"
	File ..\conf\db\application.xml
	File ..\conf\db\config
	
	SetOutPath "$INSTDIR\FrontEnd"
	File ..\conf\FrontEnd\FrontEndConfig.xml
	File ..\conf\FrontEnd\config
	
	SetOutPath "$INSTDIR"
	File ice-install.bat
	
	;CreateDirectory "$INSTDIR\ThirdParty\ice\bin"
	
	;adding ice to path
	StrCpy $R1 $IcePath
	;MessageBox MB_OK|MB_ICONEXCLAMATION "IcePath is $R1"
	Push "PATH"
	Push "$R1\bin"
    Call AddToEnvVar
    
    WriteRegStr HKCU "Software\Ferda DataMiner\" "IcePath" "$R1\bin"
    
    ;editing path in application.xml file
    Push "pwd=$\"bin/Server$\""
    Push "pwd=$\"$INSTDIR\Server\$\""
    Push all                      #-- replace all occurrences
    Push all                      #-- replace all occurrences
    Push $INSTDIR\db\application.xml      #-- file to replace in
    Call AdvReplaceInFile         #-- Call the Function

    ;Push "exepath=$\"./FerdaLMTasksBoxes.exe$\""             #-- text to be replaced  within the " "
    ;Push "exepath=$\"$INSTDIR\Server\FerdaLMTasksBoxes.exe$\""
    ;Push all                      #-- replace all occurrences
    ;Push all                      #-- replace all occurrences
    ;Push $INSTDIR\db\application.xml     #-- file to replace in
    ;Call AdvReplaceInFile         #-- Call the Function
    
    ;editing path in IceConfig.xml file

    Push "<IceBinPath />"
    Push "<IceBinPath>$R1\bin\</IceBinPath>"
    Push all
    Push all
    Push $INSTDIR\FrontEnd\FrontEndConfig.xml
    Call AdvReplaceInFile         #-- Call the Function


    ;direct installation of the service
    ; Service (manual starting)
    ;nsSCM::Install /NOUNLOAD "FerdaIceGridNode" "FerdaIceGridNode" 16 3 "$INSTDIR\ThirdParty\ice\bin\icegridnode.exe --service FerdaIceGridNode --Ice.Config=$\"$INSTDIR\db\config$\" --IceGrid.Registry.Data=$\"$INSTDIR\db\registry$\" --IceGrid.Node.Data=$\"$INSTDIR\db\node$\"" "" "" "" ""
                           
	;creating .bat for ice
    Push "icegridadmin --Ice.Config=config -e $\"application add $\'application.xml$\'$\""             #-- text to be replaced  within the " "
    Push "@echo off $\n $\"$INSTDIR\ThirdParty\ice\bin\icegridadmin$\" --Ice.Config=$\"$INSTDIR\db\config$\" -e $\"application add $\'$INSTDIR\db\application.xml$\'$\""             #-- replace with anything within the " "
    Push all                      #-- replace all occurrences
    Push all                      #-- replace all occurrences
    Push $INSTDIR\ice-install.bat     #-- file to replace in
    Call AdvReplaceInFile         #-- Call the Function

        
    
;need to start FerdaIceGridNode first, otherwise cannot add application.xml
;nsSCM::Start "FerdaIceGridNode" 
;executing the batch
;ExecWait "$INSTDIR\ice-install.bat"
;nsSCM::Stop "FerdaIceGridNode"


 SectionEnd
SectionGroupEnd