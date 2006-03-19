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
	
	;StrCpy $0 0
	;StrCpy $1 0
	;IntOp $0 $0 + 1
	;StrCpy $1 '$1.$0'
	;StrLen $2 $1
	;IntCmp $2 1022 0 -3

	
	;		${registry::Open} "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\windows\CurrentVersion\InstallerUserData" "/K=0 /V=1 /S=0 /B=1 /N='URLInfoAbout'" $0
;	StrCmp $0 -1 0 loop
;	MessageBox MB_OK "Error" IDOK close

;	loop:
	;${registry::Find} $1 $2 $3 $4

	;MessageBox MB_OKCANCEL '$$1    "path"   =[$1]$\n\
	;			$$2    "value" =[$2]$\n\
	;			$$3    "string" =[$3]$\n\
	;			$$4    "type"   =[$4]$\n\
								$\n\
	;			Find next?' IDOK loop
	;close:
	;${registry::Close}
	;${registry::Unload}




	
	;${registry::Find} "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\windows\CurrentVersion\InstallerUserData" "http://www.zeroc.com" $var $TYPE
	;MessageBox MB_OK "registry::Find$\n$\n\ var je $var type je $TYPE"


	CreateDirectory "$INSTDIR\db"
	CreateDirectory "$INSTDIR\db\node"
	CreateDirectory "$INSTDIR\db\registry"

	SectionEnd

;hidden one
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
	Push "PATH"
	Push "c:\Ice-3.0.1\bin"
    Call AddToEnvVar
    
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
    Push "<IceBinPath>$INSTDIR\ThirdParty\ice\bin\</IceBinPath>"
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