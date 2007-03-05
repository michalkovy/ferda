SectionGroup "$(AddIns)" FerdaAddIns

Section
CreateDirectory "$INSTDIR\FrontEnd\AddIns"
SectionEnd

Section "$(DatabaseInfo)" DatabaseInfo
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\DatabaseInfo.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "DatabaseInfo" "" 
SectionEnd

Section "$(ExplainTable)" ExplainTable
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ExplainTable.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ExplainTable" "" 
SectionEnd

Section "$(FrequencyDisplayer)" FrequencyDisplayer
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\FrequencyDisplayer.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "FrequencyDisplayer" "" 
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

Section "$(ShowTable)" ShowTable
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ShowTable.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ShowTable" "" 
SectionEnd

Section "$(EditCategories)" EditCategories
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\EditCategories.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "EditCategories" "" 
SectionEnd

;addins help
Section
	SetOutPath "$INSTDIR\FrontEnd\AddIns\Help"
	File ..\bin\FrontEnd\AddIns\Help\*.*
SectionEnd


SectionGroupEnd
