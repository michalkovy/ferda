SectionGroup "$(AddIns)" FerdaAddIns

Section
CreateDirectory "$INSTDIR\FrontEnd\AddIns"
SectionEnd

Section "$(ResultBrowser)" ResultBrowser
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ResultBrowser.* 
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ResultBrowser" ""
SectionEnd

Section "$(ODBCConnectionString)" ODBCConnectionString
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ODBCConnectionString.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ODBCConnectionString" "" 
SectionEnd

Section "$(MultiSelectStrings)" MultiSelectStrings
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\MultiSelectStrings.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "MultiSelectStrings" "" 
SectionEnd

;addins help
Section
	SetOutPath "$INSTDIR\FrontEnd\AddIns\Help"
	File ..\bin\FrontEnd\AddIns\Help\*.*
SectionEnd


SectionGroupEnd