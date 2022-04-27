SectionGroup "$(AddIns)" FerdaAddIns

Section
CreateDirectory "$INSTDIR\FrontEnd\AddIns"
SectionEnd

Section "$(DatabaseInfo)" DatabaseInfo
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\DatabaseInfo.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "DatabaseInfo" "" 
SectionEnd

Section "$(EditCategories)" EditCategories
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\EditCategories.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "EditCategories" "" 
SectionEnd

Section "$(ExplainTable)" ExplainTable
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ExplainTable.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ExplainTable" "" 
SectionEnd

Section "$(FormEditor)" FormEditor
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\FormEditor.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "FormEditor" ""   
SectionEnd

Section "$(FormGenerator)" FormGenerator
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\FormGenerator.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "FormGenerator" ""   
SectionEnd

Section "$(FrequencyDisplayer)" FrequencyDisplayer
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\FrequencyDisplayer.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "FrequencyDisplayer" "" 
SectionEnd

Section "$(MultiSelectStrings)" MultiSelectStrings
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\MultiSelectStrings.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "MultiSelectStrings" "" 
SectionEnd

Section "$(ODBCConnectionString)" ODBCConnectionString
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ODBCConnectionString.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ODBCConnectionString" "" 
SectionEnd

Section "$(ResultBrowser)" ResultBrowser
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ResultBrowser.* 
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ResultBrowser" ""
SectionEnd

Section "$(SetOntologyMapping)" SetOntologyMapping
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\SetOntologyMapping.* 
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "SetOntologyMapping" ""
SectionEnd

Section "$(SetOntologyPath)" SetOntologyPath
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\SetOntologyPath.* 
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "SetOntologyPath" ""
SectionEnd

Section "$(ShowTable)" ShowTable
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ShowTable.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ShowTable" "" 
SectionEnd

Section "$(EditFuzzyCategories)" EditFuzzyCategories
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\EditFuzzyCategories.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "EditFuzzyCategories" "" 
SectionEnd

;addins help
Section
	SetOutPath "$INSTDIR\FrontEnd\AddIns\Help"
	File ..\bin\FrontEnd\AddIns\Help\*.*
SectionEnd


SectionGroupEnd
