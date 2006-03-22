SectionGroup "$(AddIns)" FerdaAddIns

Section
CreateDirectory "$INSTDIR\FrontEnd\AddIns"
SectionEnd

Section "$(AttributeFrequency)" AttributeFrequency
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\AttributeFrequency.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "AttributeFrequency" ""
SectionEnd

Section "$(ColumnFrequency)" ColumnFrequency
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\ColumnFrequency.*
  WriteRegStr HKCU "Software\Ferda DataMiner\AddIns" "ColumnFrequency" ""
SectionEnd

Section "$(DatabaseInfo)" DatabaseInfo
  SetOutPath "$INSTDIR\FrontEnd\AddIns\"
  File ..\bin\FrontEnd\AddIns\DatabaseInfo.*
  ;Store installation folder
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

;addins help
Section
	SetOutPath "$INSTDIR\FrontEnd\AddIns\Help"
	File ..\bin\FrontEnd\AddIns\Help\*.*
SectionEnd


SectionGroupEnd