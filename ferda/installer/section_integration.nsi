SectionGroup "$(Integration)" FerdaIntegration

;create desktop shortcut and startmenu items
Section "$(StartMenu)" StartMenu
	CreateDirectory "$SMPROGRAMS\Ferda DataMiner"
	CreateShortCut "$SMPROGRAMS\Ferda DataMiner\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
	SetOutPath "$INSTDIR\FrontEnd\"
	CreateShortCut "$SMPROGRAMS\Ferda DataMiner\Ferda DataMiner.lnk" "$INSTDIR\FrontEnd\FerdaFrontEnd.exe" ""
SectionEnd

Section "$(DesktopShortcut)" DesktopShortcut
    SetOutPath "$INSTDIR\FrontEnd\"
	CreateShortCut "$DESKTOP\Ferda DataMiner.lnk" "$INSTDIR\FrontEnd\FerdaFrontEnd.exe" ""
SectionEnd


Section "$(FileAssociation)" FileAssociation
;associating .xfp files with Ferda
  ; back up old value of .xfp
!define Index "Line${__LINE__}"
  ReadRegStr $1 HKCR ".xfp" ""
  StrCmp $1 "" "${Index}-NoBackup"
    StrCmp $1 "FerdaDataMiner.Project" "${Index}-NoBackup"
    WriteRegStr HKCR ".xfp" "backup_val" $1
"${Index}-NoBackup:"
  WriteRegStr HKCR ".xfp" "" "FerdaDataMiner.Project"
  ReadRegStr $0 HKCR "FerdaDataMiner.Project" ""
  StrCmp $0 "" 0 "${Index}-Skip"
	WriteRegStr HKCR "FerdaDataMiner.Project" "" "Ferda DataMiner Project"
	WriteRegStr HKCR "FerdaDataMiner.Project\shell" "" "open"
	;WriteRegStr HKCR "FerdaDataMiner.Project\DefaultIcon" "" "$INSTDIR\FrontEnd\FerdaFrontEnd.ico,0"
"${Index}-Skip:"
WriteRegStr HKCR "FerdaDataMiner.Project\DefaultIcon" "" "$INSTDIR\FrontEnd\FerdaFrontEnd.ico,0"
  WriteRegStr HKCR "FerdaDataMiner.Project\shell\open\command" "" \
    '$INSTDIR\FrontEnd\FerdaFrontEnd.exe "%1"'
  WriteRegStr HKCR "FerdaDataMiner.Project\shell\edit" "" "Edit Options File"
  WriteRegStr HKCR "FerdaDataMiner.Project\shell\edit\command" "" \
    '$INSTDIR\FrontEnd\FerdaFrontEnd.exe "%1"'
!undef Index
SectionEnd

SectionGroupEnd